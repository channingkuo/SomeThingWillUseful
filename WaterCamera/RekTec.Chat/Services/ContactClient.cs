#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-08
说明 : 定时同步通讯录相关
****************/
using RekTec.Chat.WebApi;


#endregion
using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using agsXMPP;
using RekTec.Chat.DataRepository;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.Service
{
    /// <summary>
    /// 定时同步通讯录相关
    /// </summary>
    public static class ContactClient
    {
        private static object _connectionLocker = new object();

        // 最多隔15秒后重新连接

        public static ContactViewModel CurrentUserContact = new ContactViewModel();
        private static XmppClientConnection _xmppClientConnection;
        private static ChatContactService _contactService;
        private static ChatNotificationService _notificationService;

        private static UIApplication IosApplication;
        public static Action BeginConnecting;
        public static Action EndConnecting;

        private static Task Initialize()
        {
            return Task.Run(() => {
                lock (_connectionLocker) {
                    InitializeNull();

                    _xmppClientConnection = new XmppClientConnection();
                    _xmppClientConnection.Port = ChatAppSetting.ChatServerPort;
                    _contactService = new ChatContactService();
                    _notificationService = new ChatNotificationService(_xmppClientConnection);

                    SubscribeErrorEvent();
                }
            });
        }

        public static void Initialize(string serverAddress, string hostName, int port,
                                string webApiBaseUrl, string authToken, 
                                string userName, string password)
        {
            ChatAppSetting.ChatServerAddress = serverAddress;
            ChatAppSetting.HostName = hostName;
            ChatAppSetting.ChatServerPort = port;
            GlobalAppSetting.XrmWebApiBaseUrl = webApiBaseUrl;
            GlobalAppSetting.XrmAuthToken = authToken;
            ChatAppSetting.UserCode = userName;
            ChatAppSetting.Password = password;
        }

        private static void InitializeNull()
        {
            _xmppClientConnection = null;
            _contactService = null;
            _notificationService = null;
        }

        private static void SubscribeErrorEvent()
        {
            _xmppClientConnection.OnWriteXml += (sender, xml) => {
                System.Diagnostics.Debug.WriteLine("SND: " + xml);
            };

            _xmppClientConnection.OnReadXml += (sender, xml) => {
                System.Diagnostics.Debug.WriteLine("REC: " + xml);
            };

            _xmppClientConnection.OnStreamError += (sender, e) => {
                ErrorHandlerUtil.ReportError(e.ToString());
            };

            _xmppClientConnection.OnError += (sender, ex) => {
                ErrorHandlerUtil.ReportException(ex);
            };

            _xmppClientConnection.OnSocketError += (sender, ex) => {
                AutoReConnection();
            };
        }

        #region 连接、断开等相关的事件出来

        private static int _maxReconnectDelay = 30;
        private static int _currentReconnectDelay = 3;
        private static bool _isAutoConnectiong = false;

        private static void AutoReConnection()
        {
            if (!ContactClient._isLogOn || ChatClient.IsBackgroud)
                return;

            lock (_connectionLocker) {
                if (_currentReconnectDelay > _maxReconnectDelay)
                    _currentReconnectDelay = 3;

                _currentReconnectDelay++;
            }
            lock (_connectionLocker) {
                if (_isAutoConnectiong)
                    return;

                _isAutoConnectiong = true;
            }

            new System.Threading.Timer((state) => {
                CloseConnection();
                var t = OpenConnection();
                t.Wait();
                lock (_connectionLocker) {
                    _isAutoConnectiong = false;
                }
                if (!t.Result) {
                    AutoReConnection();
                }
            }, null, _currentReconnectDelay * 1000, -1);

        }

        private static async Task<bool> OpenConnection()
        {
            try {
                if (BeginConnecting != null)
                    BeginConnecting();

                //初始化服务对象
                await Initialize()
					.ConfigureAwait(continueOnCapturedContext: false);

                //var r = true;
                //启动定期同步联系人的Job程序
                _contactService.StartSyncContact();


                if (EndConnecting != null)
                    EndConnecting();

                return true;
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);

                if (EndConnecting != null)
                    EndConnecting();

                return false;
            }
        }

        private static void CloseConnection()
        {
            try {
                if (_notificationService != null && !_isLogOn)
                    _notificationService.UnSetApnsDeviceToken();

                if (_contactService != null)
                    _contactService.StopSyncContact();
            } catch (Exception ex) {
                LoggingUtil.Exception(ex);
            }
        }

        #endregion

        #region 通讯录相关的内容

        public static Task<bool> GetAllContacts()
        {
            try {
                return _contactService.GetAllContacts();
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
                return Task.Run(() => false);
            }
        }

        //		public static void SetMyAvatar(Action<string> cb)
        //		{
        //			try {
        //				_contactService.SetMyAvatar(cb);
        //			} catch (Exception ex) {
        //				ErrorUtil.ReportException(ex);
        //			}
        //		}
        //
        //		public static void SetMyProfile(Action<string> cb)
        //		{
        //			try {
        //				_contactService.SetMyProfile(cb);
        //			} catch (Exception ex) {
        //				ErrorUtil.ReportException(ex);
        //			}
        //		}

        #endregion


        #region 登录与登出相关

        private static bool _isLogOn { get; set; }

        public static bool IsLogOn()
        {
            return _isLogOn;
        }

        public static async Task<bool> Logon()
        {
            _isLogOn = true;

            ContactClient.CurrentUserContact.ContactId = GlobalAppSetting.UserCode.ToLower() + "@" + GlobalAppSetting.HostName.ToLower();

            //初始化本地sqlite数据库
            SqlDataRepository.Initialize(GlobalAppSetting.UserCode);

            return await OpenConnection();
        }

        public static void Logout()
        {
            _isLogOn = false;
            CloseConnection();
        }

        #endregion

        #region 前台后台切换

        public static bool IsBackgroud = false;

        public static async void WillEnterForeground()
        {
            IsBackgroud = false;

            //检查网路是否联通，如果不通，则等待10秒后，再重新连接
            var reachability = Reachability.ReachabilityForInternet();
            if (!reachability.IsReachable) {
                new System.Threading.Timer((state) => AutoReConnection(), null, 10 * 1000, -1);
                return;
            }

            try {
                if (ContactClient._isLogOn)
                    await OpenConnection();

            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }

        }

        public static void DidEnterBackground()
        {
            IsBackgroud = true;
            CloseConnection();
        }

        #endregion

        #region 消息推送

        public static void SetDeviceToken(string token)
        {
            ChatAppSetting.DeviceToken = token;
        }

        public static void RegisterForRemoteNotifications(UIApplication application)
        {
            if (application != null)
                IosApplication = application;

            if (IosApplication == null)
                return;

            if (ChatAppSetting.IsNotified) {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                    var types = UIUserNotificationType.Badge | UIUserNotificationType.Sound | UIUserNotificationType.Alert;
                    IosApplication.RegisterUserNotificationSettings(
                        UIUserNotificationSettings.GetSettingsForTypes(types, null)
                    );
                    IosApplication.RegisterForRemoteNotifications();

                } else {
                    var types = UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound | UIRemoteNotificationType.Alert;
                    IosApplication.RegisterForRemoteNotificationTypes(types);

                }
            } else {
                IosApplication.UnregisterForRemoteNotifications();
            }
        }

        public static void FinishedLaunching(UIApplication application, NSDictionary options)
        {
            if (application != null)
                IosApplication = application;

            if (IosApplication == null)
                return;

            IosApplication.ApplicationIconBadgeNumber = 0;
            if (options != null) {
                if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey)) {
                    var dict = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
                    if (dict != null) {

                    }
                }
            }
        }


        public static void ReceivedRemoteNotification(UIApplication app, NSDictionary userInfo)
        {
            app.ApplicationIconBadgeNumber = 0;
        }

        #endregion
    }
}



