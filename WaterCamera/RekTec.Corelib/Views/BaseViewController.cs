#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 所有ViewController的基类
****************/
#endregion

using UIKit;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;

namespace RekTec.Corelib.Views
{
    /// <summary>
    /// 所有ViewController的基类
    /// </summary>
    public class BaseViewController : UIViewController
    {
        public BaseViewController()
        {
        }

        private bool HandleError(string msg)
        {
            AlertUtil.Error(msg);
            return true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UiStyleSetting.ViewControllerColor;
            NavigationItem.BackBarButtonItem = new UIBarButtonItem(string.Empty, UIBarButtonItemStyle.Plain, null);
            if (UiStyleSetting.IsIos7OrLater)
                SetNeedsStatusBarAppearanceUpdate();
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UiStyleSetting.BarStyle;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ErrorHandlerUtil.Subscribe(HandleError);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ErrorHandlerUtil.UnSubscribe(HandleError);
        }
    }
}

