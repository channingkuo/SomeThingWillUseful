using System.Drawing;
using CoreGraphics;
using UIKit;
using RekTec.Chat.Views.Common;
using RekTec.Corelib.Utils;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.Chat
{
    public class ChatMessageImageCell : ChatMessageBaseCell
    {
        UIButton _messageImageView;
        bool _isLeft;

        public ChatMessageImageCell(bool isLeft, string cellId)
            : base(isLeft, cellId)
        {
            _isLeft = isLeft;
            var rect = new CGRect(0, 0, 1, 1);
            _messageImageView = new UIButton(rect) { BackgroundColor = UIColor.Clear };
            _messageImageView.AddTarget((sender, e) => {
                var imagePrev = new FullScreenImageView();
                imagePrev.SetImage(ImageUtil.GetImageFromCache(_messageId));
                imagePrev.Show();
            }, UIControlEvent.TouchUpInside);
            ContentView.AddSubview(_messageImageView);

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = GetCellBackgroudFrame();
            var size = GetContentSize();  
         
            _messageImageView.Frame = new CGRect(new CGPoint(frame.X + (_isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
        }

        protected override CGSize GetContentSize()
        {
            return _messageImageView.BackgroundImageForState(UIControlState.Normal).Size;
        }

        public void UpdateImage(string id, UIImage avatar, string fromName, ChatMessageSendStatus sendStatus, ChatListType listType, UIImage messageImage)
        {
            Update(id, avatar, fromName, sendStatus, listType);
           
            _messageImageView.SetBackgroundImage(messageImage, UIControlState.Normal);
            SetNeedsLayout();
        }
    }
}

