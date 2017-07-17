using Foundation;
using RekTec.Contacts.Services;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.Messages.PushNotification.Services;
using RekTec.MyProfile.Services;
using RekTec.Version.Services;
using RekTec.Application.Views;
using UIKit;
using RekTec.Chat.Service;
using tingyunApp.iOS;

namespace RekTec.Application.App
{
	public class AppDelegate : UIApplicationDelegate
	{
		private UIWindow _window;
		private PushService _pushService;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//打印发布版本和build版本
			System.Console.WriteLine (VersionService.AppVersion);
			int NBSOption_Net = 1 << 0;
			int NBSOption_UI = 1 << 1;
			int NBSOption_Crash = 1 << 2;
			int NBSOption_Socket = 1 << 4;
			int NBSOption_StrideApp = 1 << 5;
			int NBSOption_ANR = 1 << 6;
			int NBSOption_Behaviour = 1 << 7;
			int NBSOption_CDNHeader = 1 << 8;

			NBSAppAgent.SetSetOption (NBSOption_Net);
			NBSAppAgent.SetSetOption (NBSOption_UI);
			NBSAppAgent.SetSetOption (NBSOption_Crash);
			NBSAppAgent.SetSetOption (NBSOption_Socket);
			NBSAppAgent.SetSetOption (NBSOption_StrideApp);
			NBSAppAgent.SetSetOption (NBSOption_ANR);
			NBSAppAgent.SetSetOption (NBSOption_Behaviour);
			NBSAppAgent.SetSetOption (NBSOption_CDNHeader);
			NBSAppAgent.StartWithAppID ("50c28404df0c4241b467412f46259206", locationAllowed: true);

			FileSystemUtil.Disabled_iCloudBackup ();

			_window = new UIWindow (UIScreen.MainScreen.Bounds);

			var nav = new XrmNavigationController (new HomeViewController ());

			AlertUtil.Initialize (nav);
			UiStyleSetting.Initialize (app, nav);

			_window.RootViewController = nav;
			_window.MakeKeyAndVisible ();

			//GlobalAppSetting.ReFreshTokenAddress = "http://192.168.2.22:9998/";
			GlobalAppSetting.PmsServerUrl = "https://m.cloudhotels.cn/";
			if (string.IsNullOrEmpty (GlobalAppSetting.XrmWebApiBaseUrl)) {
				//GlobalAppSetting.XrmWebApiBaseUrl = "https://mhms.homeinns.com/";
				GlobalAppSetting.XrmWebApiBaseUrl = "http://hmstest.yitel.com/";
			}

			_pushService = new PushService ();
			_pushService.RegisterNotification (app, options);
			_pushService.HandleRemoteNotification (app, options);
			_pushService.SetBadge (app, 0);

			return true;
		}

		//public override void ReceiveMemoryWarning (UIApplication application)
		//{
		//	NSUrlCache.SharedCache.RemoveAllCachedResponses ();
		//}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			_pushService.HandleRemoteNotification (application, userInfo);
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			_pushService.RegisterDeviceToken (application, deviceToken);
		}

		public override void WillEnterForeground (UIApplication application)
		{
			_pushService.SetBadge (application, 0);
			if (AuthenticationService.IsLogOn ()) {
				ContactsService.StartSyncContact ();
				AuthTokenRefreshService.StartRefreshToken ();
			}
			ChatClient.WillEnterForeground ();
		}

		public override void DidEnterBackground (UIApplication application)
		{
			ContactsService.StopSyncContact ();
			AuthTokenRefreshService.StopRefreshToken ();
			ChatClient.DidEnterBackground ();
		}
	}
}

