#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :joe song
日期 :2015-07-10
说明 :服务器端获取消息列表最后一条消息内容
****************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Rest;
using RekTec.Corelib.Utils;
using RekTec.Messages.ViewModels;

namespace RekTec.Messages.Services
{
	/// <summary>
	/// 服务器端获取消息列表最后一条消息内容
	/// </summary>
	public static class BadgeMessageService
	{

		/// <summary>
		/// 从服务器端获取所有的消息类型
		/// </summary>
		private static async Task<List<MessageTypeModel>> GetMessageTypes ()
		{
			return await RestClient.GetAsync<List<MessageTypeModel>> ("api/message/GetMessageTypes")
                .ConfigureAwait (continueOnCapturedContext: false);
		}

		/// <summary>
		/// 获取某种消息的Icon的Base64图片
		/// </summary>
		private static async Task<string> GetMessageIconBase64 (string messageCode)
		{
			var retString = await RestClient.GetAsync (string.Format ("api/message/GetBase64Icon?messageCode={0}", messageCode))
                .ConfigureAwait (continueOnCapturedContext: false);

			return retString.Replace ("\"", "");
		}

		/// <summary>
		/// 从服务器端获取所有的消息类型
		/// </summary>
		private static async Task<List<BadgeMessageModel>> GetBadgeMessages ()
		{
			return await RestClient.GetAsync<List<BadgeMessageModel>> ("api/message/GetBadgeMessages")
                .ConfigureAwait (continueOnCapturedContext: false);
		}

		public static readonly string CacheItemPrefix = "badge_messages_";

		/// <summary>
		/// 服务器端获取消息列表最后一条消息内容
		/// </summary>
		public static async Task SyncLastMessages ()
		{
			try {
				var messageTypes = await GetMessageTypes ();

				var badgeMessages = await GetBadgeMessages ()
                .ConfigureAwait (continueOnCapturedContext: false);

				if (badgeMessages == null || badgeMessages.Count == 0)
					return;

				foreach (var message in badgeMessages) {
					var messageName = messageTypes
                        .Where (m => m.Code == message.From)
                        .Select (m => m.Name)
                        .FirstOrDefault ();
					if (string.IsNullOrWhiteSpace (messageName)) {
						messageName = message.From;
					}

                    //消息list本地不存在，并且Badge==0，则不插入到本地
                    var existsChatList = SqlDataRepository.Table<ChatListViewModel>()
                    .AsQueryable()
                    .FirstOrDefault(c => c.ChatListId.ToLower() == message.From.ToLower());
                    if(existsChatList == null && message.Badge <= 0)
                        continue;
				    
					string messageIcon = FileSystemUtil.GetBase64StringFromCache (CacheItemPrefix + message.From.ToLower ());
					if (messageIcon == null) {
						messageIcon = await GetMessageIconBase64 (message.From);
						FileSystemUtil.SaveBase64StringToCache (CacheItemPrefix + message.From.ToLower (), messageIcon);
					}
					var chatlist = new ChatListViewModel () {
						ChatListId = message.From,
						ListType = ChatListType.Badge,
						UnReadCount = message.Badge,
						LastMessageContent = message.Content,
						LastMessageDateTime = DateTime.Parse (message.Time),
						ChatListName = messageName,
						OrderByNum = message.Time,
						ActionUrl = message.Action
					};
                    
					MessagesDataRepository.AddOrUpdate (chatlist);
				}
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
			}
		}
	}
}