using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using RekTec.Corelib.Configuration;

namespace RekTec.Chat.Views.Contact
{
    public class ContactChooseTableViewCell : UITableViewCell
    {
        private UILabel _nameLabel;
        UIImageView _photoImageView;
        UIImageView _checkBoxImageView;

        static UIImage _checkedImage, _uncheckedImage;

        static ContactChooseTableViewCell()
        {
            _checkedImage = UIImage.FromFile("checkbox_checked.png");
            _uncheckedImage = UIImage.FromFile("checkbox_unchecked.png");
        }

        public ContactChooseTableViewCell(NSString cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            _checkBoxImageView = new UIImageView();

            _photoImageView = new UIImageView();
            _nameLabel = new UILabel()
            {
                Font = UIFont.BoldSystemFontOfSize(UiStyleSetting.FontTitleSize),
                TextColor = UIColor.Black,
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear
            };

            ContentView.AddSubviews(new UIView[] { _checkBoxImageView, _photoImageView, _nameLabel });

        }

        public void UpdateCell(UIImage photo, string name, bool isChecked)
        {
            _checkBoxImageView.Image = isChecked ? _checkedImage : _uncheckedImage;
            _photoImageView.Image = photo;
            _nameLabel.Text = name;

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _checkBoxImageView.Frame = new CGRect(10, 9, 32, 32);
            _photoImageView.Frame = new CGRect(53, 5, 40, 40);
            _nameLabel.Frame = new CGRect(118, 5, ContentView.Bounds.Width - 42 - 55 - 10, 26);
        }
    }
}

