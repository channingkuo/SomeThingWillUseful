#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : IconButtonView Control
****************/
#endregion

using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace RekTec.Corelib.Views
{
    /// <summary>
    /// IconButtonView Control
    /// </summary>
    public class IconButtonView
        :UIButton
    {
        public IconButtonView(CGRect rect, string title, string iconResFileName)
            : base(rect)
        {
            this.SetImage(UIImage.FromFile(iconResFileName), UIControlState.Normal);
            SetTitle(title, UIControlState.Normal);
            SetTitleColor(UIColor.Black, UIControlState.Normal);
            Font = UIFont.SystemFontOfSize(16F);
        }
    }
}