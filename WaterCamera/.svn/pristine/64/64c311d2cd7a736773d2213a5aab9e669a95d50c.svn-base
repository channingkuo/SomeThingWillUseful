using MonoTouch.Dialog;
using UIKit;
using RekTec.Chat.DataRepository;
using RekTec.Chat.Service;
using RekTec.Chat.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.Views.Contact
{
    public class RoomNameEditViewController:DialogViewController
    {
        RootElement _root;
        EntryElement _roomNameElement;
        ChatRoomViewModel _room;

        public RoomNameEditViewController(RootElement r, ChatRoomViewModel room)
            : base(UITableViewStyle.Plain, r, true)
        {
            this.TableView.BackgroundView = null;
            this.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;

            _root = r;
            _room = room;
            _roomNameElement = new EntryElement("群组名称:", "请输入群组名称", room.ChatRoomName);
            _root.Add(new Section() {
                _roomNameElement
            });

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Save, (sender, e) => {
                _room.ChatRoomName = _roomNameElement.Value;
                ChatClient.ChangeRoomName(_room);
                NavigationController.PopToRootViewController(true);
            }), false);
        }

        private bool HandleError(string msg)
        {
            AlertUtil.Error(msg);
            return true;
        }

        private void RoomChange(ChatRoomViewModel r, SqlDataChangeType t)
        {
            InvokeOnMainThread(() => {
                if (r.ChatRoomId == _room.ChatRoomId && t == SqlDataChangeType.Remove)
                    NavigationController.PopToRootViewController(true);
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ErrorHandlerUtil.Subscribe(HandleError);
            ChatDataRepository.SubscribeChatRoomChange(RoomChange);
            if (NavigationItem != null) {
                NavigationItem.Title = "群组名称编辑";
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ErrorHandlerUtil.UnSubscribe(HandleError);
            ChatDataRepository.UnSubscribeChatRoomChange(RoomChange);
        }
    }
}

