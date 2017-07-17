using System.Drawing;
using CoreGraphics;
using UIKit;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.Chat
{
    public class ChatMessageTextCell :ChatMessageBaseCell
    {
        UILabel _messageLabel;
        string _messageText;
        bool _isLeft;


        public ChatMessageTextCell(bool isLeft, string cellId)
            : base(isLeft, cellId)
        {
            _isLeft = isLeft;
            _messageLabel = new UILabel(new CGRect(0, 0, 1, 1)) {
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0,
                Font = font,
                BackgroundColor = UIColor.Clear
            };
            ContentView.AddSubview(_messageLabel);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = GetCellBackgroudFrame();

            var size = GetContentSize();
            _messageLabel.Frame = new CGRect(new CGPoint(frame.X + (_isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
        }

        protected override CGSize GetContentSize()
        {
            return GetSizeForText(this, _messageText) + BubblePadding;
        }

        static internal CGSize GetSizeForText(UIView tv, string text)
        {
            if (text == null)
                text = string.Empty;

            //return text.StringSize(font, new CGSize(tv.Bounds.Width * .7f - 10 - 22, 99999));
            return text.StringSize(font, new CGSize(tv.Bounds.Width * .7f - 10 - 22, 99999));
        }

        public void UpdateText(string id, UIImage avatar, string fromName, ChatMessageSendStatus sendStatus, ChatListType listType, string messageText)
        {
            base.Update(id, avatar, fromName, sendStatus, listType);

            _messageText = messageText;
            _messageLabel.Text = _messageText;
            SetNeedsLayout();
        }
    }

}

