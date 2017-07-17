#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-23 
说明     : 消息相关的本地数据库存储，包括公告通知等推送消息和聊天的消息
****************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;
using RekTec.Messages.ViewModels;
using RekTec.MyProfile.Services;

namespace RekTec.Messages.Services
{
	/// <summary>
	/// 消息相关的本地数据库存储，包括公告通知等推送消息和聊天的消息
	/// </summary>
	public static class MessagesDataRepository
	{
		static readonly object LockerMessageListDataRepository = new object ();
		static readonly object LockerMessageDataRepository = new object ();
		static MessagesDataRepository ()
		{
			SqlDataRepository.CreateTable<ChatListViewModel> ();
			SqlDataRepository.CreateTable<ChatMessageViewModel> ();
		}

		#region ChatList

		private static readonly List<Action<ChatListViewModel>> _chatListChangeEvents = new List<Action<ChatListViewModel>> ();

		public static List<ChatListViewModel> GetAllChatLists ()
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChatListViewModel> (0);

			var list = SqlDataRepository.Table<ChatListViewModel> ().ToList ();
			return list;

		}

		public static List<ChatListViewModel> GetAllChatListsOrderByDateTimeDesc ()
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChatListViewModel> (0);

			return SqlDataRepository.Table<ChatListViewModel> ()
					.OrderByDescending (r => r.LastMessageDateTime)
					.ToList ();
		}


		public static void AddOrUpdate (ChatListViewModel chatList)
		{
			if (!SqlDataRepository.IsOpened)
				return;
			if (chatList == null)
				return;

			var existsChatList = SqlDataRepository.Table<ChatListViewModel> ()
					.AsQueryable ()
					.FirstOrDefault (c => c.ChatListId.ToLower () == chatList.ChatListId.ToLower ());

			if (existsChatList == null) {
				SqlDataRepository.Insert (chatList);
			} else {
				SqlDataRepository.Update (chatList);
			}

			SqlDataRepository.Commit ();
			NotifyChatListChange (chatList);
		}

		public static void ReadAll (ChatListViewModel chatList)
		{
			if (!SqlDataRepository.IsOpened)
				return;
			if (chatList == null)
				return;

			var msgs = SqlDataRepository.Table<ChatMessageViewModel> ()
					.AsQueryable ()
					.Where (m => m.FromId.ToLower () == chatList.ChatListId.ToLower ()
						&& m.MessageType == ChatMessageType.Receive
						&& m.MessageReceiveStatus == ChatMessageReceiveStatus.Received)
					.ToList ();
			msgs.ForEach (m => {
				m.MessageReceiveStatus = ChatMessageReceiveStatus.Readed;
				SqlDataRepository.Update (m);
			});

			chatList.UnReadCount = 0;
			SqlDataRepository.Update (chatList);

			SqlDataRepository.Commit ();
			NotifyChatListChange (chatList);
		}

		private static int GetChatListUnreadCount (string id)
		{
			if (!SqlDataRepository.IsOpened)
				return 0;
			if (string.IsNullOrWhiteSpace (id))
				return 0;

			var chatList = SqlDataRepository.Table<ChatListViewModel> ()
				.FirstOrDefault (l => l.ChatListId.ToLower () == id.ToLower ());

			if (chatList == null)
				return 0;

			var unreadCount = SqlDataRepository.Table<ChatMessageViewModel> ()
				.AsQueryable ().Count (m => m.FromId == id && m.MessageType == ChatMessageType.Receive && m.MessageReceiveStatus == ChatMessageReceiveStatus.Received);

			return unreadCount;
		}

		public static void Remove (ChatListViewModel list)
		{
			if (!SqlDataRepository.IsOpened)
				return;
			if (list == null)
				return;
			if (string.IsNullOrWhiteSpace (list.ChatListId))
				return;

			SqlDataRepository.Delete (list);

			SqlDataRepository.Commit ();
			NotifyChatListChange (list);
		}

		public static void SubscribeChatListChange (Action<ChatListViewModel> cb)
		{
			lock (LockerMessageListDataRepository) {
				_chatListChangeEvents.Add (cb);
			}
		}

		public static void UnSubscribeChatListChange (Action<ChatListViewModel> cb)
		{
			lock (LockerMessageListDataRepository) {
				_chatListChangeEvents.Remove (cb);
			}
		}

		public static void NotifyChatListChange (ChatListViewModel c)
		{
			lock (LockerMessageListDataRepository) {
				_chatListChangeEvents.ForEach (cb => {
					if (cb != null)
						Task.Run (() => cb (c));
				});
			}
		}

		#endregion

		#region chat message

		private static readonly List<Action<ChatMessageViewModel, SqlDataChangeType>> _messageChangeEvents = new List<Action<ChatMessageViewModel, SqlDataChangeType>> ();

		public static ChatMessageViewModel GetMessageById (string id)
		{
			if (!SqlDataRepository.IsOpened)
				return null;
			if (string.IsNullOrWhiteSpace (id))
				return null;

			return SqlDataRepository.Table<ChatMessageViewModel> ()
					.FirstOrDefault (m => m.ChatMessageId == id);
		}

		public static List<ChatMessageViewModel> GetRecentMessagesByRoomId (string roomId, int pageIndex, int pageSize)
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChatMessageViewModel> (0);

			if (string.IsNullOrWhiteSpace (roomId))
				return new List<ChatMessageViewModel> (0);

			if (pageIndex <= 0)
				pageIndex = 1;

			if (pageSize <= 0)
				pageSize = 10;

			return SqlDataRepository.Table<ChatMessageViewModel> ()
					.Where (m => m.FromId.ToLower () == roomId.ToLower () && m.MessageListType == ChatListType.Group
			 || m.ToId.ToLower () == roomId.ToLower () && m.MessageListType == ChatListType.Group)
					.OrderByDescending (m => m.SendDateTime)
					.Take (pageIndex * pageSize)
					.ToList ();
		}

		public static List<ChatMessageViewModel> GetRecentMessagesByContactId (string contactId, int pageIndex, int pageSize)
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChatMessageViewModel> (0);

			if (string.IsNullOrWhiteSpace (contactId))
				return new List<ChatMessageViewModel> (0);

			if (pageIndex <= 0)
				pageIndex = 1;

			if (pageSize <= 0)
				pageSize = 10;

			return SqlDataRepository.Table<ChatMessageViewModel> ()
					.Where (m =>
						 (m.ToId.ToLower () == contactId.ToLower () && m.FromId.ToLower () == AuthenticationService.CurrentUserInfo.SystemUserId.ToLower ())
			 || (m.FromId.ToLower () == contactId.ToLower () && m.ToId.ToLower () == AuthenticationService.CurrentUserInfo.SystemUserId.ToLower ())
			).OrderByDescending (m => m.SendDateTime)
					.Take (pageIndex * pageSize)
					.ToList ();
		}

		public static List<ChatMessageViewModel> GetNotSendMessage ()
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ChatMessageViewModel> (0);

			return SqlDataRepository.Table<ChatMessageViewModel> ()
					.Where (m =>
						 m.MessageType == ChatMessageType.Send
			 && m.MessageSendStatus == ChatMessageSendStatus.NotSend
			).OrderBy (m => m.SendDateTime)
					.ToList ();
		}

		public static Task AddOrUpdateAsync (ChatMessageViewModel msg)
		{
			return Task.Run (() => AddOrUpdate (msg));
		}

		public static void AddOrUpdate (ChatMessageViewModel msg)
		{
			if (msg == null)
				return;
			if (!SqlDataRepository.IsOpened)
				return;

			bool isMessageChange = false;
			bool isUpdate = false;
			ChatListViewModel newChatList = null;
			#region 获取 fromName 和 toName
			var fromName = msg.FromId;
			if (msg.MessageListType == ChatListType.Private) {
				var fromContact = SqlDataRepository.Table<ContactViewModel> ()
						.FirstOrDefault (c => c.ContactId == msg.FromId);
				if (fromContact != null)
					fromName = fromContact.ContactName;
			} else if (msg.MessageListType == ChatListType.Group) {
				//var fromRoom = SqlDataRepository.DbConnection.Table<ChatRoomViewModel>()
				//    .FirstOrDefault(r => r.ChatRoomId == msg.FromId);

				//if (fromRoom != null)
				//    fromName = fromRoom.ChatRoomName;
			}

			var toName = msg.ToId;
			if (msg.MessageListType == ChatListType.Private) {
				var toContact = SqlDataRepository.Table<ContactViewModel> ()
						.FirstOrDefault (c => c.ContactId == msg.ToId);
				if (toContact != null)
					toName = toContact.ContactName;
			} else if (msg.MessageListType == ChatListType.Group) {
				//var toRoom = SqlDataRepository.DbConnection.Table<ChatRoomViewModel>()
				//    .FirstOrDefault(r => r.ChatRoomId == msg.ToId);
				//if (toRoom != null)
				//    toName = toRoom.ChatRoomName;
			}

			msg.FromName = fromName;
			msg.ToName = toName;
			#endregion

			var existsMsg = SqlDataRepository.Table<ChatMessageViewModel> ().AsQueryable ()
					.FirstOrDefault (m => m.ChatMessageId == msg.ChatMessageId);

			#region 将文件保存在缓存中
			if (existsMsg == null) {
				if (msg.MessageContentType == ChatMessageContentType.Image) {
					FileSystemUtil.SaveBase64StringToCache (msg.ChatMessageId, msg.MessageContent);
					msg.MessageContent = "[图片]";
				} else if (msg.MessageContentType == ChatMessageContentType.Audio) {
					//SaveCachedFile(msg.ChatMessageId, msg.MessageContent);
					msg.MessageContent = "[语音]";
				} else if (msg.MessageContentType == ChatMessageContentType.Video) {
					//SaveCachedFile(msg.ChatMessageId, msg.MessageContent);
					msg.MessageContent = "[视频]";
				}
			}
			#endregion

			if (existsMsg != null) {
				isUpdate = true;
				if (existsMsg.MessageType == ChatMessageType.Send) {
					existsMsg.MessageSendStatus = msg.MessageSendStatus;
					SqlDataRepository.Update (existsMsg);
					isMessageChange = true;
				}

				if (existsMsg.MessageType == ChatMessageType.Receive
					&&
					msg.MessageReceiveStatus == ChatMessageReceiveStatus.Readed) {
					existsMsg.MessageReceiveStatus = ChatMessageReceiveStatus.Readed;
					SqlDataRepository.Update (existsMsg);
					isMessageChange = true;
				}
			} else {
				msg.CreatedOn = DateTime.Now;
				SqlDataRepository.Insert (msg);
				isMessageChange = true;
			}

			#region ChatList
			if (isMessageChange) {
				string chatListName = msg.MessageType == ChatMessageType.Send ? msg.ToName : msg.FromName;
				string chatListId = msg.MessageType == ChatMessageType.Send ? msg.ToId : msg.FromId;
				if (msg.MessageListType == ChatListType.Group) {
					//chatListId = msg.ChatRoomId;
					//var room = SqlDataRepository.DbConnection.Table<ChatRoomViewModel>()
					//    .FirstOrDefault(r => r.ChatRoomId.ToLower() == msg.ChatRoomId.ToLower());
					//if (room != null)
					//    chatListName = room.ChatRoomName;
					//else
					//    chatListName = chatListId;
				} else {
					var contact = SqlDataRepository.Table<ContactViewModel> ()
							.FirstOrDefault (c => c.ContactId.ToLower () == chatListId.ToLower ());
					if (contact != null)
						chatListName = contact.ContactName;
					else
						chatListName = chatListId;
				}

				newChatList = new ChatListViewModel {
					ChatListId = chatListId,
					ChatListName = chatListName,
					LastMessageContent = msg.MessageContent,
					LastMessageDateTime = msg.SendDateTime,
					ListType = msg.MessageListType,
					UnReadCount = GetChatListUnreadCount (chatListId)
				};

				var chatList = SqlDataRepository.Table<ChatListViewModel> ()
						.FirstOrDefault (l => l.ChatListId.ToLower () == chatListId.ToLower ());
				if (chatList == null)
					SqlDataRepository.Insert (newChatList);
				else
					SqlDataRepository.Update (newChatList);
			}
			#endregion

			SqlDataRepository.Commit ();

			if (isMessageChange) {
				NotifyChatListChange (newChatList);
				NotifyChatMessageChange (msg, isUpdate ? SqlDataChangeType.Update : SqlDataChangeType.Add);
			}
		}

		public static void ClearMessagesByContactId (string contactId)
		{
			if (!SqlDataRepository.IsOpened)
				return;

			if (string.IsNullOrWhiteSpace (contactId))
				return;

			var l = GetRecentMessagesByContactId (contactId, 1, int.MaxValue);
			if (l.Count == 0)
				return;

			l.ForEach (m => {
				SqlDataRepository.Delete (m);
			});

			SqlDataRepository.Commit ();

			NotifyChatMessageChange (null, SqlDataChangeType.Remove);
		}

		public static void ClearMessagesByRoomId (string roomId)
		{
			if (!SqlDataRepository.IsOpened)
				return;

			if (string.IsNullOrWhiteSpace (roomId))
				return;

			var l = GetRecentMessagesByRoomId (roomId, 1, int.MaxValue);
			if (l.Count == 0)
				return;

			l.ForEach (m => {
				SqlDataRepository.Delete (m);
			});

			SqlDataRepository.Commit ();
			NotifyChatMessageChange (null, SqlDataChangeType.Remove);
		}

		public static void SubscribeChatMessageChange (Action<ChatMessageViewModel, SqlDataChangeType> cb)
		{
			lock (LockerMessageDataRepository) {
				_messageChangeEvents.Add (cb);
			}
		}

		public static void UnSubscribeChatMessageChange (Action<ChatMessageViewModel, SqlDataChangeType> cb)
		{
			lock (LockerMessageDataRepository) {
				_messageChangeEvents.Remove (cb);
			}
		}

		public static void NotifyChatMessageChange (ChatMessageViewModel c, SqlDataChangeType t)
		{
			lock (LockerMessageDataRepository) {
				_messageChangeEvents.ForEach (
					cb => Task.Run (() => cb (c, t))
					);
			}
		}

		#endregion
	}
}

