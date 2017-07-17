using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using RekTec.Chat.DataRepository;
using RekTec.Chat.Service;
using RekTec.Chat.ViewModels;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.Contact
{
    public class RoomDetailViewController:UITableViewController
    {
        private ChatRoomViewModel _room;
        private List<ChatRoomMemberViewModel> _members;
        private bool _isRoomOwner;
        private List<UIImageView> _iconList = new List<UIImageView>();
        private List<UIButton> _rosterButtons = new List<UIButton>();

        public RoomDetailViewController(ChatRoomViewModel room)
            : base(UITableViewStyle.Grouped)
        {
            this.TableView.BackgroundView = null;
            this.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;

            _members = new List<ChatRoomMemberViewModel>();
            _room = room;
        }

        private void LoadRoomMembersData()
        {
            AlertUtil.ShowWaiting("正在加载群聊成员...");
            ChatClient.GetRoomMembers(_room.ChatRoomId, () => {
                InvokeOnMainThread(() => {
                    _members = ChatDataRepository.GetRoomMembersByRoomId(_room.ChatRoomId);
                    _isRoomOwner = _members.FirstOrDefault(m => m.ContactId.ToLower() == ChatClient.CurrentUserContact.ContactId.ToLower() && m.IsOwner == true) != null;
                    _members = _members.Where((r) => r.ContactId.ToLower() != ChatClient.CurrentUserContact.ContactId.ToLower())
                .ToList();
                    TableView.ReloadRows(new []{ NSIndexPath.FromRowSection(0, 0) }, UITableViewRowAnimation.None);
                    AlertUtil.DismissWaiting();
                });
            });
        }

        private bool HandleError(string msg)
        {
            AlertUtil.Error(msg);
            return true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ErrorHandlerUtil.Subscribe(HandleError);

            if (NavigationItem != null) {
                NavigationItem.Title = "群组详情";
            }

            this.TableView.Source = new Source(this);
            TableView.ReloadData();

            LoadRoomMembersData();

            ChatDataRepository.SubscribeChatRoomChange(RoomChange);
        }


        private void RoomChange(ChatRoomViewModel r, SqlDataChangeType t)
        {
            InvokeOnMainThread(() => {
                if (r.ChatRoomId == _room.ChatRoomId && t == SqlDataChangeType.Remove)
                    NavigationController.PopToRootViewController(true);
            });
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ErrorHandlerUtil.UnSubscribe(HandleError);
            ChatDataRepository.UnSubscribeChatRoomChange(RoomChange);
        }

        public class Source:UITableViewSource
        {
            RoomDetailViewController _c;

            public Source(RoomDetailViewController c)
            {
                _c = c;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Section == 0) {
                    #region 计算群组成员的Cell的高度
                    var padding = 10F;
                    var rosterButtonWidth = (_c.View.Bounds.Width - padding) / 4 - padding;
                    var labelHeight = 30F;
                    var rows = Math.Ceiling((_c._members.Count + 1 + (_c._isRoomOwner ? 1 : 0)) / 4.0);
                    return (float)((rosterButtonWidth + labelHeight) * rows) + padding;
                    #endregion
                } else if (indexPath.Section == 1)
                    return 50;
                else
                    return 50;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 4;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                if (section == 0)
                    return 1;
                else if (section == 1)
                    return 1;
                else if (section == 2)
                    return 1;
                else if (section == 3)
                    return 1;
                else
                    return 0;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Section == 0)
                    return GetMemberCell(tableView, indexPath);
                else if (indexPath.Section == 1)
                    return GetRoomNameCell(tableView, indexPath);
                else if (indexPath.Section == 2)
                    return GetClearButtonCell(tableView, indexPath);
                else if (indexPath.Section == 3)
                    return GetQuitButtonCell(tableView, indexPath);
                else
                    return null;

            }

            private UITableViewCell GetRoomNameCell(UITableView tv, NSIndexPath indexPath)
            {
                var id = _c._room.ChatRoomId;
                var cell = tv.DequeueReusableCell(id);
                if (cell == null) {
                    cell = new UITableViewCell(UITableViewCellStyle.Value1, id);
                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                }

                cell.TextLabel.Text = "群聊名称";
                cell.DetailTextLabel.Text = _c._room.ChatRoomName;
                return cell;
            }

            #region 构造群组成员的cell

            private NSIndexPath _memberCellIndexPath;

            private UITableViewCell GetMemberCell(UITableView tv, NSIndexPath indexPath)
            {
                _memberCellIndexPath = indexPath;
         
                var cell = new UITableViewCell(UITableViewCellStyle.Default, "MembersCell");
                BuildMembersCell(cell.ContentView);
               
                return cell;
            }

            private bool _isRemovingMember = false;


            private void BuildMembersCell(UIView contentView)
            {
                var row = 0;
                var col = 0;

                if (_c == null || _c._members == null || _c._members.Count == 0) {
                    return;
                }
                    
                var padding = 10F;
                var rosterWidth = (_c.View.Bounds.Width - padding) / 4 - padding;
                var labelHeight = 30F;

                _c._rosterButtons.Clear();

                for (var i = 0; i < _c._members.Count; i++) {

                    #region 循环创建成员的图标
                    var m = _c._members[i];
                    var contact = ContactsDataRepository.GetContactById(m.ContactId);
                    var contactName = string.Empty;
                    UIImage avatar = null;
                    if (contact != null) {
                        contactName = contact.ContactName;
                        avatar = contact.GetAvatarImage();
                    } else {
                        contactName = m.ContactId.Substring(0, m.ContactId.IndexOf("@"));
                        avatar = ContactViewModel.DefaultAvatar;
                    }

                    row = i / 4;

                    if (col == 4)
                        col = 0;

                    var button = new UIButton(UIButtonType.Custom);
                    button.Frame = new CGRect(col * (rosterWidth + padding) + padding,
                        row * (rosterWidth + labelHeight) + padding,
                        rosterWidth, rosterWidth);
                    button.SetBackgroundImage(avatar, UIControlState.Normal);
                    button.Tag = i;
                    button.AddTarget((s, e) => {
                        if (!_isRemovingMember)
                            return;

                        var btn = s as UIButton;
                        if (btn == null)
                            return;

                        var memberIndex = btn.Tag;
                                          
                        if (!_c._isRoomOwner)
                            return;
                                               
                        var memmber = _c._members[(int)memberIndex];
                        if (memmber == null)
                            return;
                            
                        ChatClient.RemoveRoomMember(memmber);
                        _c._members.RemoveAt((int)memberIndex);
                        _c._rosterButtons.RemoveAt((int)memberIndex);
                        _c.TableView.ReloadRows(new []{ _memberCellIndexPath }, UITableViewRowAnimation.None);

                    }, UIControlEvent.TouchUpInside);

                    _c._rosterButtons.Add(button);

                    contentView.AddSubview(button);

                    var lbl = new UILabel(
                                  new CGRect(col * (rosterWidth + padding) + padding,
                                      row * (rosterWidth + labelHeight) + padding + rosterWidth,
                                      rosterWidth, labelHeight
                                  ));
                    lbl.TextAlignment = UITextAlignment.Center;
                    lbl.Text = contactName;
                    contentView.AddSubview(lbl);

                    col++;
                    #endregion
                }
                    
                #region 添加成员的按钮
                if (col == 4) {
                    col = 0;
                    row++;
                }
                var buttonAdd = new UIButton(UIButtonType.Custom);
                buttonAdd.Frame = new CGRect(col * (rosterWidth + padding) + padding,
                    row * (rosterWidth + labelHeight) + padding,
                    rosterWidth, rosterWidth);
                buttonAdd.SetBackgroundImage(UIImage.FromFile("smiley_add_btn_nor.png"), UIControlState.Normal);
                buttonAdd.SetBackgroundImage(UIImage.FromFile("smiley_add_btn_pressed.png"), UIControlState.Highlighted);
                buttonAdd.AddTarget((s, e) => {
                    ClearRosterButtonRemoveIcon();
               
                    var view = new ContactChooseViewController();
                    view.ContactChooseCompleteAction = (l) => {
                        if (l != null && l.Count > 0) {
                            ChatClient.InviteMembersToRoom(l, _c._room);

                            _c.LoadRoomMembersData();
                            _c.TableView.ReloadRows(new []{ _memberCellIndexPath }, UITableViewRowAnimation.None);
                        }
                    };
                    _c.PresentViewController(view, false, null);

                }, UIControlEvent.TouchUpInside);
                contentView.AddSubview(buttonAdd);

                col++;
                #endregion

                #region 删除成员的按钮
                if (col == 4) {
                    col = 0;
                    row++;
                }

                if (_c._isRoomOwner) {
                    var buttonRemove = new UIButton(UIButtonType.Custom);
                    buttonRemove.Frame = new CGRect(col * (rosterWidth + padding) + padding,
                        row * (rosterWidth + labelHeight) + padding,
                        rosterWidth, rosterWidth);
                    buttonRemove.SetBackgroundImage(UIImage.FromFile("smiley_minus_btn_nor.png"), UIControlState.Normal);
                    buttonRemove.SetBackgroundImage(UIImage.FromFile("smiley_minus_btn_pressed.png"), UIControlState.Highlighted);
                    contentView.AddSubview(buttonRemove);

                    buttonRemove.AddTarget((s, e) => {
                        AddRosterButtonRemoveIcon();

                    }, UIControlEvent.TouchUpInside);
                }
                #endregion

                if (_isRemovingMember) {
                    AddRosterButtonRemoveIcon();
                }
            }

            #region 添加和清除 成员删除的角标

            private void AddRosterButtonRemoveIcon()
            {
                _isRemovingMember = true;
                _c._iconList.Clear();
                var image = UIImage.FromFile("group_invitee_delete.png");
                foreach (var b in _c._rosterButtons) {
                    var icon = new UIImageView(new CGRect(-10, -10, 30, 30));
                    icon.Image = image;
                    _c._iconList.Add(icon);
                    b.AddSubview(icon);
                }
            }

            private void ClearRosterButtonRemoveIcon()
            {
                _isRemovingMember = false;
                foreach (var i in _c._iconList) {
                    i.RemoveFromSuperview();
                }
                _c._iconList.Clear();
            }

            #endregion

            #endregion

            #region 按钮的Cell

            private UITableViewCell GetClearButtonCell(UITableView tv, NSIndexPath indexPath)
            {
                var cellId = "ClearButton";
                var cell = tv.DequeueReusableCell(cellId);
                if (cell == null) {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellId);

                    var caption = "清空聊天记录";
                    cell.TextLabel.Text = caption;
                    cell.TextLabel.TextColor = UiStyleSetting.ButtonTextBlue;
                }

                return cell;
            }


            private UITableViewCell GetQuitButtonCell(UITableView tv, NSIndexPath indexPath)
            {
                var cellId = "QueryButton";
                var cell = tv.DequeueReusableCell(cellId);
                if (cell == null) {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellId);

                    var caption = "退出群聊";
                    if (_c._isRoomOwner)
                        caption = "删除并" + caption;

                    cell.TextLabel.Text = caption;
                    cell.TextLabel.TextColor = UiStyleSetting.ButtonTextBlue;
                }

                return cell;
            }

            #endregion

            class ClearMessageConfirmDel:UIAlertViewDelegate
            {
                string _roomId;

                public ClearMessageConfirmDel(string roomId)
                {
                    _roomId = roomId;
                }

                public override void Clicked(UIAlertView alertview, nint buttonIndex)
                {
                    if (buttonIndex == 1 && !string.IsNullOrWhiteSpace(_roomId))
                        MessagesDataRepository.ClearMessagesByRoomId(_roomId);
                }
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Section == 0) {
                    ClearRosterButtonRemoveIcon();
                } else if (indexPath.Section == 1) {
                    _c.NavigationController.PushViewController(new RoomNameEditViewController(new RootElement("群组名称"), _c._room), true);
                } else if (indexPath.Section == 2) {
                    if (_c != null && _c._room != null && !string.IsNullOrWhiteSpace(_c._room.ChatRoomId)) {
                        var del = new ClearMessageConfirmDel(_c._room.ChatRoomId);
                        var alert = new UIAlertView("确认?", "确实要清空聊天记录？", del, "取消", "确认");
                        alert.Show();
                    }
                } else if (indexPath.Section == 3) {
                    ChatClient.LeaveRoom(_c._room);
                    _c.NavigationController.PopToRootViewController(true);
                } else {

                }
                tableView.DeselectRow(indexPath, true);
            }
           
        }
    }
}

