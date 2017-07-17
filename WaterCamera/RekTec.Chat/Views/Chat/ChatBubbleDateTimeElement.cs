using System;
using MonoTouch.Dialog;
using Foundation;
using UIKit;

namespace RekTec.Chat.Views.Chat
{
    public class ChatBubbleDateTimeElement: StyledStringElement, IElementSizing
    {
        public ChatBubbleDateTimeElement(string caption)
            : base(caption)
        {
            Alignment = UITextAlignment.Center;
            Font = UIFont.SystemFontOfSize(10F);
            BackgroundColor = UIColor.Clear;
        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 20F;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            cell.BackgroundColor = UIColor.Clear;
            return cell;
        }
    }
}

