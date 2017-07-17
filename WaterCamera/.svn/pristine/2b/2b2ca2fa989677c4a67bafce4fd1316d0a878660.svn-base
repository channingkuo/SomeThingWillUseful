#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 显示头像等个人信息的monotouch dialog的Element
****************/
#endregion

using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;

namespace RekTec.Corelib.Views
{
    /// <summary>
    /// 显示头像等个人信息的monotouch dialog的Element
    /// </summary>
    public class ProfileElement
        : StyledStringElement, IElementSizing
    {
        public ProfileElement(string Caption, string value, UITableViewCellStyle style)
            : base(Caption, value, style)
        {

        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return (nfloat)80;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            if (cell.ImageView != null && cell.ImageView.Image != null)
                cell.ImageView.Image = cell.ImageView.Image.Scale(new CoreGraphics.CGSize(60, 60));
           
            return cell;
        }
    }
}
