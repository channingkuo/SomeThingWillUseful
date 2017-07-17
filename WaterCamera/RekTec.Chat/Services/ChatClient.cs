using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using agsXMPP;
using RekTec.Chat.DataRepository;
using RekTec.Chat.ViewModels;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Service
{
    public static class ChatClient
    {
        private static object _connectionLocker = new object();
      
        // 最多隔15秒后重新连接

        public static ContactViewModel CurrentUserContact = new ContactViewModel();
        private static XmppClientConnection _xmppClientConnection;
        private static ChatConnectionService _connectionService;
        private static ChatContactService _contactService;
        private static ChatFileMessageService _fileMessageService;
        private static ChatTextMessageService _textMessageServie;
        private static ChatRoomService _roomService;
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
                    _connectionService = new ChatConnectionService(_xmppClientConnection);
                    _contactService = new ChatContactService();
                    _fileMessageService = new ChatFileMessageService(_xmppClientConnection, _contactService);
                    _textMessageServie = new ChatTextMessageService(_xmppClientConnection, _contactService);
                    _roomService = new ChatRoomService(_xmppClientConnection);
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
            _connectionService = null;
            _contactService = null;
            _fileMessageService = null;
            _textMessageServie = null;
            _roomService = null;
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
            if (!ChatClient._isLogOn || ChatClient.IsBackgroud)
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
                //同步所有的联系人记录
//                var r = await _contactService.GetAllContacts()
//                    .ConfigureAwait(continueOnCapturedContext: false);
//                if (!r) {
//                    if (EndConnecting != null)
//                        EndConnecting();
//                    return false;
//                }
//                //启动定期同步联系人的Job程序
                _contactService.StartSyncContact();

//                //连接聊天服务器
//                r = await _connectionService.OpenConnection()
//                    .ConfigureAwait(continueOnCapturedContext: false);
//                if (r) {
//                    lock (_connectionLocker) {
//                        _currentReconnectDelay = 0;
//                    }
//                } else {
//                    if (EndConnecting != null)
//                        EndConnecting();
//                    CloseConnection();
//                    return false;
//                }
//                    
//                //向聊天服务器提交Apns的Token
//                _notificationService.SetApnsDeviceToken();
//
//                //从聊天服务器获取所有的Room，加入（使用Timer的方式Delay5秒后执行）
//                _roomService.GetAllMyRoomsAndJoin();
//
//                //查询消息队列里是否有消息，如果有，则发送
//                StartMessageSendingQueue();

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

                if (_connectionService != null)
                    _connectionService.CloseConnection();

                if (_contactService != null)
                    _contactService.StopSyncContact();

                lock (_messageSendQueueLocker) {
                    _isSendingMessage = false;
                }
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

        //        public static void SetMyAvatar(Action<string> cb)
        //        {
        //            try {
        //                _contactService.SetMyAvatar(cb);
        //            } catch (Exception ex) {
        //                ErrorUtil.ReportException(ex);
        //            }
        //        }
        //
        //        public static void SetMyProfile(Action<string> cb)
        //        {
        //            try {
        //                _contactService.SetMyProfile(cb);
        //            } catch (Exception ex) {
        //                ErrorUtil.ReportException(ex);
        //            }
        //        }

        #endregion

        #region 发送消息相关的代码

        private static object _messageSendQueueLocker = new object();
        private static bool _isSendingMessage = false;
        private static Queue<ChatMessageViewModel> _messageSendQueue = new Queue<ChatMessageViewModel>();

        public static async void StartMessageSendingQueue()
        {
            ChatMessageViewModel queuedMessage = null;
            var l = MessagesDataRepository.GetNotSendMessage();
            lock (_messageSendQueueLocker) {
                l.ForEach(m => {
                    if (!_messageSendQueue.Contains(m))
                        _messageSendQueue.Enqueue(m);
                });
                if (_messageSendQueue.Count > 0)
                    queuedMessage = _messageSendQueue.Dequeue();
            }

            if (queuedMessage != null)
                await SendMessageAsync(queuedMessage)
                    .ConfigureAwait(false);
        }

        public static async Task SendMessageAsync(ChatMessageViewModel message)
        {
            lock (_messageSendQueueLocker) {
                if (_isSendingMessage) {
                    _messageSendQueue.Enqueue(message);
                    return;
                }

                _isSendingMessage = true;
            }
            try {
                if (message.MessageContentType == ChatMessageContentType.Text)
                    await _textMessageServie.SendMessageAsync(message).ConfigureAwait(false);
                else
                    await _fileMessageService.SendFileAsync(message).ConfigureAwait(false);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            } finally {
                lock (_messageSendQueueLocker) {
                    _isSendingMessage = false;
                }
            }

            ChatMessageViewModel queuedMessage = null;
            lock (_messageSendQueueLocker) {
                if (_messageSendQueue.Count() > 0) {
                    queuedMessage = _messageSendQueue.Dequeue();

                }
            }
            if (queuedMessage != null)
                await SendMessageAsync(queuedMessage)
                    .ConfigureAwait(false);
        }

        public static async Task ReSendMessageAsync(string messageId)
        {
            try {
                var message = MessagesDataRepository.GetMessageById(messageId);
                if (message == null)
                    return;

                await SendMessageAsync(message)
                    .ConfigureAwait(false);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        #endregion

        #region 群聊的相关内容

        public static void CreateRoom(List<ContactViewModel> contacts, Action<ChatListViewModel> cb)
        {
            try {
                _roomService.CreateRoom(contacts, cb);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        public static void GetRoomMembers(string roomId, Action cb)
        {
            try {
                _roomService.GetRoomMembers(new agsXMPP.Jid(roomId), cb);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        public static void RemoveRoomMember(ChatRoomMemberViewModel member)
        {
            try {
                _roomService.RemoveRoomMember(member);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        public static void InviteMembersToRoom(List<ContactViewModel> contacts, ChatRoomViewModel room)
        {
            _roomService.InviteContactsToRoom(contacts, room);
        }

        public static void LeaveRoom(ChatRoomViewModel room)
        {
            try {
                _roomService.LeaveRoom(room);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

        public static void ChangeRoomName(ChatRoomViewModel room)
        {
            try {
                _roomService.ChangeRoomName(room);
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }

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

            ChatClient.CurrentUserContact.ContactId = ChatAppSetting.UserCode.ToLower() + "@" + ChatAppSetting.HostName.ToLower();

            //初始化本地sqlite数据库
            SqlDataRepository.Initialize(ChatAppSetting.UserCode);

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
                if (ChatClient._isLogOn)
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

