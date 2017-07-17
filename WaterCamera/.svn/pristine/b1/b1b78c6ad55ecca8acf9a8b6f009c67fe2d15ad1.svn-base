#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : Contacts的本地数据库存储的实现类
****************/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;

namespace RekTec.Contacts.Services
{
	/// <summary>
	///  Contacts的本地数据库存储的实现类
	/// </summary>
	public static class ContactsDataRepository
	{
        static readonly object LockerContactsDataRepository = new object();
		static ContactsDataRepository ()
		{
			SqlDataRepository.CreateTable<ContactViewModel> ();
			SqlDataRepository.CreateIndex<ContactViewModel> (c => c.ContactId);
		}

		private static readonly List<Action<ContactViewModel>> _contactChangeEvents = new List<Action<ContactViewModel>> ();

		public static ContactViewModel GetContactById (string contactId)
		{
			if (!SqlDataRepository.IsOpened)
				return null;
			if (string.IsNullOrWhiteSpace (contactId))
				return null;

			return SqlDataRepository.Table<ContactViewModel> ()
				.FirstOrDefault (c => c.ContactId.ToLower () == contactId.ToLower ());
		}

		public static ContactViewModel GetContactByCode (string contactCode)
		{
			if (!SqlDataRepository.IsOpened)
				return null;

			var contactId = contactCode.ToLower () + "@" + GlobalAppSetting.HostName;
			return SqlDataRepository.Table<ContactViewModel> ()
				.FirstOrDefault (c => c.ContactId.ToLower () == contactId.ToLower ());
		}

		public static List<ContactViewModel> GetAllContacts ()
		{
			if (!SqlDataRepository.IsOpened)
				return new List<ContactViewModel> (0);

			return SqlDataRepository.Table<ContactViewModel> ().ToList ();
		}

		public static int GetContactsCount ()
		{
			if (!SqlDataRepository.IsOpened)
				return 0;
			
			return SqlDataRepository.Table<ContactViewModel> ().Count ();
		}

		public static List<ContactViewModel> SearchContactsLikeName (string name)
		{
			if (string.IsNullOrWhiteSpace (name))
				return new List<ContactViewModel> (0);
			if (!SqlDataRepository.IsOpened)
				return new List<ContactViewModel> (0);
         
			name = name.ToUpper ();

			var contacts = SqlDataRepository.Table<ContactViewModel> ().ToList ();
			return contacts.Where ((c) =>
                    c.ContactName.Contains (name)
			|| c.ContactNamePinYinFirst.Contains (name)
			|| c.ContactNamePinYin.Replace (" ", "").Contains (name)
			).ToList ();
		}

		public static void AddOrUpdate (ContactViewModel contact)
		{
			if (!SqlDataRepository.IsOpened)
				return;

			if (contact == null)
				return;

			if (!string.IsNullOrWhiteSpace (contact.AvatarImageBase64String)) {
				FileSystemUtil.SaveBase64StringToCache (contact.ContactId, contact.AvatarImageBase64String);
				contact.AvatarImageBase64String = null;
			}

			var existsContact = SqlDataRepository.Table<ContactViewModel> ()
                    .AsQueryable ()
                    .FirstOrDefault (c => c.ContactId == contact.ContactId);

			if (existsContact == null) {
				SqlDataRepository.Insert (contact);
			} else {
				SqlDataRepository.Update (contact);
			}

			SqlDataRepository.Commit ();
         
			NotifyContactChange (contact);
		}

		public static void FirstAddOrUpdate (ContactViewModel contact)
		{
			if (!SqlDataRepository.IsOpened)
				return;
			if (contact == null)
				return;

			if (!string.IsNullOrWhiteSpace (contact.AvatarImageBase64String)) {
				FileSystemUtil.SaveBase64StringToCache (contact.ContactId, contact.AvatarImageBase64String);
				contact.AvatarImageBase64String = null;
			}

			SqlDataRepository.Insert (contact);
			SqlDataRepository.Commit ();
            
			NotifyContactChange (contact);
		}


		public static void Delete (ContactViewModel contact)
		{
			if (!SqlDataRepository.IsOpened)
				return;
			if (contact == null)
				return;

			var existsContact = SqlDataRepository.Table<ContactViewModel> ()
                    .AsQueryable ()
                    .FirstOrDefault (c => c.ContactId == contact.ContactId);

			if (existsContact != null) {
				SqlDataRepository.Delete (contact);
			}

			SqlDataRepository.Commit ();
            
			NotifyContactChange (contact);
		}

		public static void FirstDeleteCon (ContactViewModel contact)
		{
			if (!SqlDataRepository.IsOpened)
				return;
			if (contact == null)
				return;

			SqlDataRepository.Delete (contact);
			SqlDataRepository.Commit ();
            
			NotifyContactChange (contact);
		}

		public static void AddOrUpdate (List<ContactViewModel> contacts)
		{
			if (!SqlDataRepository.IsOpened || contacts == null || contacts.Count == 0)
				return;

			try {
				SqlDataRepository.BeginTransaction ();
				foreach (var contact in contacts) {
					if (!string.IsNullOrWhiteSpace (contact.AvatarImageBase64String)) {
						FileSystemUtil.SaveBase64StringToCache (contact.ContactId, contact.AvatarImageBase64String);
						contact.AvatarImageBase64String = null;
					}
					var existsContact = SqlDataRepository.Table<ContactViewModel> ()
						.FirstOrDefault (c => c.ContactId.ToLower () == contact.ContactId.ToLower ());
					if (existsContact == null) {
						SqlDataRepository.Insert (contact);
					} else {
						SqlDataRepository.Update (contact);
					}
				}

				SqlDataRepository.Commit ();
			} catch {
				SqlDataRepository.Rollback ();
				throw;
			}

			NotifyContactChange (null);
		}

		public static List<ContactViewModel> GetUnUpateContact ()
		{
			if (!SqlDataRepository.IsOpened)
				return null;

			return SqlDataRepository.Table<ContactViewModel> ().AsQueryable ()
                    .Where ((c) => c.IsUpdate == false)
                    .ToList ();
		}

		public static void UpDateAvatr (List<UiRoster> contactList)
		{
			if (contactList == null || contactList.Count <= 0)
				return;
			foreach (var contact in contactList) {
				string image = contact.Avatar;
				var existsContact = SqlDataRepository.Table<ContactViewModel> ()
                        .AsQueryable ()
                        .FirstOrDefault (c => c.XrmUserId == contact.Id);

				if (existsContact == null)
					return;

				if (!string.IsNullOrWhiteSpace (image)) {
					FileSystemUtil.SaveBase64StringToCache (existsContact.ContactId, image);
					existsContact.AvatarImageBase64String = null;
				}

				existsContact.IsUpdate = true;
				SqlDataRepository.Update (existsContact);
				SqlDataRepository.Commit ();
			}
		}

		public static void SubscribeContactChange (Action<ContactViewModel> cb)
		{
		    lock (LockerContactsDataRepository)
		    {
                _contactChangeEvents.Add(cb);    
		    }
		}

		public static void UnSubscribeContactChange (Action<ContactViewModel> cb)
		{
		    lock (LockerContactsDataRepository)
		    {
		        _contactChangeEvents.Remove(cb);
		    }
		}

		public static void NotifyContactChange (ContactViewModel c)
		{
		    lock (LockerContactsDataRepository)
		    {
		        _contactChangeEvents.ForEach(cb =>
		        {
		            if (cb != null)
		                Task.Run(()=>cb(c));
		        });
		    }
		}
	}
}

