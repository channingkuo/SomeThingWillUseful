using Foundation;
using UIKit;
using MonoTouch.Dialog;

namespace RekTec.Chat.Views.Common
{
    public class AvatarElement : BadgeElement, IElementSizing
    {
        public AvatarElement(UIImage image, string text)
            : base(image, text)
        {

        }

        public new float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 80;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            if (cell.ImageView != null && cell.ImageView.Image != null)
                cell.ImageView.Image = cell.ImageView.Image.Scale(new System.Drawing.SizeF(60, 60));

            return cell;
        }
    }
}

