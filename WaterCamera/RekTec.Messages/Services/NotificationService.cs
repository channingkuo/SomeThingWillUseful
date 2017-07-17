#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-05-04
说明 : 服务器端获取消息列表最后一条消息内容
****************/
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RekTec.Corelib.Rest;
using RekTec.Corelib.Utils;
using RekTec.Messages.ViewModels;

namespace RekTec.Messages.Services
{
    /// <summary>
    /// 服务器端获取消息列表最后一条消息内容
    /// </summary>
    public static class NotificationService
    {
        /// <summary>
        /// 服务器端获取消息列表最后一条消息内容
        /// </summary>
        private static async Task<List<NotificationModel>> GetChatListMessage()
        {
            var apiUrl = "api/Pns/GetUnread";
            return await RestClient.GetAsync<List<NotificationModel>>(apiUrl)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// 服务器端获取消息列表最后一条消息内容
        /// </summary>
        public static async Task SyncLastNotification()
        {
            try {
                var chatListMessage = await GetChatListMessage()
                .ConfigureAwait(continueOnCapturedContext: false);

                if (chatListMessage == null || chatListMessage.Count == 0)
                    return;

                foreach (var message in chatListMessage) {
                    var chatlist = new ChatListViewModel() {
                        ChatListId = message.Type.ToString(),
                        UnReadCount = message.Count,
                        LastMessageContent = message.LastMessageContent,
                        LastMessageDateTime = message.LastMessageTime,
                        ChatListName = NotifactionTypeName.GetTypeName(message.Type.ToString())
                    };
                    MessagesDataRepository.AddOrUpdate(chatlist);
                }
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
            }
        }
    }
}

