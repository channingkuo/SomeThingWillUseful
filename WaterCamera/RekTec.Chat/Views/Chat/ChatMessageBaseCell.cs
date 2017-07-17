using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using RekTec.Chat.Service;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.Chat
{
    public abstract class ChatMessageBaseCell : UITableViewCell
    {
        public static NSString KeyLeft = new NSString("BubbleElementLeft");
        public static NSString KeyRight = new NSString("BubbleElementRight");
        public static UIImage bleft, bright, left, right;
        public static UIFont font = UIFont.SystemFontOfSize(14);

        UIImageView _cellBackgroudImageView, _avatarImageView;
        UIActivityIndicatorView _sendStatusView;
        ChatMessageSendStatus _messageSendStatus;
        UILabel _contactNameLable;
        UIButton _resendButton;
        bool _isLeft;
        protected string _messageId;

        static ChatMessageBaseCell()
        {
            bright = UIImage.FromFile("chat_sender_bg.png");
            bleft = UIImage.FromFile("chat_receiver_bg.png");
            left = bleft.CreateResizableImage(new UIEdgeInsets(26, 16, 11, 11), UIImageResizingMode.Stretch);
            right = bright.CreateResizableImage(new UIEdgeInsets(26, 11, 11, 16), UIImageResizingMode.Stretch);
        }

        public ChatMessageBaseCell(bool isLeft, string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            _isLeft = isLeft;
            SelectionStyle = UITableViewCellSelectionStyle.None;

            _cellBackgroudImageView = new UIImageView(_isLeft ? left : right) { UserInteractionEnabled = true };
            ContentView.AddSubview(_cellBackgroudImageView);

            _avatarImageView = new UIImageView();
            ContentView.Add(_avatarImageView);

            _contactNameLable = new UILabel();
            _contactNameLable.BackgroundColor = UIColor.Clear;
            _contactNameLable.Font = UIFont.SystemFontOfSize(12F);
            _contactNameLable.TextAlignment = UITextAlignment.Center;
            ContentView.Add(_contactNameLable);

            if (!isLeft) {
                _sendStatusView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
                ContentView.Add(_sendStatusView);

                _resendButton = new UIButton();
                _resendButton.SetBackgroundImage(UIImage.FromFile("messageSendFail.png"), UIControlState.Normal);
                ContentView.Add(_resendButton);
                _resendButton.AddTarget((sender, e) => {
                    ChatClient.ReSendMessageAsync(_messageId).ContinueWith((t) => {
                    });
                }, UIControlEvent.TouchUpInside);
            }

            this.BackgroundColor = UIColor.Clear;
            ContentView.BackgroundColor = UIColor.Clear;
            ContentView.Superview.BackgroundColor = UIColor.Clear;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = ContentView.Frame;
            _avatarImageView.Frame = new CGRect(_isLeft ? 10 : frame.Width - 50, 10, 40, 40);
            _contactNameLable.Frame = new CGRect(_isLeft ? 10 : frame.Width - 50, 50, 40, 20);

            var size = GetContentSize();
            _cellBackgroudImageView.Frame = new CGRect(new CGPoint(_isLeft ? 60 : frame.Width - size.Width - 60, frame.Y + 10), size);
            frame = _cellBackgroudImageView.Frame;

            if (!_isLeft) {
                 
                if (_messageSendStatus == ChatMessageSendStatus.NotSend) {
                    _sendStatusView.Frame = new CGRect(new CGPoint(ContentView.Frame.Width - frame.Width - 100, frame.Y), new CGSize{
                        Width = 30,
                        Height = 30
                    });
                    _sendStatusView.StartAnimating();
                } else
                    _sendStatusView.StopAnimating();

                if (_messageSendStatus == ChatMessageSendStatus.Fail) {
                    _resendButton.Frame = new CGRect(new CGPoint(ContentView.Frame.Width - frame.Width - 100, frame.Y), new CGSize{
                        Width = 30,
                        Height = 30
                    });
                } else {
                    _resendButton.Frame = new CGRect(new CGPoint(ContentView.Frame.Width - frame.Width - 100, frame.Y), new CGSize{
                        Width = 0,
                        Height = 0
                    });
                }
            }
        }

        protected CGRect GetCellBackgroudFrame()
        {
            return _cellBackgroudImageView.Frame;
        }

        protected virtual CGSize GetContentSize()
        {
            return new CGSize(0F, 0F);
        }

        static internal SizeF BubblePadding = new SizeF(22, 16);

        protected void Update(string id, UIImage avatar, string fromName, ChatMessageSendStatus sendStatus, ChatListType listType)
        {
            _messageId = id;
            _messageSendStatus = sendStatus;
            _avatarImageView.Image = avatar;
            _contactNameLable.Text = listType == ChatListType.Group ? fromName : string.Empty;
        }
    }
}

