using System.Collections.Generic;
using RekTec.Chat.ViewModels;
using RekTec.Contacts.ViewModels;

namespace RekTec.Chat.DataRepository
{
    public static class ChatContactDataCache
    {
        private static Dictionary<string,ContactViewModel> _contacts;

        public static void Initialize(IList<ContactViewModel> l)
        {
            _contacts = new Dictionary<string,ContactViewModel>(l.Count);
            foreach (var c in l) {
                _contacts.Add(c.ContactId.ToLower(), c);
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
                _contacts.Add(c.ContactId.ToLower(), c);
        }

        public static void Remove(string contactId)
        {
            if (_contacts == null || !_contacts.ContainsKey(contactId.ToLower()))
                return;

            _contacts.Remove(contactId.ToLower());
        }
    }
}

