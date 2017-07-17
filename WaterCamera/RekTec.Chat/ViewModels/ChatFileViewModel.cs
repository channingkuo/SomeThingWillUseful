using System;
using SQLite;
using System.Xml;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.ViewModels
{
    public class ChatFileOpenViewModel
    {
        [PrimaryKey]
        public string ChatFileOpenId { get; set; }

        public string FileId { get; set; }

        public string FileType { get; set; }

        public int PackageCount { get; set; }

        private ChatMessageType _messageType = ChatMessageType.Send;

        public ChatMessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        private ChatMessageSendStatus _messageSendStatus = ChatMessageSendStatus.NotSend;

        public ChatMessageSendStatus MessageSendStatus
        {
            get { return _messageSendStatus; }
            set { _messageSendStatus = value; }
        }

        private ChatMessageReceiveStatus _messageReceiveStatus = ChatMessageReceiveStatus.Received;

        public ChatMessageReceiveStatus MessageReceiveStatus
        {
            get { return _messageReceiveStatus; }
            set { _messageReceiveStatus = value; }
        }
    }


    public class ChatFileCloseViewModel
    {
        [PrimaryKey]
        public string ChatFileCloseId { get; set; }

        public string FileId { get; set; }

        private ChatMessageType _messageType = ChatMessageType.Send;

        public ChatMessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        private ChatMessageSendStatus _messageSendStatus = ChatMessageSendStatus.NotSend;

        public ChatMessageSendStatus MessageSendStatus
        {
            get { return _messageSendStatus; }
            set { _messageSendStatus = value; }
        }

        private ChatMessageReceiveStatus _messageReceiveStatus = ChatMessageReceiveStatus.Received;

        public ChatMessageReceiveStatus MessageReceiveStatus
        {
            get { return _messageReceiveStatus; }
            set { _messageReceiveStatus = value; }
        }
    }

    public class ChatFilePackageViewModel
    {
        [PrimaryKey]
        public string ChatFilePackageId { get; set; }

        public string FileId { get; set; }

        public int Seq { get; set; }

        public string Value { get; set; }

        private ChatMessageType _messageType = ChatMessageType.Send;

        public ChatMessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        private ChatMessageSendStatus _messageSendStatus = ChatMessageSendStatus.NotSend;

        public ChatMessageSendStatus MessageSendStatus
        {
            get { return _messageSendStatus; }
            set { _messageSendStatus = value; }
        }

        private ChatMessageReceiveStatus _messageReceiveStatus = ChatMessageReceiveStatus.Received;

        public ChatMessageReceiveStatus MessageReceiveStatus
        {
            get { return _messageReceiveStatus; }
            set { _messageReceiveStatus = value; }
        }
    }
}

