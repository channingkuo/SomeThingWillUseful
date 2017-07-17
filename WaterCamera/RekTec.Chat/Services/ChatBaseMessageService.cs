using System;
using agsXMPP;
using agsXMPP.protocol.client;
using AudioToolbox;
using RekTec.Chat.DataRepository;
using RekTec.Chat.ViewModels;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Service
{
    internal abstract class ChatBaseMessageService
    {
        protected readonly XmppClientConnection _clientImp;
        protected readonly ChatContactService _contactService;
        private readonly SystemSound _systemSound;

        internal ChatBaseMessageService(XmppClientConnection conn, ChatContactService contactService)
        {
            //AudioSession.Initialize();
            _systemSound = SystemSound.FromFile("beep.mp3");

            _contactService = contactService;
            _clientImp = conn;

            _clientImp.OnMessage += OnReciveMessage;
            _clientImp.OnEndSend += OnEndSendMessage;
        }

        private void OnReciveMessage(object send, Message msg)
        {
            if (msg.Type == MessageType.error) {
                ErrorHandlerUtil.ReportError(msg.Error.ToString());
                return;
            }

            try {
                #region 判断群聊主数据是否存在，不存在，则新增
                if (msg.Type == MessageType.groupchat && !string.IsNullOrWhiteSpace(msg.Subject)) {
                    ChatDataRepository.AddOrUpdate(new ChatRoomViewModel {
                        ChatRoomId = msg.From.Bare,
                        ChatRoomName = msg.Subject
                    });
                    return;
                }
                #endregion

                #region 判断通讯录主数据是否存在，不存在，则新增
                if (msg.Type == MessageType.chat && !string.IsNullOrWhiteSpace(msg.From.Bare)) {
                    var contact = ContactsDataRepository.GetContactById(msg.From.Bare);
                    if (contact == null) {
                        contact = new ContactViewModel {
                            ContactId = msg.From.Bare,
                            ContactName = msg.From.Bare.Substring(0, msg.From.Bare.IndexOf("@"))
                        };
                        ContactsDataRepository.AddOrUpdate(contact);
                    }
                }
                #endregion

                if ((msg.Type == MessageType.chat || msg.Type == MessageType.groupchat)) {
                    string id = null;
                    if (!string.IsNullOrEmpty(msg.Id))
                        id = msg.Id;
                    else if (msg.XDelay != null)
                        id = msg.From.ToString() + msg.XDelay.Stamp.ToString("yyyyMMddHHmmssfff");
                    else
                        id = Guid.NewGuid().ToString();

                    msg.Id = id;

                    var existsMessage = MessagesDataRepository.GetMessageById(msg.Id);
                    if (existsMessage != null)
                        return;

                    if (ChatAppSetting.IsNotifiedBeep) {
                        SystemSound.Vibrate.PlaySystemSound();
                    }

                    if (ChatAppSetting.IsNotifiedVoice) {
                        _systemSound.PlaySystemSound();
                    }
                    ReceiveMessage(msg);
                }

            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        protected virtual void ReceiveMessage(Message msg)
        {

        }

        protected virtual void EndSendMessage(string msgId, bool isSuccess)
        {
            try {
                if (string.IsNullOrWhiteSpace(msgId))
                    return;

                var msg = MessagesDataRepository.GetMessageById(msgId);

                if (msg == null)
                    return;
                    
                if (msg.MessageType != ChatMessageType.Send)
                    return;

                msg.MessageSendStatus = isSuccess ? ChatMessageSendStatus.Sent : ChatMessageSendStatus.Fail;

                MessagesDataRepository.AddOrUpdate(msg);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        private void OnEndSendMessage(string msgId, bool isSuccess)
        {
            try {
                if (string.IsNullOrWhiteSpace(msgId))
                    return;

                EndSendMessage(msgId, isSuccess);

            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }
    }
}

