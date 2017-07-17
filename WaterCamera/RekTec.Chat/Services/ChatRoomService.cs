using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.x.data;
using agsXMPP.protocol.x.muc;
using agsXMPP.protocol.x.muc.iq.admin;
using agsXMPP.protocol.x.muc.iq.owner;
using RekTec.Chat.DataRepository;
using RekTec.Chat.ViewModels;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;
using Item = agsXMPP.protocol.x.muc.iq.admin.Item;

namespace RekTec.Chat.Service
{
    internal class ChatRoomService
    {
        XmppClientConnection _chatConnection;
        MucManager _muc;

        internal ChatRoomService(XmppClientConnection chatConnection)
        {
            _chatConnection = chatConnection;
            _muc = new MucManager(_chatConnection);
            _chatConnection.OnMessage += OnInviteToRoom;
            _chatConnection.OnPresence += OnRoomPresence;
        }

        #region 被其他人从群中移除

        private void OnRoomPresence(object sender, Presence p)
        {
            if (p.Type != PresenceType.unavailable)
                return;

            var room = ChatDataRepository.GetRoomsById(p.From.Bare);
            if (room == null)
                return;

            if (p.MucUser == null || p.MucUser.Item == null)
                return;
                
            if (p.MucUser != null && p.MucUser.Item != null && p.MucUser.Item.Jid != null) {
                MemberBeRemoved(p, room);
            } else if (p.From.Resource == ChatClient.CurrentUserContact.ContactCode && p.MucUser.Item.Role == Role.none) {
                ChatDataRepository.Remove(room);
            }
        }

        private void MemberBeRemoved(Presence p, ChatRoomViewModel room)
        {
            if (p.MucUser == null)
                return;

            if (p.MucUser.Item == null)
                return;

            var jid = p.MucUser.Item.Jid;
            if (jid == null)
                return;

            if (p.MucUser.Item.Role == Role.none) {
                ChatDataRepository.Remove(new ChatRoomMemberViewModel {
                    ChatRoomId = room.ChatRoomId,
                    ContactId = jid.Bare
                });

                if (jid.Bare == ChatClient.CurrentUserContact.ContactId)
                    ChatDataRepository.Remove(room);
            } else if (p.MucUser.Status != null && p.MucUser.Status.Code == StatusCode.Kicked) {
                ChatDataRepository.Remove(new ChatRoomMemberViewModel {
                    ChatRoomId = room.ChatRoomId,
                    ContactId = jid.Bare
                });

                if (jid.Bare == ChatClient.CurrentUserContact.ContactId)
                    ChatDataRepository.Remove(room);
            }
        }

        #endregion

        #region 邀请和接受群聊邀请

        private void OnInviteToRoom(object sender, Message msg)
        {
            try {
                if (msg.Type == MessageType.error) {
                    ErrorHandlerUtil.ReportError(msg.Error.ToString());
                    return;
                }

                if (msg.Type == MessageType.normal && msg.SelectSingleElement(typeof(Invite), true) != null) {
                    AcceptRoomInvite(msg);
                    return;
                }
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        private void AcceptRoomInvite(Message msg)
        {
            MucManager muc = new MucManager(_chatConnection);
            var invite = msg.SelectSingleElement(typeof(Invite), true) as Invite;
            muc.JoinRoom(msg.From, ChatClient.CurrentUserContact.ContactCode);
            ChatDataRepository.AddOrUpdate(new ChatRoomViewModel {
                ChatRoomId = msg.From.Bare,
                ChatRoomName = invite.Reason
            });
                    
            var inviteFromContact = ContactsDataRepository.GetContactById(invite.From.Bare);
            var inviteFromFullName = inviteFromContact != null ? inviteFromContact.ContactName : invite.From.Bare;

            MessagesDataRepository.AddOrUpdate(new ChatListViewModel {
                ChatListId = msg.From.Bare,
                ChatListName = string.IsNullOrWhiteSpace(invite.Reason) ? "群聊" : invite.Reason,
                LastMessageContent = inviteFromFullName + "邀请你加入群聊",
                LastMessageDateTime = DateTime.Now,
                ListType = ChatListType.Group
            });
        }

        internal void InviteContactsToRoom(List<ContactViewModel> contacts, ChatRoomViewModel room)
        {
            try {
                var muc = new MucManager(_chatConnection);

                contacts.ForEach((c) => {
                    muc.Invite(new Jid(c.ContactId), new Jid(room.ChatRoomId), room.ChatRoomName);
                    muc.GrantAdminPrivileges(new Jid(room.ChatRoomId), new Jid(c.ContactId));

                    ChatDataRepository.AddOrUpdate(new ChatRoomMemberViewModel {
                        ChatRoomId = room.ChatRoomId,
                        ContactId = c.ContactId,
                        Id = Guid.NewGuid().ToString(),
                        IsOwner = false
                    });

                });
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        #endregion

        #region 移除群聊成员

        internal void RemoveRoomMember(ChatRoomMemberViewModel roomMember)
        {
            try {
                var contact = ContactsDataRepository.GetContactById(roomMember.ContactId);

                if (contact == null)
                    return;

                ChangeAffiliation(Affiliation.member, new Jid(roomMember.ChatRoomId), new Jid(roomMember.ContactId), (sender1, iq1, data1) => {
                    ChangeAffiliation(Affiliation.none, new Jid(roomMember.ChatRoomId), new Jid(roomMember.ContactId), (sender2, iq2, data2) => {
                        _muc.ChangeRole(Role.visitor, new Jid(roomMember.ChatRoomId), contact.ContactCode, "x", (sender3, iq3, data3) => {
                            _muc.ChangeRole(Role.none, new Jid(roomMember.ChatRoomId), contact.ContactCode, "x", (sender4, iq4, data4) => {
                                ChatDataRepository.Remove(roomMember);
                            }, null);
          
                        }, null);

                    }, null);
                }, null);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        internal void LeaveRoomMember(ChatRoomMemberViewModel roomMember)
        {
            try {
                //var contact = ChatDataRepository.GetContactById(roomMember.ContactId);

                //if (contact == null)
                //    return;

                ChangeAffiliation(Affiliation.none, new Jid(roomMember.ChatRoomId), new Jid(roomMember.ContactId), (sender1, iq1, data1) => {
                    ChatDataRepository.Remove(roomMember);
                }, null);

            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        private void ChangeAffiliation(Affiliation affiliation, Jid room, Jid member, IqCB cb, object cbArg)
        {
            var aIq = new AdminIq();
            aIq.To = room;
            aIq.Type = IqType.set;

            Item itm = new Item();
            itm.Affiliation = affiliation;               
            itm.Jid = member;
            aIq.Query.AddItem(itm);

            _chatConnection.IqGrabber.SendIq(aIq, cb, cbArg, false);
        }

        #endregion

        #region 退出群聊

        internal void LeaveRoom(ChatRoomViewModel room)
        {
            try {
                var isOwner = false;
                var currentMember = ChatDataRepository.GetRoomMemberById(room.ChatRoomId, ChatClient.CurrentUserContact.ContactId);

                if (currentMember == null) {
                    currentMember = new ChatRoomMemberViewModel {
                        Id = Guid.NewGuid().ToString(),
                        ChatRoomId = room.ChatRoomId,
                        ContactId = ChatClient.CurrentUserContact.ContactId,
                        IsOwner = false
                    };
                }

                isOwner = currentMember.IsOwner;
             
                if (isOwner) {
//                    var members = ChatDataRepository.GetRoomMembersByRoomId(room.ChatRoomId);
//                    foreach (var m in members) {
//                        RemoveRoomMember(m);
//                    }

                    _muc.DestroyRoom(new Jid(room.ChatRoomId), string.Empty);
                } else {
                    LeaveRoomMember(currentMember);
                    _muc.LeaveRoom(new Jid(room.ChatRoomId), ChatClient.CurrentUserContact.ContactCode);
                }
                ChatDataRepository.Remove(room);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        #endregion

        #region 获取群聊的成员列表

        internal void GetRoomMembers(Jid room, Action cb)
        {
            GetRoomAdminList(room, cb);
            GetRoomOwnerList(room, cb);
        }

        private void GetRoomAdminList(Jid room, Action cb)
        {
            try {
                var aIq = new AdminIq();
                aIq.To = room;
                aIq.Type = IqType.get;

                aIq.Query.AddItem(new Item(Affiliation.admin));
                _chatConnection.IqGrabber.SendIq(aIq, (rSender, rIq, rData) => {
                    if (rIq.Type == IqType.error) {
                        ErrorHandlerUtil.ReportError(rIq.Error.ToString());
                        return;
                    }
                    if (rIq.Type == IqType.result) {
                        try {
                            var items = rIq.Query.SelectElements<Item>();

                            foreach (var item in items) {
                                ChatDataRepository.AddOrUpdate(new ChatRoomMemberViewModel {
                                    Id = Guid.NewGuid().ToString(),
                                    ChatRoomId = room.Bare,
                                    ContactId = item.Jid.Bare,
                                    IsOwner = false
                                });
                            }

                            if (cb != null)
                                cb();
                        } catch (Exception ex) {
                            ErrorHandlerUtil.ReportException(ex);
                        }
                    }
                }, null, false);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        private void GetRoomOwnerList(Jid room, Action cb)
        {
            try {
                var aIq = new AdminIq();
                aIq.To = room;
                aIq.Type = IqType.get;

                aIq.Query.AddItem(new Item(Affiliation.owner));
                _chatConnection.IqGrabber.SendIq(aIq, (rSender, rIq, rData) => {
                    try {
                        if (rIq.Type == IqType.error) {
                            ErrorHandlerUtil.ReportError(rIq.Error.ToString());
                            return;
                        }
                        var items = rIq.Query.SelectElements<Item>();

                        foreach (var item in items) {
                            ChatDataRepository.AddOrUpdate(new ChatRoomMemberViewModel {
                                Id = Guid.NewGuid().ToString(),
                                ChatRoomId = room.Bare,
                                ContactId = item.Jid.Bare,
                                IsOwner = true
                            });
                        }

                        if (cb != null)
                            cb();
                    } catch (Exception ex) {
                        ErrorHandlerUtil.ReportException(ex);
                    }
                }, null, false);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        #endregion

        #region 创建群聊

        internal void CreateRoom(List<ContactViewModel> contacts, Action<ChatListViewModel> cb)
        {
            if (contacts == null || contacts.Count == 0)
                return;

            var roomId = new Jid(Guid.NewGuid().ToString() + "@" + ChatAppSetting.HostConferenceName);

            ChatDataRepository.AddOrUpdate(new ChatRoomMemberViewModel {
                ChatRoomId = roomId.Bare,
                ContactId = ChatClient.CurrentUserContact.ContactId,
                Id = Guid.NewGuid().ToString(),
                IsOwner = true
            });


            var roomName = contacts.Select((c) => c.ContactName)
                .ToArray().Aggregate((r, n) => r + "," + n);
            roomName += "," + ChatClient.CurrentUserContact.ContactName;

            _muc.JoinRoom(roomId, ChatClient.CurrentUserContact.ContactCode);

            var room = new ChatRoomViewModel {
                ChatRoomId = roomId.Bare,
                ChatRoomName = roomName
            };


            var iqSend = new OwnerIq();
            iqSend.Type = IqType.get;
            iqSend.To = roomId;

            _chatConnection.IqGrabber.SendIq(iqSend, (sender, iq, data) => {
                try {
                    if (iq.Type == IqType.error) {
                        ErrorHandlerUtil.ReportError(iq.Error.ToString());
                        return;
                    }

                    ChatDataRepository.AddOrUpdate(room);

                    SetRoomConfiguration(roomId, roomName, (nameSender, nameIq, nameData) => {
                        if (nameIq.Type == IqType.error) {
                            ErrorHandlerUtil.ReportError(nameIq.Error.ToString());
                            return;
                        }

                        _muc.ChangeSubject(roomId, roomName);
                        InviteContactsToRoom(contacts, room);

                        if (cb != null) {
                            var chatList = new ChatListViewModel {
                                ChatListId = roomId.Bare,
                                ChatListName = roomName,
                                ListType = ChatListType.Group,
                                LastMessageDateTime = DateTime.Now,
                                LastMessageContent = "你邀请" + roomName + "加入群聊"
                            };
                            MessagesDataRepository.AddOrUpdate(chatList);
                            cb(chatList);
                        }
                    });
                } catch (Exception ex) {
                    ErrorHandlerUtil.ReportException(ex);
                }
            }, null, false);
        }

        internal void ChangeRoomName(ChatRoomViewModel room)
        {
            try {
                var muc = new MucManager(_chatConnection);
                muc.ChangeSubject(new Jid(room.ChatRoomId), room.ChatRoomName);
                ChatDataRepository.AddOrUpdate(room);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        private void SetRoomConfiguration(Jid room, string roomName, IqCB cb)
        {
            var oIq = new OwnerIq(IqType.set, room);

            var submitData = new Data(XDataFormType.submit);
            var configF = new Field {
                Var = "FORM_TYPE",
                Type = FieldType.Hidden
            };
            configF.AddValue("http://jabber.org/protocol/muc#roomconfig");
            submitData.AddField(configF);

            var nameF = new Field {
                Var = "muc#roomconfig_roomname",
                Type = FieldType.Text_Single
            };
            nameF.AddValue(roomName);
            submitData.AddField(nameF);

            var descF = new Field {
                Var = "muc#roomconfig_roomdesc",
                Type = FieldType.Text_Single
            };
            descF.AddValue(roomName);
            submitData.AddField(descF);

            var memberF = new Field {
                Var = "muc#roomconfig_membersonly",
                Type = FieldType.Boolean
            };
            memberF.AddValue("1");
            submitData.AddField(memberF);
                    
            var persistentF = new Field {
                Var = "muc#roomconfig_persistentroom",
                Type = FieldType.Boolean
            }; 
            persistentF.AddValue("1");
            submitData.AddField(persistentF);

            var internalF = new Field {
                Var = "muc#roomconfig_internalroom",
                Type = FieldType.Boolean
            }; 
            internalF.AddValue("0");
            submitData.AddField(internalF);

            var maxF = new Field {
                Var = "muc#roomconfig_maxusers",
                Type = FieldType.List_Single
            }; 
            maxF.AddValue("200");
            submitData.AddField(maxF);

            var publicF = new Field {
                Var = "muc#roomconfig_publicroom",
                Type = FieldType.Boolean
            }; 
            publicF.AddValue("0");
            submitData.AddField(publicF);

            var moderateF = new Field {
                Var = "muc#roomconfig_moderatedroom",
                Type = FieldType.Boolean
            }; 
            moderateF.AddValue("1");
            submitData.AddField(moderateF);

            var inviteF = new Field {
                Var = "muc#roomconfig_allowinvites",
                Type = FieldType.Boolean
            }; 
            inviteF.AddValue("0");
            submitData.AddField(inviteF);


            oIq.Query.AddChild(submitData);

            _chatConnection.IqGrabber.SendIq(oIq, cb, null, false);

        }

        #endregion

        #region 获取当前用户的所有加入的群组

        private void JoinRoom(ChatRoomViewModel room)
        {
            try {
                if (room == null || room.ChatRoomId == null)
                    return;
                _muc.JoinRoom(new Jid(room.ChatRoomId), ChatClient.CurrentUserContact.ContactCode);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        internal void GetAllMyRoomsAndJoin()
        {
            new Timer((state) => {
                var discIq = new DiscoItemsIq(IqType.get) {
                    Id = Guid.NewGuid().ToString(),
                    To = new Jid(ChatAppSetting.HostConferenceName),
                    From = new Jid(ChatClient.CurrentUserContact.ContactId)
                };

                _chatConnection.IqGrabber.SendIq(discIq, (sender, iq, data) => {
                    if (iq.Type == IqType.error) {
                        ErrorHandlerUtil.ReportError(iq.Error.ToString());
                        return;
                    }

                    var dItems = iq.SelectSingleElement<DiscoItems>();
                    var items = dItems.GetDiscoItems();

                    #region 删除本地存在，但是服务器上已经不存在的Room
                    var rooms = ChatDataRepository.GetAllRooms();
                    rooms.ForEach(r => {
                        var item = items.FirstOrDefault(i => i.Jid.Bare == r.ChatRoomId);
                        if (item == null)
                            ChatDataRepository.Remove(r);
                    });
                    #endregion

                    //获取每个Room的主题，并且自动加入Room
                    foreach (var item in items) {
                        GetRoomConfiguration(item.Jid);
                    }
                }, null, false);
            }, null, 5 * 1000, -1);
        }

        private void GetRoomConfiguration(Jid room)
        {
            var oIq = new DiscoInfoIq(IqType.get);
            oIq.Id = Guid.NewGuid().ToString();
            oIq.To = room;

            _chatConnection.IqGrabber.SendIq(oIq, (sender, iq, data) => {
                if (iq.Type == IqType.error) {
                    ErrorHandlerUtil.ReportError(iq.Error.ToString());
                    return;
                }

                var fields = iq.SelectElements<Field>(true);
                var subjectF = fields.FirstOrDefault(f => f.Var == "muc#roominfo_subject");
                if (subjectF != null) {
                    var r = new ChatRoomViewModel {
                        ChatRoomId = room.Bare,
                        ChatRoomName = subjectF.GetValue()
                    };
                    ChatDataRepository.AddOrUpdate(r);

                    JoinRoom(r);
                }
            }, null, false);
        }

        #endregion
    }
}

