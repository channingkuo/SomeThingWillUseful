using System;
using System.Drawing;
using CoreGraphics;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using RekTec.Chat.DataRepository;
using RekTec.Corelib.Configuration;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.Chat
{
    public class ChatBubbleMessageElement : Element, IElementSizing
    {
        bool isLeft;
        public ChatMessageViewModel ChatMessage;
        private UIImage _messageImage;
        private string _messageText;

        public ChatBubbleMessageElement(ChatMessageViewModel msg)
            : base(msg.MessageContent)
        {
            ChatMessage = msg;

            this.isLeft = ChatMessage.MessageType == ChatMessageType.Receive;

            if (ChatMessage.MessageContentType == ChatMessageContentType.Text)
                _messageText = EmojiUtil.ReplaceCharWithUnicode(ChatMessage.MessageContent);

            if (ChatMessage.MessageContentType == ChatMessageContentType.Image) {
                var image = ChatMessage.GetMessageImage();
                if (image == null)
                    image = UIImage.FromFile("error.png");

                var width = image.Size.Width / 5;
                if (width > UiStyleSetting.ScreenWidth - 180)
                    width = UiStyleSetting.ScreenWidth - 180;

                var scale = image.Size.Width / width;
                _messageImage = image.Scale(new CGSize(image.Size.Width / scale, image.Size.Height / scale));
            }
        }

        public override UITableViewCell GetCell(UITableView tv)
        {

            if (ChatMessage.MessageContentType == ChatMessageContentType.Text) {
                var cell = tv.DequeueReusableCell(ChatMessage.ChatMessageId) as ChatMessageTextCell;
                if (cell == null) {                   
                    var fromName = GetMessageFromContactName(ChatMessage);
                    cell = new ChatMessageTextCell(isLeft, ChatMessage.ChatMessageId);
                    var avatar = GetMessageFromAvatar(ChatMessage);
                    cell.UpdateText(ChatMessage.ChatMessageId, avatar, fromName, ChatMessage.MessageSendStatus, ChatMessage.MessageListType, _messageText);
                }
                return cell;
            } else {
                var cell = tv.DequeueReusableCell(ChatMessage.ChatMessageId) as ChatMessageImageCell;
                if (cell == null) {
                    var fromName = GetMessageFromContactName(ChatMessage);
                    cell = new ChatMessageImageCell(isLeft, ChatMessage.ChatMessageId); 
                    var avatar = GetMessageFromAvatar(ChatMessage);
                    cell.UpdateImage(ChatMessage.ChatMessageId, avatar, fromName, ChatMessage.MessageSendStatus, ChatMessage.MessageListType, _messageImage);
                }
                return cell;
            }
        }

        private UIImage GetMessageFromAvatar(ChatMessageViewModel chatMessage)
        {
            if (ChatMessage.MessageListType == ChatListType.Private)
                return ChatSessionDataCache.GetAvatarById(chatMessage.FromId);
            else {
                return ChatSessionDataCache.GetAvatarById(ChatMessage.FromResource + "@" + ChatAppSetting.HostName);
            }
        }

        private string GetMessageFromContactName(ChatMessageViewModel chatMessage)
        {
            string fromName = string.Empty;
            if (ChatMessage.MessageListType == ChatListType.Private)
                fromName = ChatMessage.FromName;
            else {
                var contact = ChatSessionDataCache.GetContactById(ChatMessage.FromResource + "@" + ChatAppSetting.HostName);
                if (contact == null) {
                    fromName = ChatMessage.FromResource;
                } else {
                    fromName = contact.ContactName.Split(new char[]{ '-' })[0];
                }
            }
            return fromName;
        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            nfloat h = 0F;
            if (ChatMessage.MessageContentType == ChatMessageContentType.Text)
                h = ChatMessageTextCell.GetSizeForText(tableView, _messageText).Height + ChatMessageTextCell.BubblePadding.Height;
            else if (ChatMessage.MessageContentType == ChatMessageContentType.Image)
                h = _messageImage.Size.Height;

            h += 20;

            if (h < 70)
                h = 70;

            return h;
        }
    }
}
