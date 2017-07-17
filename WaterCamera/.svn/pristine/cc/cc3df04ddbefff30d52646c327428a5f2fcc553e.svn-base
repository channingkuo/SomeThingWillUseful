#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-23 
说明     : 极光推送SDK的封装类
****************/
#endregion

using Foundation;
using UIKit;
using JPushSDK;

namespace RekTec.Messages.PushNotification.Services
{
    /// <summary>
    /// 极光推送SDK的封装类
    /// </summary>
    internal class JPushProvider : IPushProvider
    {
        /// <summary>
        /// 向极光的服务器器注册推送服务，应该在AppDelegate的FinishedLaunching 方法中调用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public void RegisterNotification(UIApplication app,NSDictionary options)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
				var types = UIUserNotificationType.Badge | UIUserNotificationType.Sound | UIUserNotificationType.Alert;
			    APService.RegisterForRemoteNotificationTypes((uint)types, null);
			} else {
				var types = UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound | UIRemoteNotificationType.Alert;
                APService.RegisterForRemoteNotificationTypes((uint)types, null);
			}

			APService.SetupWithOption(options);
        }

        
        /// <summary>
        /// 处理接收到的消息，应该在AppDelete的ReceivedRemoteNotification和FinishedLaunching中调用
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
	    public void HandleRemoteNotification(UIApplication application, NSDictionary userInfo)
	    {
            APService.HandleRemoteNotification(userInfo);
	    }

        /// <summary>
        /// 向极光的服务器注册设备的DeviceToken，应该在AppDelegate的RegisteredForRemoteNotifications调用
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        public void RegisterDeviceToken(UIApplication application, NSData deviceToken)
        {
            APService.RegisterDeviceToken(deviceToken);
        }

        /// <summary>
        /// 向极光的应用服务器注册用户，应该在xrm
        /// </summary>
        public void Login(string userId,string password)
        {
            var jpushAlias = userId.ToLower().Replace("-", "").Replace("{", "").Replace("}", "");
            APService.SetAlias(jpushAlias, null, null);
        }

        public void SetBadge(UIApplication app, int badges)
        {
            APService.SetBadge(badges);
        }
    }
}
