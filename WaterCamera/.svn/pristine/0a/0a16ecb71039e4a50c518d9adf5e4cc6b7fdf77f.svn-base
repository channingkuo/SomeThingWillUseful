using UIKit;
using SQLite;

namespace RekTec.Chat.ViewModels
{
    public class ChatRoomViewModel
    {
        #region 联系人默认头像

        private static UIImage _defaultAvatar;

        public static UIImage DefaultAvatar {
            get {
                if (_defaultAvatar == null)
                    _defaultAvatar = UIImage.FromFile("room_default_avatar.png");

                return _defaultAvatar;
            }
        }

        #endregion

        [PrimaryKey]
        public string ChatRoomId { get; set; }

        public string ChatRoomName { get; set; }

        private string _contactNamePinYin;

        [Ignore]
        public string ContactNamePinYin {
            get {
                if (string.IsNullOrWhiteSpace(_contactNamePinYin))
                    _contactNamePinYin = NPinyin.Pinyin.GetPinyin(ChatRoomName, System.Text.Encoding.Unicode);

                return _contactNamePinYin;
            }
        }

        private string _contactNamePinYinFirst;

        [Ignore]
        public string ContactNamePinYinFirst {
            get {
                if (string.IsNullOrWhiteSpace(_contactNamePinYinFirst))
                    _contactNamePinYinFirst = NPinyin.Pinyin.GetInitials(ChatRoomName, System.Text.Encoding.Unicode);

                return _contactNamePinYinFirst;
            }
        }

        public UIImage GetAvatarImage()
        {
            return ChatRoomViewModel.DefaultAvatar;
        }

        public override string ToString()
        {
            return string.Format("[ChatRoomViewModel: ChatRoomId={0}, ChatRoomName={1}]", ChatRoomId, ChatRoomName);
        }
    }
}

