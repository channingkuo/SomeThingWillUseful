#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : Contacts的内存缓存实现类，解决当联系人非常多的时候，加载速度慢的问题
****************/
#endregion

using System.Collections.Generic;
using System.Collections.Concurrent;
using RekTec.Contacts.ViewModels;

namespace RekTec.Contacts.Services
{
    /// <summary>
    ///  Contacts的内存缓存实现类，解决当联系人非常多的时候，加载速度慢的问题
    /// </summary>
    public static class ContactsDataCache
    {
        private static ConcurrentDictionary<string,ContactViewModel> _contacts;

        public static void Initialize(IList<ContactViewModel> l)
        {
            _contacts = new ConcurrentDictionary<string,ContactViewModel>();
            foreach (var c in l) {
                _contacts.AddOrUpdate(c.ContactId.ToLower(), c, (key, oldValue) => {
                    return c;
                });
            }
        }

        public static ContactViewModel GetContactById(string contactId)
        {
            if (_contacts == null || !_contacts.ContainsKey(contactId.ToLower()))
                return null;

            return _contacts[contactId.ToLower()];
        }

        public static List<ContactViewModel> GetAllContacts()
        {
            if (_contacts == null)
                return null;
			
            var l = new List<ContactViewModel>(_contacts.Keys.Count);
            foreach (var v in _contacts.Values) {
                l.Add(v);
            }
            return l;
        }

        public static void AddOrUpdate(ContactViewModel c)
        {
            if (_contacts == null)
                return;

            if (_contacts.ContainsKey(c.ContactId.ToLower()))
                _contacts[c.ContactId.ToLower()] = c;
            else
                _contacts.AddOrUpdate(c.ContactId.ToLower(), c, (key, oldValue) => {
                    return c;
                });
        }

        public static void Remove(string contactId)
        {
            if (_contacts == null || !_contacts.ContainsKey(contactId.ToLower()))
                return;
            ContactViewModel c;
            _contacts.TryRemove(contactId.ToLower(), out c);
        }
    }
}

