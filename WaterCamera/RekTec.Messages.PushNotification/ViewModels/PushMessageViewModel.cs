#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-23 
说明     : Apns推送过来的消息的ViewModel
****************/
#endregion

using System.Collections.Generic;
using Foundation;

namespace RekTec.Messages.PushNotification.ViewModels
{
    /// <summary>
    /// Apns推送过来的消息的ViewModel
    /// </summary>
    public class PushMessageViewModel
    {
        public string Content { get; set; }

        public string Badge { get; set; }

        public string Sound { get; set; }

        public string ObjectId { get; set; }

        public string ObjectType { get; set; }

        /// <summary>
        /// 将Apns推送过来的数据类型，转换为PushMessageViewModel
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static PushMessageViewModel TryParse(NSDictionary userInfo)
        {
            if (userInfo == null)
                return null;

            var msg = new PushMessageViewModel();
            var aps = userInfo.ValueForKey((NSString)"aps");
            msg.Content = aps.ValueForKey((NSString)"alert").ToString(); //推送显示的内容
            msg.Badge = aps.ValueForKey((NSString)"badge").ToString(); //ge数量
            msg.Sound = aps.ValueForKey((NSString)"sound").ToString(); //播放的声音

            // 取得自定义字段内容
            msg.ObjectId = userInfo.ValueForKey((NSString)"Id") == null ? string.Empty : userInfo.ValueForKey((NSString)"Id").ToString(); //自定义参数，key是自己定义的
            msg.ObjectType = userInfo.ValueForKey((NSString)"Type") == null ? "-1" : userInfo.ValueForKey((NSString)"Type").ToString();

            return msg;
        }

        /// <summary>
        /// 获取与推送通知相关的几个WebView的只读页面的Url地址
        /// </summary>
        public static string GetReadOnlyWeViewUrl(string type, string id)
        {
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(id))
                return null;

            if (type == "1") {
                return "notice/noticeRead/" + id;
            } else if (type == "2") {
                return "/task/read/" + id;
            }

            return null;
        }
    }
}
