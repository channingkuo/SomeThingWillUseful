using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using RekTec.Chat.Views.Common;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Views;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.ChatList
{
    public class ChatListTableViewCell : UITableViewCell
    {
        UILabel _nameLabel, _lastMessageLabel, _lastMessageDateTimeLabel;
        UIImageView _avatarImageView;
        BadgeView _badge;

        public ChatListTableViewCell(NSString cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            _avatarImageView = new UIImageView();
            _badge = new BadgeView(string.Empty);
            _nameLabel = new UILabel() {
                Font = UIFont.BoldSystemFontOfSize(UiStyleSetting.FontTitleSize),
                TextColor = UIColor.Black,
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear
            };
            _lastMessageLabel = new UILabel() {
                Font = UIFont.SystemFontOfSize(UiStyleSetting.FontDetailSize),
                TextColor = UIColor.Gray,
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear
            };
            _lastMessageDateTimeLabel = new UILabel() {
                Font = UIFont.SystemFontOfSize(UiStyleSetting.FontDetailSize),
                TextColor = UIColor.Gray,
                TextAlignment = UITextAlignment.Right,
                BackgroundColor = UIColor.Clear
            };
            ContentView.AddSubviews(new UIView[] {
                _avatarImageView,
                _badge,
                _nameLabel,
                _lastMessageLabel,
                _lastMessageDateTimeLabel
            });

        }

        public void UpdateCell(ChatListViewModel chatList)
        {
            _avatarImageView.Image = chatList.GetAvatarImage();
            _badge.Text = chatList.UnReadCount.ToString();

            _nameLabel.Text = chatList.ChatListName;
            _lastMessageLabel.Text = chatList.LastMessageContent;
            _lastMessageDateTimeLabel.Text = DateTimeUtil.GetDateTimeString(chatList.LastMessageDateTime, false);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _avatarImageView.Frame = new CGRect(UiStyleSetting.PaddingSizeMedium
                , UiStyleSetting.PaddingSizeMedium
                , UiStyleSetting.HeadIconSizeMedium
                , UiStyleSetting.HeadIconSizeMedium);
            if (_badge.Text != "0")
                _badge.Frame = new CGRect(UiStyleSetting.PaddingSizeMedium + UiStyleSetting.HeadIconSizeMedium - 11, 0, 22, 22);
            else
                _badge.Frame = new CGRect(0, 0, 0, 0);

            _nameLabel.Frame = new CGRect(UiStyleSetting.HeadIconSizeMedium + UiStyleSetting.PaddingSizeMedium * 2
                , UiStyleSetting.PaddingSizeMedium
                , ContentView.Bounds.Width - (UiStyleSetting.HeadIconSizeMedium + UiStyleSetting.PaddingSizeMedium * 2) - 100
                , 26);
            _lastMessageLabel.Frame = new CGRect(UiStyleSetting.HeadIconSizeMedium + UiStyleSetting.PaddingSizeMedium * 2
                , UiStyleSetting.PaddingSizeMedium + UiStyleSetting.FontTitleSize + UiStyleSetting.PaddingSizeMedium
                , ContentView.Bounds.Width - (UiStyleSetting.HeadIconSizeMedium + UiStyleSetting.PaddingSizeMedium * 2)
                , 24);
            _lastMessageDateTimeLabel.Frame = new CGRect(ContentView.Bounds.Width - 90, 10, 80, 26);
        }
    }
}

