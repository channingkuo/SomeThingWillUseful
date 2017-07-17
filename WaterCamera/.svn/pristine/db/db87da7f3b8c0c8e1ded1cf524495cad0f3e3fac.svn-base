using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RekTec.Chat.DataRepository;
using RekTec.Chat.WebApi;
using RekTec.Contacts.Configuration;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.Service
{
	internal class ChatContactService
	{
		//同步通讯录的间隔
		int _syncContactTimeSpan = 15 * 1000;
		Timer _syncTimer;
		//循环次数
		int foreachtime = 0;

		internal async Task<bool> GetAllContacts ()
		{
			try {
				//最后更新的时间
				var lastUpdate = ContactsAppSetting.ContactsLastUpdateTime;

				//从通讯录接口获取到的联系人
				var rosterSync = await WebApiFacade.GetRosters (lastUpdate);

				if (rosterSync == null) {
					var curr = ContactsDataRepository.GetContactById (ContactClient.CurrentUserContact.ContactId);
					if (curr != null)
						ContactClient.CurrentUserContact = curr;

					return true;
				}

				if (rosterSync.Contacts == null || rosterSync.Contacts.Count <= 0) {
					var curr = ContactsDataRepository.GetContactById (ContactClient.CurrentUserContact.ContactId);
					if (curr != null)
						ContactClient.CurrentUserContact = curr;

					return true;
				}

				var l = new List<ContactViewModel> (rosterSync.Contacts.Count);
				rosterSync.Contacts.ForEach (r => {
					foreachtime += 1;
					var jid = r.Code + "@" + ChatAppSetting.HostName;
					var c = ContactsDataRepository.GetContactById (jid);
					if (r.IsDisabled && c != null) {
						ContactsDataRepository.Delete (c);
					} else {
						l.Add (new ContactViewModel {
							XrmUserId = r.Id,
							ContactId = r.Code + "@" + ChatAppSetting.HostName,
							ContactName = r.Name,
							Phone = r.Telephone,
							Department = r.BusinessName,
							Position = r.Position,
							Email = r.EmailAddress,
							AvatarImageBase64String = r.Avatar,
							IsDisabled = r.IsDisabled,
							IsUpdate = false
						});
					}
				});
				ContactsDataRepository.AddOrUpdate (l);

				//从接口返回的数据插入数据库中更新最后更新时间
				if (foreachtime == rosterSync.Contacts.Count) {
					ContactsAppSetting.ContactsLastUpdateTime = rosterSync.SysnTime;
				}

				#region 更新头像
				//获取本地所有未更新头像的用户
				List<ContactViewModel> contacts = ContactsDataRepository.GetUnUpateContact ();
				if (contacts != null && contacts.Count > 0) {
					int index = 0;
					List<string> userList = new List<string> ();
					foreach (var contact in contacts) {
						userList.Add (contact.XrmUserId);
						index = index + 1;
						if (index % 10 == 0 || index == contacts.Count) {
							try {
								var contactList = await WebApiFacade.GetAvatars (userList.ToArray ());
								ContactsDataRepository.UpDateAvatr (contactList);
								//清空list中所有的数据
								userList.Clear ();
							} catch (Exception ex) {
								AlertUtil.Error (ex.Message);
							}
						}

					}
				}
				#endregion

				var meContact = ContactsDataRepository.GetContactById (ContactClient.CurrentUserContact.ContactId);
				if (meContact != null)
					ContactClient.CurrentUserContact = meContact;



				return true;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return false;
			}
		}

		internal void StartSyncContact ()
		{
			_syncTimer = null;
			_syncTimer = new Timer (SyncContactTimerCallback, null, _syncContactTimeSpan, _syncContactTimeSpan);
		}

		internal void StopSyncContact ()
		{
			if (_syncTimer != null)
				_syncTimer.Dispose ();

			_syncTimer = null;
		}

		private void SyncContactTimerCallback (object state)
		{
			var t = GetAllContacts ();
			t.Wait ();
		}

		//        internal async void SetMyAvatar(Action<string> cb)
		//        {
		//            try {
		//                if (string.IsNullOrWhiteSpace(ChatClient.CurrentUserContact.AvatarImageBase64String))
		//                    cb(string.Empty);
		//
		//                if (ChatClient.CurrentUserContact.AvatarImageBase64String == "null")
		//                    cb(string.Empty);
		//
		//                await WebApiFacade.UpdateAvatar(new UiAvatar {
		//                    UserName = ChatAppSetting.UserCode,
		//                    Avatar = ChatClient.CurrentUserContact.AvatarImageBase64String
		//                });
		//                cb(string.Empty);
		//            } catch (Exception ex) {
		//                cb(ex.Message);
		//            }
		//        }
		//
		//        internal async void SetMyProfile(Action<string> cb)
		//        {
		//            try {
		//				if (string.IsNullOrWhiteSpace(ContactClient.CurrentUserContact.Phone))
		//                    cb(string.Empty);
		//                   
		//				await WebApiFacade.UpdatePhoneNumber(new UiPhoneModel(){PhoneNumber=ContactClient.CurrentUserContact.Phone});
		//                cb(string.Empty);
		//            } catch (Exception ex) {
		//                cb(ex.Message);
		//            }
		//        }
	}
}

