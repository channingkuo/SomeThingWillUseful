#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 与聊天相关的设置项目
****************/
#endregion
using RekTec.Corelib.Configuration;

namespace RekTec.Chat.MT.Configuration
{
    /// <summary>
    /// 与聊天相关的设置项目
    /// </summary>
    public static class ChatAppSetting
    {

        

        /// <summary>
        /// Openfire聊天服务器的HostConferenceName
        /// </summary>
        public static string HostConferenceName
        {
            set
            {
                GlobalAppSetting.SetValue("chat_HostConferenceName", value);
            }
            get
            {
                var v = GlobalAppSetting.GetValue("chat_HostConferenceName");
                if (string.IsNullOrWhiteSpace(v))
                    return "conference." + GlobalAppSetting.HostName;
                else
                    return v;
            }
        }

        /// <summary>
        /// Openfire聊天服务器的TCP端口号
        /// </summary>
        public static int ChatServerPort
        {
            get
            {
                const int defaultPort = 5222;
                var v = GlobalAppSetting.GetValue("chat_ChatServerPort");
                if (string.IsNullOrWhiteSpace(v))
                    return defaultPort;

                int port;
                var r = int.TryParse(v, out port);
                if (r)
                    return port;
                else
                    return defaultPort;
            }
            set
            {
                GlobalAppSetting.SetValue("chat_ChatServerPort", value.ToString());
            }
        }

        /// <summary>
        /// Openfire聊天服务器的IP地址
        /// </summary>
        public static string ChatServerAddress
        {
            get
            {
                return GlobalAppSetting.GetValue("chat_ChatServerAddress");
            }
            set
            {
                GlobalAppSetting.SetValue("chat_ChatServerAddress", value);
            }
        }
    }
}
