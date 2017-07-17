using UIKit;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.Views.Common
{
    public class ChatBaseViewController : UIViewController
    {
        public ChatBaseViewController()
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
            View.BackgroundColor = UiStyleSetting.NavigationBarColor;
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

