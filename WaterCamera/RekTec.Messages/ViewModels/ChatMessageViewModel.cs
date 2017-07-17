#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 : joesong
日期 : 2015-05-04
说明 : 存储在本地数据库的聊天消息的Model
****************/
#endregion

using System;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Utils;
using SQLite;
using UIKit;

namespace RekTec.Messages.ViewModels
{
    /// <summary>
    /// 存储在本地数据库的聊天消息的Model
    /// </summary>
    public class ChatMessageViewModel
    {
        [PrimaryKey]
        public string ChatMessageId { get; set; }

        public string ChatRoomId { get; set; }

        private string _fromId;

        public string FromId {
            get { return _fromId; }
            set { _fromId = value; }
        }

        public string FromResource { get; set; }

        [Ignore]
        public string FromCode {
            get {
                if (string.IsNullOrWhiteSpace(FromId) || FromId.Length <= 1)
                    return string.Empty;

                return FromId.Substring(0, FromId.IndexOf("@", StringComparison.Ordinal));
            }
        }

        private string _fromName;

        public string FromName {
            get { return _fromName; }
            set { _fromName = value; }
        }

        private string _toId;

        public string ToId {
            get { return _toId; }
            set { _toId = value; }
        }

        [Ignore]
        public string ToCode {
            get { return ToId.Substring(0, ToId.IndexOf("@", StringComparison.Ordinal)); }
        }

        private string _toName;

        public string ToName {
            get { return _toName; }
            set { _toName = value; }
        }

        private DateTime _sendDateTime;

        public DateTime SendDateTime {
            get { return _sendDateTime; }
            set { _sendDateTime = value; }
        }

        public DateTime CreatedOn { get; set; }

        private string _messageContent;

        public string MessageContent {
            get { return _messageContent; }
            set { _messageContent = value; }
        }

        private ChatMessageType _messageType = ChatMessageType.Send;

        public ChatMessageType MessageType {
            get { return _messageType; }
            set { _messageType = value; }
        }

        private ChatMessageContentType _messageContentType = ChatMessageContentType.Text;

        public ChatMessageContentType MessageContentType {
            get { return _messageContentType; }
            set { _messageContentType = value; }
        }

        private ChatMessageSendStatus _messageSendStatus = ChatMessageSendStatus.NotSend;

        public ChatMessageSendStatus MessageSendStatus {
            get { return _messageSendStatus; }
            set { _messageSendStatus = value; }
        }

        private ChatMessageReceiveStatus _messageReceiveStatus = ChatMessageReceiveStatus.Received;

        public ChatMessageReceiveStatus MessageReceiveStatus {
            get { return _messageReceiveStatus; }
            set { _messageReceiveStatus = value; }
        }

        private ChatListType _messageListType = ChatListType.Private;

        public ChatListType MessageListType {
            get { return _messageListType; }
            set { _messageListType = value; }
        }

        public UIImage GetMessageFromAvatar()
        {
            UIImage photo = null;
            var contact = ContactsDataRepository.GetContactById(FromId);
            if (contact != null)
                photo = contact.GetAvatarImage();
            else {
                contact = ContactsDataRepository.GetContactByCode(FromResource);
                if (contact != null)
                    photo = contact.GetAvatarImage();
            }

            return photo ?? ContactViewModel.DefaultAvatar;
        }

        public string MessageContentId { get; set; }

        public MessageBody GetXmppMessageBody()
        {
            return new MessageBody {
                type = this.MessageContentType.ToXmppTypeString(),
                msg = this.MessageContentType == ChatMessageContentType.Text ? this.MessageContent : this.MessageContentId,
                ext = this.MessageContentType.ToXmppExtString()
            };
        }

        public UIImage GetMessageImage()
        {
            return ImageUtil.GetImageFromCache(ChatMessageId);
        }

        public override string ToString()
        {
            return string.Format("[ChatMessageViewModel: ChatMessageId={0}, FromId={1}, FromCode={2}, FromName={3}, ToId={4}, ToCode={5}, ToName={6}, SendDateTime={7}, MessageContent={8}, MessageType={9}, MessageContentType={10}, MessageSendStatus={11}]", ChatMessageId, FromId, FromCode, FromName, ToId, ToCode, ToName, SendDateTime, MessageContent, MessageType, MessageContentType, MessageSendStatus);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            var o = obj as ChatMessageViewModel;
            if (o == null)
                return false;

            if (string.IsNullOrWhiteSpace(ChatMessageId))
                return false;

            if (string.IsNullOrWhiteSpace(o.ChatMessageId))
                return false;

            return this.ChatMessageId.Equals(o.ChatMessageId, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return ChatMessageId.GetHashCode();
        }
    }

    public enum ChatMessageType
    {
        Send,
        Receive
    }

    public enum ChatMessageContentType
    {
        Text,
        Image,
        Audio,
        Video,
        Location,
        Command,
        File
    }

    public static class ChatMessageContentTypeExtensions
    {
        public static string ToXmppTypeString(this ChatMessageContentType t)
        {
            switch (t) {
                case ChatMessageContentType.Text:
                    return "txt";
                case ChatMessageContentType.Image:
                    return "img";
                case ChatMessageContentType.Audio:
                    return "audio";
                case ChatMessageContentType.Video:
                    return "video";
                case ChatMessageContentType.Location:
                    return "loc";
                case ChatMessageContentType.Command:
                    return "cmd";
                case ChatMessageContentType.File:
                    return "file";
                default:
                    return string.Empty;
            }
        }

        public static string ToXmppExtString(this ChatMessageContentType t)
        {
            switch (t) {
                case ChatMessageContentType.Text:
                    return "txt";
                case ChatMessageContentType.Image:
                    return "jpg";
                case ChatMessageContentType.Audio:
                    return "mp3";
                case ChatMessageContentType.Video:
                    return "avi";
                case ChatMessageContentType.Location:
                    return "loc";
                case ChatMessageContentType.Command:
                    return "cmd";
                case ChatMessageContentType.File:
                    return "file";
                default:
                    return string.Empty;
            }
        }
    }

    public enum ChatMessageSendStatus
    {
        NotSend,
        Sent,
        Fail
    }

    public enum ChatMessageReceiveStatus
    {
        Received,
        Readed
    }
}

