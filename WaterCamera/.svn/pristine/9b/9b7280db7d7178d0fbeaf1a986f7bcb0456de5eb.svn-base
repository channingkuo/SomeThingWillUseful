using Foundation;

namespace RekTec.Chat.DataRepository
{
    public static class ChatAppSetting
    {
        private static NSUserDefaults _defaults;

        public static void SetValue(string key, string value)
        {
            if (_defaults == null)
                _defaults = NSUserDefaults.StandardUserDefaults;

            _defaults.SetString(value, key);
        }

        public static string GetValue(string key)
        {
            if (_defaults == null)
                _defaults = NSUserDefaults.StandardUserDefaults;

            return _defaults.StringForKey(key);
        }

        public static string ChatAppVersion {
            get { 
                return "v23";
            }
        }

        public static string UserCode {
            get {
                return GetValue("chat_UserCode");
            }
            set {
                SetValue("chat_UserCode", value);
            }
        }

        public static string Password {
            get {
                return GetValue("chat_Password");
            }
            set {
                SetValue("chat_Password", value);
            }
        }

        public static string DeviceToken {
            get {
                return GetValue("chat_DeviceToken");
            }
            set {
                SetValue("chat_DeviceToken", value);
            }
        }

        public static string ContactsLastUpdateTime {
            get {
                return GetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_ContactsLastUpdateTime");
            }
            set {
                SetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_ContactsLastUpdateTime", value);
            }
        }

        public static string HostName {
            set { 
                SetValue("chat_HostName", value);
            }
            get {
                var v = GetValue("chat_HostName");
                if (string.IsNullOrWhiteSpace(v))
                    return "joe-nb-t430";
                else
                    return v;
            }
        }

        public static string HostConferenceName {
            set { 
                SetValue("chat_HostConferenceName", value);
            }
            get {
                var v = GetValue("chat_HostConferenceName");
                if (string.IsNullOrWhiteSpace(v))
                    return "conference." + HostName;
                else
                    return v;
            }
        }

        public static int ChatServerPort {
            get {
                var defaultPort = 5222;
                var v = GetValue("chat_ChatServerPort");
                if (string.IsNullOrWhiteSpace(v))
                    return defaultPort;

                var port = defaultPort;
                var r = int.TryParse(v, out port);
                if (r)
                    return port;
                else
                    return defaultPort;
            }
            set {
                SetValue("chat_ChatServerPort", value.ToString());
            }
        }

        public static string ChatServerAddress {
            get {
                return GetValue("chat_ChatServerAddress");
            }
            set {
                SetValue("chat_ChatServerAddress", value);
            }
        }

        public static bool IsNotified {
            get {
                var v = GetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotified");
                if (string.IsNullOrWhiteSpace(v))
                    return true;

                return GetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotified") == "1";
            }
            set {
                SetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotified", value ? "1" : "0");
            }
        }

        public static bool IsNotifiedVoice {
            get {
                var v = GetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotifiedVoice");
                if (string.IsNullOrWhiteSpace(v))
                    return true;

                return GetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotifiedVoice") == "1";
            }
            set {
                SetValue("chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotifiedVoice", value ? "1" : "0");
            }
        }

        private static string IsNotifiedBeepKey {
            get {
                return "chat" + "_" + ChatAppVersion + "_" + UserCode + "_IsNotifiedBeep";
            }
        }

        public static bool IsNotifiedBeep {
            get {
                var v = GetValue(IsNotifiedBeepKey);
                if (string.IsNullOrWhiteSpace(v))
                    return true;

                return GetValue(IsNotifiedBeepKey) == "1";
            }
            set {
                SetValue(IsNotifiedBeepKey, value ? "1" : "0");
            }
        }
    }
}

