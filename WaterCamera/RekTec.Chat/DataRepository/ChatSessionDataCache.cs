using System.Collections.Generic;
using UIKit;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;

namespace RekTec.Chat.DataRepository
{
    public static class ChatSessionDataCache
    {
        private static Dictionary<string,ContactViewModel> _contacts = null;
        private static Dictionary<string,UIImage> _avatars = null;

        public static void Initialize()
        {
            _contacts = new Dictionary<string, ContactViewModel>();
            _avatars = new Dictionary<string, UIImage>();
        }

        public static ContactViewModel GetContactById(string contactId)
        {
            if (_contacts.ContainsKey(contactId.ToLower())) {
                return _contacts[contactId.ToLower()];
            } else {
                var contact = ContactsDataRepository.GetContactById(contactId);
                if (contact != null) {
                    _contacts.Add(contactId.ToLower(), contact);
                    return contact;
                } else {
                    return null;
                }
            }
        }

        public static UIImage GetAvatarById(string contactId)
        {
            if (_avatars.ContainsKey(contactId.ToLower())) {
                return _avatars[contactId.ToLower()];
            } else {
                var contact = GetContactById(contactId);
                if (contact == null) {
                    return ContactViewModel.DefaultAvatar;
                } else {
                    var avatar = contact.GetAvatarImage();
                    if (avatar == null) {
                        return ContactViewModel.DefaultAvatar;
                    } else {
                        _avatars.Add(contactId.ToLower(), avatar);
                        return avatar;
                    }
                }
            }
        }
    }
}

