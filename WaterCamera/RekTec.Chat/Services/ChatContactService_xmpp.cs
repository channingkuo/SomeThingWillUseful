using System;
using System.Linq;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.vcard;

namespace RekTec.Chat.Common
{
    internal class ChatContactService_xmpp
    {
        private readonly XmppClientConnection _xmppConnection;
        private  TaskCompletionSource<bool> _taskWaitor = null;

        internal ChatContactService_xmpp(XmppClientConnection conn)
        {
            _xmppConnection = conn;
            _xmppConnection.AutoPresence = false;
            _xmppConnection.AutoRoster = false;

            _xmppConnection.OnStreamError += (sender, e) => {
                TryCompleteTask(false);
            };

            _xmppConnection.OnSocketError += (sender, ex) => {
                TryCompleteTask(false);
            };
        }

        private void NewRosterEvent(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            if (item == null || item.Jid == null)
                return;

            try {
                var contact = ChatDataRepository.GetContactById(item.Jid.Bare);
                if (contact == null) {
                    contact = new ContactViewModel {
                        ContactId = item.Jid.Bare,
                        ContactName = item.Name == null ? item.Jid.Bare : item.Name
                    };
                    ChatDataRepository.AddOrUpdate(contact);
                }

                var task = GetContactVcard(contact);
                task.Wait(new TimeSpan(0, 0, 5)); //最多等待5s
            } catch (Exception ex) {
                ErrorUtil.ReportException(ex);
            }
        }

        private void TryCompleteTask(bool isSuccess)
        {
            if (_taskWaitor != null) {
                _taskWaitor.TrySetResult(isSuccess);
                _taskWaitor = null;
            }
        }

        internal Task<bool> GetAllContacts()
        {
            try {
//                var count = ChatDataRepository.GetContactsCount();
//                var myVcard = ChatDataRepository.GetContactById(ChatClient.CurrentUserContact.ContactId);
//                if (count > 0 && myVcard != null) {
//                    _xmppConnection.OnRosterItem += NewRosterEvent;
//                    return Task.Run(() => true);
//                }

                var task = GetAllContactIds();
                task.Wait();
                if (!task.Result)
                    return task;

//                foreach (var c in ChatDataRepository.GetAllContacts()) {
//                    task = GetContactVcard(c);
//                    task.Wait();
//                    if (!task.Result)
//                        return task;
//                }
//                
//                task = GetMyVcard();
//                task.Wait();
//                if (!task.Result)
//                    return task;

                //_xmppConnection.OnRosterItem += NewRosterEvent;

                return task;
            } catch (Exception ex) {
                ErrorUtil.ReportException(ex);
                return Task.Run(() => false);
            }
        }

        private Task<bool> GetAllContactIds()
        {
            TryCompleteTask(false);
            _taskWaitor = new TaskCompletionSource<bool>();

            var rosterIq = new agsXMPP.protocol.iq.roster.RosterIq(IqType.get);
            _xmppConnection.IqGrabber.SendIq(rosterIq, (sender, retIq, data) => {
                try {
                    if (retIq.Type == IqType.error) {
                        ErrorUtil.ReportError(retIq.Error.ToString());
                        TryCompleteTask(false);
                        return;
                    }
                    if (retIq.Type != IqType.result) {
                        ErrorUtil.ReportError("获取所有联系人Id出错！");
                        TryCompleteTask(false);
                        return;
                    }

                    var r = retIq.Query as agsXMPP.protocol.iq.roster.Roster;
                    if (r == null) {
                        ErrorUtil.ReportError("获取所有联系人Id出错！");
                        TryCompleteTask(false);
                        return;
                    }

                    foreach (var item in r.GetRoster()) {
                        var jid = item.Jid.Bare;
                        var c = ChatDataRepository.GetContactById(jid);
                        if (c == null) {
                            ChatDataRepository.AddOrUpdate(new ContactViewModel {
                                ContactId = item.Jid.ToString()
                            });
                        }
                    }

                    TryCompleteTask(true);
                } catch (Exception ex) {
                    TryCompleteTask(false);
                    ErrorUtil.ReportException(ex);
                }
            }, null, false);

            return _taskWaitor.Task;
        }

        internal Task<bool> GetContactVcard(ContactViewModel c)
        {
            TryCompleteTask(false);
            _taskWaitor = new TaskCompletionSource<bool>();

            var viq = new VcardIq(IqType.get, new Jid(c.ContactId), new Jid(ChatClient.CurrentUserContact.ContactId)) {
                Id = Guid.NewGuid().ToString()
            };

            _xmppConnection.IqGrabber.SendIq(viq, (s, iq, data) => {
                try {
                    if (iq.Type == IqType.error) {
                        ErrorUtil.ReportError(iq.Error.ToString());
                        TryCompleteTask(false);
                        return;
                    }

                    if (iq.Type != IqType.result) {
                        ErrorUtil.ReportError("获取所有联系人Vcard出错！");
                        TryCompleteTask(false);
                        return;
                    }


                    var requestContact = data as ContactViewModel;

                    var contact = ChatDataRepository.GetContactById(requestContact.ContactId);
                    if (contact != null) {
                        contact.ContactName = iq.Vcard.Fullname;
                        string avatar = null;
                        if (iq.Vcard.Photo != null)
                            avatar = iq.Vcard.Photo.GetTag("BINVAL");
                        if (string.IsNullOrWhiteSpace(avatar))
                            avatar = iq.Vcard.GetTag("BINVAL");

                        contact.AvatarImageBase64String = avatar;
                        var workPhone = iq.Vcard.GetTelephoneNumber(TelephoneType.VOICE, TelephoneLocation.WORK);
                        if (workPhone != null)
                            contact.WorkPhone = workPhone.Number;
                        var cellPhone = iq.Vcard.GetTelephoneNumber(TelephoneType.CELL, TelephoneLocation.WORK);
                        if (cellPhone != null)
                            contact.CellPhone = cellPhone.Number;

                        ChatDataRepository.AddOrUpdate(contact);
                    }

                    TryCompleteTask(true);
                } catch (Exception ex) {
                    ErrorUtil.ReportException(ex);
                    TryCompleteTask(false);
                }

            }, c, false);

            return _taskWaitor.Task;
        }


        private void SubscribeContactPresenceEvent()
        {
            _xmppConnection.OnPresence += (sender, pres) => {
                try {
                    if (pres.Type == PresenceType.subscribe) {
                    } else if (pres.Type == PresenceType.subscribed) {
                    } else if (pres.Type == PresenceType.unsubscribe) {
                    } else if (pres.Type == PresenceType.unsubscribed) {
                    } else if (pres.Type == PresenceType.error) {
                    } else {
                        if (pres.From.Bare == pres.To.Bare) {
                            ChatClient.CurrentUserContact.ContactId = pres.From.Bare;
                            return;
                        }

                        var contact = ChatDataRepository.GetContactById(pres.From.Bare) ??
                                      new ContactViewModel { ContactId = pres.From.Bare };

                        contact.IsOnline = pres.Type == PresenceType.available;
                        contact.StatusType = pres.Show.ToString();
                        contact.StatusDescription = pres.Status;

                        ChatDataRepository.AddOrUpdate(contact);
                    }
                } catch (Exception ex) {
                    ErrorUtil.ReportException(ex);
                }
            };
        }

        private Task<bool> GetMyVcard()
        {
            TryCompleteTask(false);
            _taskWaitor = new TaskCompletionSource<bool>();

            var viq = new VcardIq(IqType.get, new Jid(ChatClient.CurrentUserContact.ContactId), new Jid(ChatClient.CurrentUserContact.ContactId)) {
                Id = Guid.NewGuid().ToString()
            };

            _xmppConnection.IqGrabber.SendIq(viq, (s, iq, data) => {
                try {
                    if (iq.Type == IqType.error) {
                        ErrorUtil.ReportError(iq.Error.ToString());
                        TryCompleteTask(false);
                        return;
                    }

                    if (iq.Type != IqType.result) {
                        ErrorUtil.ReportError("获取我的Vcard出错！");
                        TryCompleteTask(false);
                        return;
                    }

                    var requestContact = data as ContactViewModel;
                    if (requestContact == null) {
                        ErrorUtil.ReportError("获取我的Vcard出错！");
                        TryCompleteTask(false);
                        return;
                    }
                    if (requestContact.ContactCode != ChatClient.CurrentUserContact.ContactCode) {
                        ErrorUtil.ReportError("获取我的Vcard出错！");
                        TryCompleteTask(false);
                        return;
                    }


                    ChatClient.CurrentUserContact.ContactName = iq.Vcard.Fullname;

                    if (iq.Vcard.Photo != null)
                        ChatClient.CurrentUserContact.AvatarImageBase64String = iq.Vcard.Photo.GetTag("BINVAL");

                    var workPhone = iq.Vcard.GetTelephoneNumber(TelephoneType.VOICE, TelephoneLocation.WORK);
                    if (workPhone != null)
                        ChatClient.CurrentUserContact.WorkPhone = workPhone.Number;
                    var cellPhone = iq.Vcard.GetTelephoneNumber(TelephoneType.CELL, TelephoneLocation.WORK);
                    if (cellPhone != null)
                        ChatClient.CurrentUserContact.CellPhone = cellPhone.Number;

                    ChatDataRepository.AddOrUpdate(ChatClient.CurrentUserContact);

                    TryCompleteTask(true);
                } catch (Exception ex) {
                    TryCompleteTask(false);
                    ErrorUtil.ReportException(ex);
                }

            }, ChatClient.CurrentUserContact, false);
                    
            return _taskWaitor.Task;
        }

        internal void SetMyVcard(Action<string> cb)
        {
            var vcard = new Vcard {
                JabberId = new Jid(ChatClient.CurrentUserContact.ContactCode + "@" + ChatAppSetting.HostName),
                Fullname = ChatClient.CurrentUserContact.ContactName,
                Nickname = ChatClient.CurrentUserContact.ContactName
            };
            vcard.AddTelephoneNumber(new Telephone {
                Type = TelephoneType.VOICE,
                Location = TelephoneLocation.WORK,
                Number = ChatClient.CurrentUserContact.WorkPhone
            });
            vcard.AddTelephoneNumber(new Telephone {
                Type = TelephoneType.CELL,
                Location = TelephoneLocation.WORK,
                Number = ChatClient.CurrentUserContact.CellPhone
            });
            vcard.Photo = new Photo();
            vcard.Photo.SetTag("BINVAL", ChatClient.CurrentUserContact.AvatarImageBase64String);

            var viq = new VcardIq(IqType.set, new Jid(ChatClient.CurrentUserContact.ContactId), vcard) {
                Id = Guid.NewGuid().ToString()
            };
            _xmppConnection.IqGrabber.SendIq(viq, (s, iq, data) => {
                if (iq.Type == IqType.error) {
                    cb(iq.ToString());
                }

                if (iq.Type == IqType.result) {
                    cb(string.Empty);
                }
            }, null);
        }
    }
}

