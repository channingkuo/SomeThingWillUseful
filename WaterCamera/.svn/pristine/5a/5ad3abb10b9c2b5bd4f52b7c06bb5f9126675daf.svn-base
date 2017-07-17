#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 : joesong
日期 : 2015-05-04
说明 : 存储在本地数据库的对话列表的ViewModel
****************/
#endregion

using System;
using UIKit;
using SQLite;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;

namespace RekTec.Messages.ViewModels
{
	/// <summary>
	/// 存储在本地数据库的对话列表的ViewModel
	/// </summary>
	public class ChatListViewModel
	{
		private string _chatListId;

		[PrimaryKey]
		public string ChatListId {
			get { return _chatListId; }
			set { _chatListId = value; }
		}

		[Ignore]
		public string ChatListCode {
			get { return ChatListId.Substring (0, ChatListId.IndexOf ("@", StringComparison.Ordinal)); }
		}

		private string _chatListName;

		public string ChatListName {
			get { return _chatListName; }
			set { _chatListName = value; }
		}

		private DateTime _lastMessageDateTime;

		public DateTime LastMessageDateTime {
			get { return _lastMessageDateTime; }
			set { _lastMessageDateTime = value; }
		}

		private string _lastMessageContent;

		public string LastMessageContent {
			get { return _lastMessageContent; }
			set { _lastMessageContent = value; }
		}

		private ChatListType _listType = ChatListType.Private;

		public ChatListType ListType {
			get { return _listType; }
			set { _listType = value; }
		}

		private int _unReadCount = 0;

		public int UnReadCount {
			get { return _unReadCount; }
			set { _unReadCount = value; }
		}

		private string _orderByNum = "";

		public string OrderByNum {
			get { return _orderByNum; }
			set { _orderByNum = value; }
		}

		private string _actionUrl = "";

		public string ActionUrl {
			get{ return _actionUrl; }
			set{ _actionUrl = value; }
		}

		public UIImage GetAvatarImage ()
		{
			if (this.ChatListId == "1") {
				return UIImage.FromFile ("avatar_notice.png");
			} else if (this.ChatListId == "2") {
				return UIImage.FromFile ("avatar_task.png");
			} else if (this.ChatListId == "3") {
				return UIImage.FromFile ("avatar_reminer.png");
			}
			UIImage photoImage = null;
			if (ListType == ChatListType.Badge) {
				photoImage = ImageUtil.GetImageFromCache (BadgeMessageService.CacheItemPrefix + this.ChatListId.ToLower ());
			} else if (ListType == ChatListType.Private) {
				var contact = ContactsDataRepository.GetContactById (this.ChatListId);
				if (contact != null)
					photoImage = contact.GetAvatarImage ();
				else
					photoImage = ContactViewModel.DefaultAvatar;
			} else if (ListType == ChatListType.Group) {
				//var room = ChatDataRepository.GetRoomsById(this.ChatListId);
				//if (room != null)
				//    photoImage = room.GetAvatarImage();
				// else
				//    photoImage = ChatRoomViewModel.DefaultAvatar;
			} else {
				photoImage = ContactViewModel.DefaultAvatar;
			}

			if (photoImage == null) {
				photoImage = ContactViewModel.DefaultAvatar;
			}
			return photoImage;
		}

		public override string ToString ()
		{
			return string.Format ("[ChatListViewModel: ChatListId={0}, ChatListCode={1}, ChatListName={2}, LastMessageDateTime={3}, LastMessageContent={4}, ListType={5}]", ChatListId, ChatListCode, ChatListName, LastMessageDateTime, LastMessageContent, ListType);
		}
	}

	public enum ChatListType
	{
		Private,
		Group,
		Public,
		Badge
	}
}

