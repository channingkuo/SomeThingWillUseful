using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;

namespace RekTec.Chat.Views.Common
{
    public class ProfileElement
        : StyledStringElement, IElementSizing
    {
        public ProfileElement(string Caption, string value, UITableViewCellStyle style)
            : base(Caption, value, style)
        {

        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
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
