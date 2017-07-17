using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using System.Threading.Tasks;
using RekTec.Chat.DataRepository;
using RekTec.Chat.ViewModels;
using RekTec.Chat.WebApi;
using RekTec.Chat.XmppElement;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Service
{
    internal class ChatFileMessageService:ChatBaseMessageService
    {
        internal ChatFileMessageService(XmppClientConnection conn, ChatContactService contactService)
            : base(conn, contactService)
        {
        }

        #region 接收消息部分的代码

        protected override async void ReceiveMessage(Message msg)
        {
            if (msg == null)
                return;

            if (msg.Type != MessageType.chat && msg.Type != MessageType.groupchat)
                return;

            if (string.IsNullOrWhiteSpace(msg.Body))
                return;

            XmppMessageBody xmppBody = null;
            try {
                xmppBody = XmppMessageBody.ParseJsonString(msg.Body);
            } catch {
            }

            if (xmppBody == null)
                return;

            if (string.IsNullOrWhiteSpace(xmppBody.type) || xmppBody.type == ChatMessageContentType.Text.ToXmppTypeString())
                return;
           
            bool isFromMe = false;
            if (msg.From.Bare == ChatClient.CurrentUserContact.ContactId)
                isFromMe = true;
            if (msg.From.Resource == ChatClient.CurrentUserContact.ContactCode)
                isFromMe = true;

            var message = new ChatMessageViewModel() {
                ChatMessageId = msg.Id,
                MessageContentId = xmppBody.msg,
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
               
            try {
                var bytes = await WebApiFacade.DownloadChatFile(xmppBody.msg);
                if (bytes == null || bytes.Length == 0)
                    return;

                message.MessageContent = Convert.ToBase64String(bytes);

                MessagesDataRepository.AddOrUpdate(message);
            } catch (System.Exception ex) {
                LoggingUtil.Exception(ex);
            }
        }

        #endregion

        #region 发送文件部分的代码

        internal async Task SendFileAsync(ChatMessageViewModel message)
        {
            if (message == null)
                return;
            try {
                if (message.MessageListType == ChatListType.Private)
                    message.FromResource = _clientImp.Resource;
                if (message.MessageListType == ChatListType.Group)
                    message.ChatRoomId = message.ToId;
                await MessagesDataRepository.AddOrUpdateAsync(message)
                    .ConfigureAwait(false);

                var fileContent = FileSystemUtil.GetBase64StringFromCache(message.ChatMessageId);
                byte[] fileData = null;
                try {
                    if (message.MessageContentType == ChatMessageContentType.Image) {
                        var image = ImageUtil.ConvertBase64String2Image(fileContent);
                        fileData = image.AsJPEG().ToArray();
                    }
                } catch {
                }
                if (fileData == null)
                    return;

                message.MessageContentId = await WebApiFacade.UploadChatFile(message.ChatMessageId, fileData)
                    .ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(message.MessageContentId))
                    return;

                await MessagesDataRepository.AddOrUpdateAsync(message)
                    .ConfigureAwait(false);

                var msg = new Message {
                    Id = message.ChatMessageId,
                    From = new Jid(ChatClient.CurrentUserContact.ContactId),
                    To = new Jid(message.ToId),
                    Type = message.MessageListType == ChatListType.Private ? MessageType.chat : MessageType.groupchat,
                    Body = message.GetXmppMessageBody().ToJsonString()
                };

                await _clientImp.SendAsync(msg, msg.Id).ConfigureAwait(false);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        internal async Task ReSendFileAsync(ChatMessageViewModel message)
        {
            message.MessageSendStatus = ChatMessageSendStatus.NotSend;
            await SendFileAsync(message).ConfigureAwait(false);
        }

        #endregion
    }
}

