using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using RekTec.Chat.Service;
using RekTec.Chat.Views.Chat;
using RekTec.Chat.Views.Contact;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Views;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.ChatList
{
    public class ChatListViewController : BaseViewController
    {
        private UITableView _tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _tableView = new UITableView(new CGRect(View.Bounds.X, View.Bounds.Y,
                View.Frame.Width, View.Frame.Height - UiStyleSetting.StatusBarHeight - UiStyleSetting.NavigationBarHeight - UiStyleSetting.TabBarHeight));

            Add(_tableView);
        }

        private void CreateNav()
        {
            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Compose, (sender, e) => {
                var view = new ContactChooseViewController();
                view.ContactChooseCompleteAction = (l) => {
                    StartChat(l);
                };
                PresentViewController(view, true, null);
            }), false);

            NavigationItem.Title = "对话";
            NavigationItem.TitleView = null;
        }

        private void ChatListChangeCallback(ChatListViewModel c)
        {
            InvokeOnMainThread(() => {
                _tableView.ReloadData();
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            CreateNav();
            _tableView.Source = new Source(this);
            _tableView.ReloadData();

            MessagesDataRepository.SubscribeChatListChange(ChatListChangeCallback);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagesDataRepository.UnSubscribeChatListChange(ChatListChangeCallback);
        }

        private void StartChat(List<ContactViewModel> contacts)
        {
            if (contacts == null || contacts.Count == 0)
                return;

            if (contacts.Count > 1) {
                ChatClient.CreateRoom(contacts, StartChatCallback);
            } else {
                StartChatCallback(new ChatListViewModel {
                    ChatListId = contacts[0].ContactId,
                    ChatListName = contacts[0].ContactName,
                    ListType = ChatListType.Private
                });
            }
        }

        private void StartChatCallback(ChatListViewModel chatList)
        {
            if (chatList == null)
                return;

            InvokeOnMainThread(() => {
                NavigationController.PushViewController(new ChatViewController() { ChatList = chatList }, false);
            });
        }

        private class Source : UITableViewSource
        {
            private List<ChatListViewModel> _chatLists;
            private ChatListViewController _controller;

            public Source(ChatListViewController controller)
            {
                _controller = controller;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                _chatLists = MessagesDataRepository.GetAllChatListsOrderByDateTimeDesc();
                return 1;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 65;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return _chatLists == null ? 0 : _chatLists.Count();
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (_chatLists == null)
                    return null;
                if (_chatLists.Count <= indexPath.Row)
                    return null;

                var chatList = _chatLists[indexPath.Row];

                var cellIdentifier = chatList.ChatListId;
                ChatListTableViewCell cell = tableView.DequeueReusableCell(cellIdentifier) as ChatListTableViewCell;
                if (cell == null) {
                    cell = new ChatListTableViewCell((NSString)cellIdentifier);
                }

                cell.UpdateCell(chatList);
                return cell;
            }

            public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
            {
                return true;
            }

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle != UITableViewCellEditingStyle.Delete)
                    return;

                if (_chatLists == null)
                    return;

                if (_chatLists.Count <= indexPath.Row)
                    return;

                var room = _chatLists[indexPath.Row];

                MessagesDataRepository.Remove(room);
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                if (_chatLists == null)
                    return;

                if (_chatLists.Count <= indexPath.Row)
                    return;

                var chatlist = _chatLists[indexPath.Row];
                chatlist.UnReadCount = 0;
                MessagesDataRepository.AddOrUpdate(chatlist);

                _controller.NavigationController.PushViewController(
                    new ChatViewController() { ChatList = chatlist }
                    , false);

                tableView.DeselectRow(indexPath, false);
            }
        }

    }
}

