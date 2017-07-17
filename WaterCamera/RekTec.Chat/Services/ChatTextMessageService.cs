using System;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using System.Threading.Tasks;
using RekTec.Chat.DataRepository;
using RekTec.Chat.ViewModels;
using RekTec.Chat.XmppElement;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Service
{
    internal class ChatTextMessageService:ChatBaseMessageService
    {
        internal ChatTextMessageService(XmppClientConnection conn, ChatContactService contactService)
            : base(conn, contactService)
        {
        }

        #region 接收消息

        protected override void ReceiveMessage(Message msg)
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
            if (xmppBody != null && xmppBody.type != ChatMessageContentType.Text.ToXmppTypeString())
                return;

            bool isFromMe = false;
            if (msg.From.Bare == ChatClient.CurrentUserContact.ContactId)
                isFromMe = true;
            if (msg.From.Resource == ChatClient.CurrentUserContact.ContactCode)
                isFromMe = true;
                
            var toId = msg.To == null ? ChatClient.CurrentUserContact.ContactId : msg.To.Bare;
            MessagesDataRepository.AddOrUpdate(new ChatMessageViewModel {
                ChatMessageId = msg.Id,
                ChatRoomId = msg.Type == MessageType.groupchat ? msg.From.Bare : string.Empty,
                FromId = msg.From.Bare,
                FromResource = msg.From.Resource,
                ToId = toId,
                MessageContent = xmppBody == null ? msg.Body : xmppBody.msg,
                MessageType = isFromMe ? ChatMessageType.Send : ChatMessageType.Receive,
                MessageContentType = ChatMessageContentType.Text,
                MessageSendStatus = ChatMessageSendStatus.Sent,
                MessageReceiveStatus = ChatMessageReceiveStatus.Received,
                SendDateTime = msg.XDelay != null ? msg.XDelay.Stamp : DateTime.Now,
                MessageListType = msg.Type == MessageType.chat ? ChatListType.Private : ChatListType.Group
            });
        }

        #endregion

        #region 发送消息

        internal async Task SendMessageAsync(ChatMessageViewModel message)
        {
            try {
                if (message == null)
                    return;
 
                if (message.MessageListType == ChatListType.Private)
                    message.FromResource = _clientImp.Resource;
                if (message.MessageListType == ChatListType.Group)
                    message.ChatRoomId = message.ToId;

                await MessagesDataRepository.AddOrUpdateAsync(message)
                    .ConfigureAwait(false);
       
                var msg = new Message {
                    Id = message.ChatMessageId,
                    From = new Jid(ChatClient.CurrentUserContact.ContactId),
                    To = new Jid(message.ToId),
                    Type = message.MessageListType == ChatListType.Private ? MessageType.chat : MessageType.groupchat,
                    Body = message.GetXmppMessageBody().ToJsonString()
                };

                await _clientImp.SendAsync(msg, msg.Id)
                    .ConfigureAwait(false);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        internal async Task ReSendMessageAsync(ChatMessageViewModel message)
        {
            message.MessageSendStatus = ChatMessageSendStatus.NotSend;
            await SendMessageAsync(message)
                .ConfigureAwait(false);
        }

        #endregion
    }
}

