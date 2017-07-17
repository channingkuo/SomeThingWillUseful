#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-23 
说明     : 消息推送的Provider的接口，可能的实现如极光、百度、OpenFire等
****************/
#endregion

using Foundation;
using UIKit;

namespace RekTec.Messages.PushNotification.Services
{
    /// <summary>
    /// 消息推送的Provider的接口，可能的实现如极光、百度、OpenFire等
    /// </summary>
    internal interface IPushProvider
    {
        /// <summary>
        /// 向服务器器注册推送服务，应该在AppDelegate的FinishedLaunching 方法中调用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        void RegisterNotification(UIApplication app,NSDictionary options);

        /// <summary>
        /// 处理接收到的消息，应该在AppDelete的ReceivedRemoteNotification和FinishedLaunching中调用
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        void HandleRemoteNotification(UIApplication application, NSDictionary userInfo);

        /// <summary>
        /// 向服务务器注册设备的DeviceToken，应该在AppDelegate的RegisteredForRemoteNotifications调用
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        void RegisterDeviceToken(UIApplication application, NSData deviceToken);

        /// <summary>
        /// 向应用服务器注册用户，应该在xrm
        /// </summary>
        void Login(string userId,string password);


        /// <summary>
        /// 设置APP的Badges
        /// </summary>
        void SetBadge(UIApplication app, int badges);
    }
}