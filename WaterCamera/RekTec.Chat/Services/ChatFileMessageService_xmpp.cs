using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using System.Threading.Tasks;

namespace RekTec.Chat.Common
{
    internal class ChatFileMessageService_xmpp:ChatBaseMessageService
    {
        private readonly int _maxPackageLength = 4 * 1024;

        internal ChatFileMessageService_xmpp(XmppClientConnection conn, ChatContactService contactService)
            : base(conn, contactService)
        {
        }

        #region 接收消息部分的代码

        protected override void ReceiveMessage(Message msg)
        {
            if (!isFileMessage(msg))
                return;

            var fileId = string.Empty;
           
            var e = GetChildElementByTagName(msg, ChatFileOpenElement.ElementTagName);
            if (e != null) {
                var fileOpen = ChatFileOpenElement.Parse(e);
                fileId = fileOpen.FileId;
                var vm = new ChatFileOpenViewModel {
                    ChatFileOpenId = msg.Id,
                    FileId = fileOpen.FileId,
                    FileType = fileOpen.FileType,
                    PackageCount = fileOpen.PackageCount,
                    MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                    MessageType = ChatMessageType.Receive,
                    MessageSendStatus = ChatMessageSendStatus.Sent
                };
                ChatDataRepository.AddOrUpdate(vm);
            }

            e = GetChildElementByTagName(msg, ChatFilePackageElement.ElementTagName);
            if (e != null) {
                var filePackage = ChatFilePackageElement.Parse(e);
                fileId = filePackage.FileId;
                var vm = new ChatFilePackageViewModel {
                    ChatFilePackageId = msg.Id,
                    FileId = filePackage.FileId,
                    Value = filePackage.Value,
                    Seq = filePackage.Seq,
                    MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                    MessageType = ChatMessageType.Receive,
                    MessageSendStatus = ChatMessageSendStatus.Sent
                };
                ChatDataRepository.AddOrUpdate(vm);
            }

            e = GetChildElementByTagName(msg, ChatFileCloseElement.ElementTagName);
            if (e != null) {
                var fileClose = ChatFileCloseElement.Parse(e);
                fileId = fileClose.FileId;
                var vm = new ChatFileCloseViewModel {
                    ChatFileCloseId = msg.Id,
                    FileId = fileClose.FileId,
                    MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                    MessageType = ChatMessageType.Receive,
                    MessageSendStatus = ChatMessageSendStatus.Sent
                };
                ChatDataRepository.AddOrUpdate(vm);
            }

            var exitsFileOpen = ChatDataRepository.GetFileOpenByFileId(fileId);
            if (exitsFileOpen == null)
                return;

            var exitsFileClose = ChatDataRepository.GetFileCloseByFileId(fileId);
            if (exitsFileClose == null)
                return;

            var exitsFilePackageList = ChatDataRepository.GetFilePackagesByFileId(fileId);
            if (exitsFilePackageList.Count != exitsFileOpen.PackageCount)
                return;

            bool isFromMe = false;
            if (msg.From.Bare == ChatClient.CurrentUserContact.ContactId)
                isFromMe = true;
            if (msg.From.Resource == ChatClient.CurrentUserContact.ContactCode)
                isFromMe = true;

            var message = new ChatMessageViewModel() {
                ChatMessageId = fileId,
                ChatRoomId = msg.Type == MessageType.groupchat ? msg.From.Bare : string.Empty,
                MessageContentType = ChatMessageContentType.Image,
                MessageType = isFromMe ? ChatMessageType.Send : ChatMessageType.Receive,
                MessageSendStatus = ChatMessageSendStatus.Sent,
                MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                FromId = msg.From.Bare,
                FromResource = msg.From.Resource,
                ToId = ChatClient.CurrentUserContact.ContactId,
                MessageListType = msg.Type == MessageType.chat ? ChatListType.Private : ChatListType.Group,
                SendDateTime = msg.XDelay != null ? msg.XDelay.Stamp : DateTime.Now,
            };
            var filePackages = exitsFilePackageList.OrderBy(f => f.Seq).ToList();
            for (var i = 0; i < filePackages.Count; i++) {
                message.MessageContent += filePackages[i].Value;
            }

            ChatDataRepository.AddOrUpdate(message);
        }

        private bool isFileMessage(Message msg)
        {
            if (msg == null)
                return false;

            if (GetChildElementByTagName(msg, ChatFileOpenElement.ElementTagName) != null)
                return true;

            if (GetChildElementByTagName(msg, ChatFilePackageElement.ElementTagName) != null)
                return true;

            if (GetChildElementByTagName(msg, ChatFileCloseElement.ElementTagName) != null)
                return true;
                
            return false;
        }

        private Element GetChildElementByTagName(Message msg, string tagName)
        {
            if (msg == null || string.IsNullOrWhiteSpace(tagName))
                return null;

            foreach (agsXMPP.Xml.Dom.Node n in msg.ChildNodes) {
                var e = n as agsXMPP.Xml.Dom.Element;
                if (e == null)
                    continue;

                if (e.TagName.ToLower() == tagName.ToLower())
                    return e;
            }

            return null;
        }

        #endregion

        #region 构造xmpp的文件element的部分

        private ChatFileOpenElement GetFileOpenElement(ChatMessageViewModel message, string fileContent)
        {
            var c = (int)Math.Ceiling((float)fileContent.Length / _maxPackageLength);

            return new ChatFileOpenElement {
                FileId = message.ChatMessageId,
                PackageCount = c
            };
        }

        private ChatFileCloseElement GetFileCloseElement(ChatFileOpenElement fileOpen)
        {
            return new ChatFileCloseElement {
                FileId = fileOpen.FileId
            };
        }

        private List<ChatFilePackageElement> GetFilePackageElements(ChatFileOpenElement file, ChatMessageViewModel message, string content)
        {
            var c = file.PackageCount;
            var l = new List<ChatFilePackageElement>(c);
            for (var i = 0; i < c; i++) {
                var len = _maxPackageLength;
                if (content.Length - (i * _maxPackageLength) < _maxPackageLength)
                    len = content.Length - (i * _maxPackageLength);

                l.Add(new ChatFilePackageElement {
                    FileId = file.FileId,
                    Seq = i,
                    Value = content.Substring(i * _maxPackageLength, len)
                });
            }

            return l;
        }

        #endregion

        #region 发送文件部分的代码

        private async Task SendFileAsync(ChatListViewModel chatList, ChatMessageViewModel message, string fileType)
        {
            if (chatList == null || message == null)
                return;

            try {
                message.MessageListType = chatList.ListType;
                if (chatList.ListType == ChatListType.Private)
                    message.FromResource = _clientImp.Resource;
                if (message.MessageListType == ChatListType.Group)
                    message.ChatRoomId = message.ToId;

                ChatDataRepository.AddOrUpdate(message);
           
                var fileContent = FileSystemUtil.GetBase64StringFromCache(message.ChatMessageId);

                #region file transfer open
                var fileOpen = GetFileOpenElement(message, fileContent);
                fileOpen.FileType = fileType;
                var fileOpenVM = new ChatFileOpenViewModel {
                    ChatFileOpenId = Guid.NewGuid().ToString(),
                    FileId = fileOpen.FileId,
                    FileType = fileOpen.FileType,
                    PackageCount = fileOpen.PackageCount,
                    MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                    MessageType = ChatMessageType.Send,
                    MessageSendStatus = ChatMessageSendStatus.NotSend
                };
                ChatDataRepository.AddOrUpdate(fileOpenVM);

                var msgFileOpen = new Message();
                msgFileOpen.Id = fileOpenVM.ChatFileOpenId;
                msgFileOpen.Type = message.MessageListType == ChatListType.Private ? MessageType.chat : MessageType.groupchat;
                msgFileOpen.To = new Jid(chatList.ChatListId);
                msgFileOpen.From = new Jid(ChatClient.CurrentUserContact.ContactId);
                msgFileOpen.AddChild(fileOpen);
                await _clientImp.SendAsync(msgFileOpen, msgFileOpen.Id).ConfigureAwait(false);
                #endregion

                #region file transfer package
                var filePagckages = GetFilePackageElements(fileOpen, message, fileContent);
                foreach (var p in filePagckages) {
                    var pVM = new ChatFilePackageViewModel {
                        ChatFilePackageId = Guid.NewGuid().ToString(),
                        FileId = p.FileId,
                        Value = p.Value,
                        MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                        MessageType = ChatMessageType.Send,
                        MessageSendStatus = ChatMessageSendStatus.NotSend
                    };
                    ChatDataRepository.AddOrUpdate(pVM);
                    var msgPackage = new Message();
                    msgPackage.Id = pVM.ChatFilePackageId;
                    msgPackage.Type = message.MessageListType == ChatListType.Private ? MessageType.chat : MessageType.groupchat;
                    msgPackage.To = new Jid(chatList.ChatListId);
                    msgPackage.From = new Jid(ChatClient.CurrentUserContact.ContactId);
                    msgPackage.AddChild(p);
                    await _clientImp.SendAsync(msgPackage, msgPackage.Id).ConfigureAwait(false);
                }
                #endregion

                #region file transfer close
                var fileClose = GetFileCloseElement(fileOpen);
                var fileCloseVM = new ChatFileCloseViewModel {
                    ChatFileCloseId = Guid.NewGuid().ToString(),
                    FileId = fileClose.FileId,
                    MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                    MessageType = ChatMessageType.Send,
                    MessageSendStatus = ChatMessageSendStatus.NotSend
                };
                ChatDataRepository.AddOrUpdate(fileCloseVM);
                var msgFileClose = new Message();
                msgFileClose.Id = fileCloseVM.ChatFileCloseId;
                msgFileClose.Type = message.MessageListType == ChatListType.Private ? MessageType.chat : MessageType.groupchat;
                msgFileClose.To = new Jid(chatList.ChatListId);
                msgFileClose.From = new Jid(ChatClient.CurrentUserContact.ContactId);
                msgFileClose.AddChild(fileClose);
                await _clientImp.SendAsync(msgFileClose, msgFileClose.Id).ConfigureAwait(false);
                #endregion
            } catch (Exception ex) {
                ErrorUtil.ReportException(ex);
            }
        }

        internal async Task SendImageFileAsync(ChatListViewModel chatList, ChatMessageViewModel message)
        {
            await SendFileAsync(chatList, message, "image").ConfigureAwait(false);
        }

        internal async Task ReSendImageFileAsync(ChatMessageViewModel message)
        {
            message.MessageSendStatus = ChatMessageSendStatus.NotSend;
            var chatList = new ChatListViewModel {
                ChatListId = message.FromId,
                ChatListName = message.FromName,
                ListType = message.MessageListType
            };

            await SendFileAsync(chatList, message, "image").ConfigureAwait(false);
        }

        internal async Task SendAudioFileAsync(ChatListViewModel chatList, ChatMessageViewModel message)
        {
            await SendFileAsync(chatList, message, "audio").ConfigureAwait(false);
        }

        internal async Task SendVideoFileAsync(ChatListViewModel chatList, ChatMessageViewModel message)
        {
            await SendFileAsync(chatList, message, "video").ConfigureAwait(false);
        }

        protected override void EndSendMessage(string msgId, bool isSuccess)
        {
            if (string.IsNullOrWhiteSpace(msgId))
                return;

            string fileId = null;
            try {
                #region fileOpen
                var fileOpen = ChatDataRepository.GetFileOpenById(msgId);
                if (fileOpen != null && fileOpen.MessageType == ChatMessageType.Send) {
                    fileId = fileOpen.FileId;
                    fileOpen.MessageSendStatus = isSuccess ? ChatMessageSendStatus.Sent : ChatMessageSendStatus.Fail;
                    ChatDataRepository.AddOrUpdate(fileOpen);
                }
                #endregion

                #region filePackage
                var filePackage = ChatDataRepository.GetFilePackageById(msgId);
                if (filePackage != null && filePackage.MessageType == ChatMessageType.Send) {
                    filePackage.MessageSendStatus = isSuccess ? ChatMessageSendStatus.Sent : ChatMessageSendStatus.Fail;
                    ChatDataRepository.AddOrUpdate(filePackage);

                    fileId = filePackage.FileId;
                }
                #endregion

                #region fileClose
                var fileClose = ChatDataRepository.GetFileCloseById(msgId);
                if (fileClose != null && fileClose.MessageType == ChatMessageType.Send) {
                    fileClose.MessageSendStatus = isSuccess ? ChatMessageSendStatus.Sent : ChatMessageSendStatus.Fail;
                    ChatDataRepository.AddOrUpdate(fileClose);

                    fileId = fileClose.FileId;
                }
                #endregion

                if (string.IsNullOrWhiteSpace(fileId))
                    return;

                var isComp = IsFileSendComplte(fileId);
                if (!isComp)
                    return;

                #region file Message
                var message = ChatDataRepository.GetMessageById(fileId);
                if (message == null)
                    return;

                if (message.MessageContentType == ChatMessageContentType.Text)
                    return;
                if (message.MessageType != ChatMessageType.Send)
                    return;

                message.MessageSendStatus = isSuccess ? ChatMessageSendStatus.Sent : ChatMessageSendStatus.Fail;
                ChatDataRepository.AddOrUpdate(message);
                #endregion
            } catch (Exception ex) {
                ErrorUtil.ReportException(ex);
            }
        }

        private bool IsFileSendComplte(string fileId)
        {
            var isAnyNotSend = ChatDataRepository.ExistsNotSendFileOpenByFileId(fileId);
            if (isAnyNotSend)
                return false;

            isAnyNotSend = ChatDataRepository.ExistsNotSendFilePackageByFileId(fileId);
            if (isAnyNotSend)
                return false;

            isAnyNotSend = ChatDataRepository.ExistsNotSendFileCloseByFileId(fileId);
            if (isAnyNotSend)
                return false;

            return true;
        }

        #endregion
    }
}

