using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using RekTec.Chat.Common;

namespace RekTec.Chat.UI
{
    public class ChatTableViewCell : UITableViewCell
    {
        public static NSString KeyLeft = new NSString("BubbleElementLeft");
        public static NSString KeyRight = new NSString("BubbleElementRight");
        public static UIImage bleft, bright, left, right;
        public static UIFont font = UIFont.SystemFontOfSize(14);

        UIImageView _cellBackgroudImageView, _avatarImageView;
        UIActivityIndicatorView _sendStatusView;
        UIButton _messageImageView;
        UILabel _messageLabel;

        string _messageText;
        string _messageId;
        UIImage _messageImageThumb;
        ChatMessageContentType _messageContentType;
        ChatMessageSendStatus _messageSendStatus;

        bool _isLeft;

        static ChatTableViewCell()
        {
            bright = UIImage.FromFile("chat_sender_bg.png");
            bleft = UIImage.FromFile("chat_receiver_bg.png");
            left = bleft.CreateResizableImage(new UIEdgeInsets(26, 16, 11, 11), UIImageResizingMode.Stretch);
            right = bright.CreateResizableImage(new UIEdgeInsets(26, 11, 11, 16), UIImageResizingMode.Stretch);
        }

        public ChatTableViewCell(bool isLeft)
            : base(UITableViewCellStyle.Default, isLeft ? KeyLeft : KeyRight)
        {
            _isLeft = isLeft;
            var rect = new RectangleF(0, 0, 1, 1);
            SelectionStyle = UITableViewCellSelectionStyle.None;

            _cellBackgroudImageView = new UIImageView(_isLeft ? left : right) { UserInteractionEnabled = true };
            ContentView.AddSubview(_cellBackgroudImageView);
            _messageLabel = new UILabel(rect) {
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0,
                Font = font,
                BackgroundColor = UIColor.Clear
            };
            ContentView.AddSubview(_messageLabel);

            _messageImageView = new UIButton(rect) { BackgroundColor = UIColor.Clear };
            _messageImageView.AddTarget((sender, e) => {
                var imagePrev = new FullScreenImageView();
                imagePrev.SetImage(ImageUtil.GetImageFromCache(_messageId));
                imagePrev.Show();
            }, UIControlEvent.TouchUpInside);
            ContentView.AddSubview(_messageImageView);

            _avatarImageView = new UIImageView();
            ContentView.Add(_avatarImageView);

           
            if (!isLeft) {
                _sendStatusView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
                ContentView.Add(_sendStatusView);
            }

            ContentView.BackgroundColor = UIColor.Clear;
            ContentView.Superview.BackgroundColor = UIColor.Clear;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = ContentView.Frame;
            _avatarImageView.Frame = new RectangleF(_isLeft ? 10 : frame.Width - 50, 10, 40, 40);

            var size = new SizeF();

            if (_messageContentType == ChatMessageContentType.Text) {
                size = GetSizeForText(this, _messageText) + BubblePadding;
                _messageImageView.RemoveFromSuperview();
            } else if (_messageContentType == ChatMessageContentType.Image) {
                size = _messageImageView.BackgroundImageForState(UIControlState.Normal).Size;
                _messageLabel.RemoveFromSuperview();
            }

            _cellBackgroudImageView.Frame = new RectangleF(new PointF(_isLeft ? 60 : frame.Width - size.Width - 60, frame.Y + 10), size);
            frame = _cellBackgroudImageView.Frame;

            if (!_isLeft) {
                _sendStatusView.Frame = new RectangleF(new PointF(ContentView.Frame.Width - frame.Width - 100, frame.Y), new SizeF {
                    Width = 30,
                    Height = 30
                }); 
                if (_messageSendStatus == ChatMessageSendStatus.NotSend)
                    _sendStatusView.StartAnimating();
                else
                    _sendStatusView.StopAnimating();
            }
            if (_messageContentType == ChatMessageContentType.Text) {
                _messageLabel.Frame = new RectangleF(new PointF(frame.X + (_isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
            } else if (_messageContentType == ChatMessageContentType.Image) {
                _messageImageView.Frame = new RectangleF(new PointF(frame.X + (_isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
            }

        }

        static internal SizeF BubblePadding = new SizeF(22, 16);

        static internal SizeF GetSizeForText(UIView tv, string text)
        {
            if (text == null)
                text = string.Empty;

            return tv.StringSize(text, font, new SizeF(tv.Bounds.Width * .7f - 10 - 22, 99999));
        }

        public void Update(string id, UIImage avatar, ChatMessageContentType contentType, ChatMessageSendStatus sendStatus, string messageText, UIImage messageImage)
        {
            _messageId = id;
            _messageText = messageText;
            _messageImageThumb = messageImage;
            _messageContentType = contentType;
            _messageSendStatus = sendStatus;

            _avatarImageView.Image = avatar;

            if (_messageContentType == ChatMessageContentType.Text)
                _messageLabel.Text = _messageText;
            else if (_messageContentType == ChatMessageContentType.Image)
                _messageImageView.SetBackgroundImage(messageImage, UIControlState.Normal);

            SetNeedsLayout();
        }
    }

    public class ChatBubbleElement : Element, IElementSizing
    {
        bool isLeft;
        public ChatMessageViewModel ChatMessage;
        private UIImage _messageImage;
        private string _messageText;

        public ChatBubbleElement(ChatMessageViewModel msg)
            : base(msg.MessageContent)
        {
            ChatMessage = msg;

            this.isLeft = ChatMessage.MessageType == ChatMessageType.Receive;

            if (ChatMessage.MessageContentType == ChatMessageContentType.Text)
                _messageText = EmojiUtil.ReplaceCharWithUnicode(ChatMessage.MessageContent);

            if (ChatMessage.MessageContentType == ChatMessageContentType.Image) {
                var image = ImageUtil.ConvertBase64String2Image(ChatMessage.GetMessageFileContent());
                if (image == null)
                    image = UIImage.FromFile("error.png");

                var width = image.Size.Width / 5;
                if (width > UIUtil.ScreenWidth - 180)
                    width = UIUtil.ScreenWidth - 180;

                var scale = image.Size.Width / width;
                _messageImage = image.Scale(new SizeF(image.Size.Width / scale, image.Size.Height / scale));
            }
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(ChatMessage.ChatMessageId) as ChatTableViewCell;
            if (cell == null)
                cell = new ChatTableViewCell(isLeft);

            cell.Update(ChatMessage.ChatMessageId, ChatMessage.GetMessageFromAvatar(), ChatMessage.MessageContentType, ChatMessage.MessageSendStatus, _messageText, _messageImage);
            return cell;
        }

        public float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            var h = 0F;
            if (ChatMessage.MessageContentType == ChatMessageContentType.Text)
                h = ChatTableViewCell.GetSizeForText(tableView, _messageText).Height + ChatTableViewCell.BubblePadding.Height;
            else if (ChatMessage.MessageContentType == ChatMessageContentType.Image)
                h = _messageImage.Size.Height;

            h += 20;

            if (h < 60)
                h = 60;

            return h;
        }
    }
}
