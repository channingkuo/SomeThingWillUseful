using System;
using System.Drawing;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using RekTec.Chat.ViewModels;

namespace RekTec.Chat.Views.Common
{
    public class RoomMemberElement:StyledStringElement,IElementSizing
    {
        public ChatRoomMemberViewModel RoomMember { get; set; }

        public RoomMemberElement(string caption)
            : base(caption)
        {

        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 50;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            if (cell.ImageView != null && cell.ImageView.Image != null)
                cell.ImageView.Image = cell.ImageView.Image.Scale(new SizeF(40, 40));

            return cell;
        }
    }
}

