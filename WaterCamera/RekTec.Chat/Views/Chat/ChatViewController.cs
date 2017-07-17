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
using RekTec.Chat.Views.Contact;
using RekTec.Contacts.Services;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Views;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.Views.Chat
{
    [Register("ChatViewController")]
    public class ChatViewController : BaseViewController
    {
        private const float _chatTextMaxHeight = 90;
        private const int _chatToolBarHeight = 40;
        private const int _pageSize = 10;

        private int _currentPageIndex = 1;
        private nfloat previousContentHeight;

        public ChatListViewModel ChatList { get; set; }

        UIViewBuilder _viewBuilder;
        RootElement _rootContactDetail;

        public override void ViewDidLoad()
        {
            _rootContactDetail = new RootElement("详细信息");
            base.ViewDidLoad();
            _viewBuilder = new UIViewBuilder(this.View);
            UIKeyboard.Notifications.ObserveWillShow(PlaceKeyboard);
            UIKeyboard.Notifications.ObserveWillHide(PlaceKeyboard);

            //导航栏部分
            CreateNav();

            //聊天消息列表部分
            CreateChatBubbleList();

            //按钮工具栏部分
            CreateChatToolbar();

            //文字输入框部分
            CreateInputTextView();

            //图片、拍照、视频部分
            CreateMoreView();
            SetMoreViewButtonFrame();

            //emoji表情部分
            CreateEmojiView();
            SetEmojiViewButtonFrame();
        }


        private void RoomChange(ChatRoomViewModel r, SqlDataChangeType t)
        {
            InvokeOnMainThread(() => {
                if (r.ChatRoomId.ToLower() == ChatList.ChatListId.ToLower() && t == SqlDataChangeType.Remove)
                    NavigationController.PopToRootViewController(true);
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationItem.Title = ChatList.ChatListName;
            if (ChatList.ListType == ChatListType.Group) {
                ChatDataRepository.SubscribeChatRoomChange(RoomChange);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (ChatList.ListType == ChatListType.Group) {
                ChatDataRepository.UnSubscribeChatRoomChange(RoomChange);
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            _discussionViewController.View.Frame = _discussionHost.Frame;
            ScrollToBottom(false);
        }

        #region 导航栏的按钮部分

        private void CreateNav()
        {
            NavigationItem.Title = ChatList.ChatListName;
            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIImage.FromFile("contact_detail.png"), UIBarButtonItemStyle.Plain, (sender, e) => {
                if (ChatList == null)
                    return;

                if (ChatList.ListType == ChatListType.Group) {
                    var room = ChatDataRepository.GetRoomsById(ChatList.ChatListId);
                    if (room != null)
                        NavigationController.PushViewController(new RoomDetailViewController(room), true);
                } else if (ChatList.ListType == ChatListType.Private) {
                    var contact = ContactsDataRepository.GetContactById(ChatList.ChatListId);
                    if (contact != null)
                        NavigationController.PushViewController(new ContactDetailViewController(_rootContactDetail, contact), true);
                }
            }), false);
        }

        private UIImageView _chatToolBar;
        private UIViewBuilder _chatToolBarBuilder;

        private void CreateChatToolbar()
        {
            _chatToolBar = _viewBuilder.CreateToolBarByImageView();
            _chatToolBar.Frame = new CGRect(0, View.Bounds.Height - _chatToolBarHeight, View.Bounds.Width, _chatToolBarHeight);
            _chatToolBarBuilder = new UIViewBuilder(_chatToolBar);
        }

        #endregion

        #region 聊天消息列表部分

        private RootElement _root;
        private UIView _discussionHost;
        private DialogViewController _discussionViewController;
        private UIRefreshControl _refreshControl;

        private void CreateChatBubbleList()
        {
            _root = new RootElement(ChatList.ChatListName) { new Section() };
            _root.UnevenRows = true;

            _discussionViewController = new DialogViewController(UITableViewStyle.Plain, _root, true);
            _discussionViewController.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _discussionViewController.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;
            _discussionViewController.OnSelection += ChatMessageSelection;

            _refreshControl = new UIRefreshControl();
            _discussionViewController.RefreshControl = _refreshControl;
            _refreshControl.ValueChanged += RefreshControlValueChanged;

            _discussionHost = new UIView(new CGRect(View.Bounds.X, View.Bounds.Y, View.Bounds.Width, View.Bounds.Height - _chatToolBarHeight)) {
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                AutosizesSubviews = true,
                UserInteractionEnabled = true
            };
            View.AddSubview(_discussionHost);
            _discussionHost.AddSubview(_discussionViewController.View);
        }

        private void RefreshControlValueChanged(object sender, EventArgs e)
        {
            InvokeOnMainThread(() => {
                if (_discussionViewController.Root[0].Count < _currentPageIndex * _pageSize) {
                    _refreshControl.EndRefreshing();
                    return;
                }
                _currentPageIndex++;

                ReloadData();

                _refreshControl.EndRefreshing();
            });
        }

        private void ChatMessageSelection(NSIndexPath indexPath)
        {
            View.EndEditing(true);
            _isEmojiShowing = false;
            _isMoreViewShowing = false;
            AdjustViewHeight(_chatToolBarHeight);
        }

        void ScrollToBottom(bool animated)
        {
            int row = _discussionViewController.Root[0].Elements.Count - 1;
            if (row == -1)
                return;
            _discussionViewController.TableView.ScrollToRow(NSIndexPath.FromRowSection(row, 0), UITableViewScrollPosition.Bottom, false);
        }

        #endregion

        #region 文本输入框的事件

        private UITextView _txtEntry;

        private void CreateInputTextView()
        {
            _txtEntry = new UITextView(new CGRect(5, 5, _chatToolBar.Bounds.Width - 80, 30)) {
                ContentSize = new CGSize(_chatToolBar.Bounds.Width - 80, 30),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                ScrollEnabled = true,
                ScrollIndicatorInsets = new UIEdgeInsets(5, 0, 4, -2),

                ClearsContextBeforeDrawing = false,
                Font = UIFont.SystemFontOfSize(UiStyleSetting.FontTitleSize),
                DataDetectorTypes = UIDataDetectorType.All,
                BackgroundColor = UIColor.Clear,
                ReturnKeyType = UIReturnKeyType.Send,
                EnablesReturnKeyAutomatically = true
            };
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0)) {
                _txtEntry.TextContainerInset = new UIEdgeInsets(5, 5, 5, 5);
            } else {
                _txtEntry.ContentInset = new UIEdgeInsets(5, 5, 5, 5);
            }
            _txtEntry.Layer.BorderWidth = 1;
            _txtEntry.Layer.BorderColor = UIColor.LightGray.CGColor;
            _txtEntry.Layer.CornerRadius = 6;
            _txtEntry.Layer.MasksToBounds = true;

            previousContentHeight = _txtEntry.ContentSize.Height;
            _chatToolBar.AddSubview(_txtEntry);

            _txtEntry.Changed += TextEntryChange;
            _txtEntry.ShouldChangeText = TextEntryShouldChangeText;
            _txtEntry.ShouldBeginEditing = TextEntryShouldBeginEditing;
        }

        private bool TextEntryShouldChangeText(UITextView View, NSRange rang, string text)
        {
            if (text == Environment.NewLine) {
                SendTextMessage();
                return false;
            }
                
            _txtEntry.ContentInset = new UIEdgeInsets(0, 0, 3, 0);
            return true;
        }

        private bool TextEntryShouldBeginEditing(UITextView view)
        {
            _isMoreViewShowing = false;
            _isEmojiShowing = false;
            _moreButton.SetBackgroundImage(UIImage.FromFile("chatBar_more.png"), UIControlState.Normal);
            _moreButton.SetBackgroundImage(UIImage.FromFile("chatBar_moreSelected.png"), UIControlState.Selected);
            AdjustViewHeight(_chatToolBar.Frame.Height);
            return true;
        }

        private void TextEntryChange(object sender, EventArgs e)
        {
            var contentHeight = _txtEntry.ContentSize.Height - 6;
            if (_txtEntry.HasText
                && !string.IsNullOrWhiteSpace(_txtEntry.Text)
                && contentHeight != previousContentHeight) {

                if (contentHeight <= _chatTextMaxHeight) {
                    AdjustViewHeight(contentHeight + 10);
                    if (previousContentHeight > _chatTextMaxHeight)
                        _txtEntry.ScrollEnabled = false;
                } else if (previousContentHeight <= _chatTextMaxHeight) {
                    _txtEntry.ScrollEnabled = true;
                    if (previousContentHeight < _chatTextMaxHeight) {
                        AdjustViewHeight(_chatTextMaxHeight + 10);
                    }
                }

            } else {
                if (previousContentHeight > 30) {
                    AdjustViewHeight(_chatToolBarHeight);
                    if (previousContentHeight > _chatTextMaxHeight)
                        _txtEntry.ScrollEnabled = false;
                }
            }
            previousContentHeight = contentHeight;
        }

        #endregion

        #region emoji表情选择部分

        private bool _isEmojiShowing = false;
        private UIButton _emojiViewButton;
        private UIImageView _emojiView;
        private List<UIButton> _emojiButtons;
        private nfloat _emojiViewHeight = 0;
        UIScrollView _emojiScrollView;
        UIPageControl _emojiPageControl;
        UIButton _emojiSendButton;

        private void CreateEmojiView()
        {
            var emojiImageSize = (this.View.Frame.Width - 10) / 8 - 10;
            _emojiViewHeight = (emojiImageSize + 10) * 4 + 10;

            #region 打开表情视图的按钮
            _emojiViewButton = _chatToolBarBuilder.CreateButton(string.Empty, "chatBar_face.png", "chatBar_faceSelected.png");
            _emojiViewButton.Frame = new CGRect(_chatToolBar.Bounds.Width - 70, 5, 30, 30);
            _emojiViewButton.AddTarget(EmojiButtonClick, UIControlEvent.TouchUpInside);
            #endregion

            #region 表情视图
            _emojiView = _viewBuilder.CreateToolBarByImageView(withBorder: false);
            _emojiView.Frame = new CGRect(0, View.Bounds.Height, View.Bounds.Width, _emojiViewHeight);
            _emojiView.BackgroundColor = UiStyleSetting.ViewControllerColor;
            var emojiViewBuilder = new UIViewBuilder(_emojiView);
            #endregion

            #region 滚动视图
            _emojiScrollView = new UIScrollView(new CGRect(_emojiView.Bounds.X, _emojiView.Bounds.Height,
                _emojiView.Bounds.Width, _emojiView.Bounds.Height - (emojiImageSize + 10)));
            _emojiScrollView.PagingEnabled = true;
            _emojiView.AddSubview(_emojiScrollView);
            _emojiScrollView.BackgroundColor = UIColor.Clear;
            _emojiScrollView.DecelerationEnded += (sender, e) => {
                var index = (int)(_emojiScrollView.ContentOffset.X / _emojiScrollView.Frame.Width);
                _emojiPageControl.CurrentPage = index;
            };
            var emojiButtonBuilder = new UIViewBuilder(_emojiScrollView);
            #endregion

            #region 分页控件
            _emojiPageControl = new UIPageControl();
            _emojiView.AddSubview(_emojiPageControl);
            #endregion

            #region 每个具体的表情按钮
            _emojiButtons = new List<UIButton>(EmojiUtil.Count);
            for (var i = 0; i < EmojiUtil.Count; i++) {
                var ee = emojiButtonBuilder.CreateButton(EmojiUtil.Unicodes[i]);
                ee.Tag = i;
                _emojiButtons.Add(ee);

                ee.AddTarget(delegate(object sender, EventArgs e) {
                    var btn = sender as UIButton;
                    if (btn == null)
                        return;

                    if (btn.Tag < 0 || btn.Tag >= EmojiUtil.Count)
                        return;

                    var emojiChar = EmojiUtil.Chars[btn.Tag];
                    this._txtEntry.Text = this._txtEntry.Text + emojiChar;

                    //手动调用下TextChange事件，IOS不会自动触发
                    TextEntryChange(null, null);
                }, UIControlEvent.TouchUpInside);
            }
            #endregion

            #region 发送按钮
            _emojiSendButton = emojiViewBuilder.CreateButton("发送");
            _emojiSendButton.AddTarget((sender, e) => {
                SendTextMessage();
            }, UIControlEvent.TouchUpInside);
            #endregion
            this.Add(_emojiView);
        }

        private void SetEmojiViewButtonFrame()
        {
            var emojiImageSize = (this.View.Frame.Width - 10) / 8 - 10;

            _emojiScrollView.Frame = new CGRect(_emojiView.Bounds.X, _emojiView.Bounds.Y,
                _emojiView.Bounds.Width, _emojiView.Bounds.Height - (emojiImageSize + 10));
            var pageCount = Math.Ceiling(EmojiUtil.Count / 24F);
            _emojiScrollView.ContentSize = new CGSize(_emojiView.Bounds.Width * (float)pageCount, _emojiView.Bounds.Height - (emojiImageSize + 10));

            _emojiPageControl.Frame = new CGRect(_emojiView.Bounds.X, _emojiView.Bounds.Y + _emojiScrollView.Frame.Height, _emojiView.Bounds.Width, 1);
            _emojiPageControl.Pages = (int)pageCount;

            _emojiSendButton.Frame = new CGRect(_emojiScrollView.Frame.Width - UiStyleSetting.NormalButtonWidth - 10,
                _emojiView.Bounds.Y + _emojiScrollView.Frame.Height + 10, 
                UiStyleSetting.NormalButtonWidth, emojiImageSize);


            for (var i = 0; i < EmojiUtil.Count; i++) {
                var pageIndex = i / 24;
                var rowNo = (i / 8) - (pageIndex * 3);
                var colNo = i % 8;
                var ee = _emojiButtons[i];
                ee.Frame = new CGRect(10 + pageIndex * View.Bounds.Width + colNo * (10 + emojiImageSize),
                    10 + rowNo * (10 + emojiImageSize),
                    emojiImageSize, emojiImageSize);
            }
        }

        private void EmojiButtonClick(object sender, EventArgs e)
        {
            _isEmojiShowing = !_isEmojiShowing;
            _isMoreViewShowing = false;

            AdjustViewHeight(_chatToolBar.Frame.Height);
            if (!_isEmojiShowing)
                _txtEntry.BecomeFirstResponder();
            else
                View.EndEditing(true);
        }

        #endregion

        #region 图片、拍照、视频、地理位置等部分

        private UIImageView _moreView;
        private nfloat _moreViewButtonWidth = 40;
        private nfloat _moreViewHeight = 80;
        private UIButton _moreButton, _cameraButton, _photoButton;
        private bool _isMoreViewShowing = false;

        private void CreateMoreView()
        {
            _moreButton = _chatToolBarBuilder.CreateButton(string.Empty, "chatBar_more.png", "chatBar_moreSelected.png");
            _moreButton.Frame = new CGRect(_chatToolBar.Bounds.Width - 35, 5, 30, 30);
            _moreButton.AddTarget(MoreButtonClick, UIControlEvent.TouchUpInside);

            _moreViewButtonWidth = (View.Bounds.Width - 20) / 4 - 20;
            _moreViewHeight = _moreViewButtonWidth + 40;

            _moreView = _viewBuilder.CreateToolBarByImageView(false);
            _moreView.Frame = new CGRect(0, View.Bounds.Height, View.Bounds.Width, _moreViewHeight);
            _moreView.BackgroundColor = UiStyleSetting.ViewControllerColor;
            var moreViewBuilder = new UIViewBuilder(_moreView);

            _cameraButton = moreViewBuilder.CreateButton("", "chatBar_colorMore_camera.png", "chatBar_colorMore_cameraSelected.png");
            _cameraButton.AddTarget(CameraButtonClick, UIControlEvent.TouchUpInside);

            _photoButton = moreViewBuilder.CreateButton("", "chatBar_colorMore_photo.png", "chatBar_colorMore_photoSelected.png");
            _photoButton.AddTarget(PhotoButtonClick, UIControlEvent.TouchUpInside);
        }

        private void SetMoreViewButtonFrame()
        {
            _cameraButton.Frame = new CGRect(20, 20, _moreViewButtonWidth, _moreViewButtonWidth);
            _photoButton.Frame = new CGRect(20 + _moreViewButtonWidth + 20, 20, _moreViewButtonWidth, _moreViewButtonWidth);
        }

        private void MoreButtonClick(object sender, EventArgs e)
        {
            _isMoreViewShowing = !_isMoreViewShowing;
            _isEmojiShowing = false;

            AdjustViewHeight(_chatToolBar.Frame.Height);
            if (!_isMoreViewShowing)
                _txtEntry.BecomeFirstResponder();
            else
                View.EndEditing(true);
        }

        private void CameraButtonClick(object sender, EventArgs e)
        {
            CameraUtil.TakePicture(this, SendPictureMessage);
        }

        private void PhotoButtonClick(object sender, EventArgs e)
        {
            CameraUtil.SelectPicture(this, SendPictureMessage);
        }

        #endregion

        #region 消息处理相关的代码

        private async void SendPictureMessage(UIImage image)
        {
            if (image == null)
                return;
    
            string base64Image = null;
            using (var t = new Toast()) {
                t.ProgressWaiting("正在压缩图片...");
                base64Image = await ImageUtil.ConvertImage2Base64StringAsync(image, 30).ConfigureAwait(true);
            }
           
            var msg = new ChatMessageViewModel {
                ChatMessageId = Guid.NewGuid().ToString(),
                FromId = ChatClient.CurrentUserContact.ContactId,
                FromName = ChatClient.CurrentUserContact.ContactName,
                FromResource = ChatClient.CurrentUserContact.ContactCode,
                ToId = ChatList.ChatListId,
                ToName = ChatList.ChatListName,
                MessageContent = base64Image,
                MessageType = ChatMessageType.Send,
                MessageContentType = ChatMessageContentType.Image,
                MessageSendStatus = ChatMessageSendStatus.NotSend,
                SendDateTime = DateTime.Now,
                MessageListType = ChatList.ListType
            };

            using (var t = new Toast()) {
                t.ProgressWaiting("正在发送图片...");
                await ChatClient.SendMessageAsync(msg).ConfigureAwait(true);
            }

            _txtEntry.Text = "";
            _isEmojiShowing = false;
            _isMoreViewShowing = false;
            AdjustViewHeight(_chatToolBarHeight);
            ScrollToBottom(true);

        }

        private async void SendTextMessage()
        {
            var msgBody = _txtEntry.Text;
            _txtEntry.Text = "";
            if (string.IsNullOrWhiteSpace(msgBody))
                return;

            var msg = new ChatMessageViewModel {
                ChatMessageId = Guid.NewGuid().ToString(),
                FromId = ChatClient.CurrentUserContact.ContactId,
                FromName = ChatClient.CurrentUserContact.ContactName,
                FromResource = ChatClient.CurrentUserContact.ContactCode,
                ToId = ChatList.ChatListId,
                ToName = ChatList.ChatListName,
                MessageContent = msgBody,
                MessageType = ChatMessageType.Send,
                MessageContentType = ChatMessageContentType.Text,
                MessageSendStatus = ChatMessageSendStatus.NotSend,
                SendDateTime = DateTime.Now,
                MessageListType = ChatList.ListType
            };

            await ChatClient.SendMessageAsync(msg)
                .ConfigureAwait(true);
           
            _isEmojiShowing = false;
            _isMoreViewShowing = false;
            AdjustViewHeight(_chatToolBarHeight);
            ScrollToBottom(true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            MessagesDataRepository.ReadAll(this.ChatList);

            ReloadData();

            ScrollToBottom(true);

            MessagesDataRepository.SubscribeChatMessageChange(MessageObserver);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            MessagesDataRepository.UnSubscribeChatMessageChange(MessageObserver);
        }

        //每隔一个小时显示消息发送时间
        int _timeSpanSecond = 1 * 60 * 60;

        private void ReloadData()
        {
            List<ChatMessageViewModel> messages = null;
            if (ChatList.ListType == ChatListType.Group)
                messages = MessagesDataRepository.GetRecentMessagesByRoomId(ChatList.ChatListId, _currentPageIndex, _pageSize);
            else
                messages = MessagesDataRepository.GetRecentMessagesByContactId(ChatList.ChatListId, _currentPageIndex, _pageSize);

            messages = messages.OrderBy((m) => m.SendDateTime).ToList();
            _discussionViewController.Root[0].Clear();

            ChatMessageViewModel lastMessage = null;
            messages.ForEach((msg) => {
                if (lastMessage == null || (msg.SendDateTime - lastMessage.SendDateTime).TotalSeconds >= _timeSpanSecond) {
                    _discussionViewController.Root[0].Add(new ChatBubbleDateTimeElement(DateTimeUtil.GetDateTimeString(msg.SendDateTime, true)));
                }
                _discussionViewController.Root[0].Add(new ChatBubbleMessageElement(msg));
                lastMessage = msg;
            });
        }

        private void ReloadMessageRow(ChatMessageViewModel msg)
        {
            var exitsElement = _discussionViewController.Root[0]
                .Elements.OfType<ChatBubbleMessageElement>()
                .FirstOrDefault(e => e.ChatMessage.ChatMessageId == msg.ChatMessageId);
            if (exitsElement != null) {
                exitsElement.ChatMessage = msg;
                _discussionViewController.TableView.ReloadRows(new NSIndexPath[]{ exitsElement.IndexPath }, UITableViewRowAnimation.None);

            } else {
                var lastElement = _discussionViewController.Root[0]
                    .Elements.OfType<ChatBubbleMessageElement>().LastOrDefault();
                if (lastElement == null || (msg.SendDateTime - lastElement.ChatMessage.SendDateTime).TotalSeconds >= _timeSpanSecond) {
                    _discussionViewController.Root[0].Add(new ChatBubbleDateTimeElement(DateTimeUtil.GetDateTimeString(msg.SendDateTime, true)));
                }
                _discussionViewController.Root[0].Add(new ChatBubbleMessageElement(msg));
                ScrollToBottom(true);
            }
        }

        private void MessageObserver(ChatMessageViewModel msg, SqlDataChangeType t)
        {
            if (msg == null && t == SqlDataChangeType.Remove)
            {
                ReloadData();
            }

            if (msg == null)
                return;

            if (msg.MessageType == ChatMessageType.Receive && msg.MessageReceiveStatus == ChatMessageReceiveStatus.Received) {
                msg.MessageReceiveStatus = ChatMessageReceiveStatus.Readed;
                MessagesDataRepository.AddOrUpdate(msg);
                return; // 消息更新后，再刷新界面
            }

            if (
                (ChatList.ListType == ChatListType.Group && msg.FromId.ToLower() == ChatList.ChatListId.ToLower())
                ||
                (ChatList.ListType == ChatListType.Group && msg.ToId.ToLower() == ChatList.ChatListId.ToLower())) {
                InvokeOnMainThread(() => {
                    ReloadMessageRow(msg);
                });
            }

            if (ChatList.ListType == ChatListType.Private) {
                if (
                    (msg.ToCode.ToLower() == ChatList.ChatListCode.ToLower() && msg.FromCode.ToLower() == ChatClient.CurrentUserContact.ContactCode.ToLower())
                    || (msg.FromCode.ToLower() == ChatList.ChatListCode.ToLower() && msg.ToCode.ToLower() == ChatClient.CurrentUserContact.ContactCode.ToLower())) {
                    InvokeOnMainThread(() => {
                        ReloadMessageRow(msg);
                    });
                }
            }
        }

        #endregion

        #region 控件高度和键盘相关处理代码

        void AdjustViewHeight(nfloat newChatEntryBarHeight)
        {
            if (_isMoreViewShowing) {
                _moreButton.SetBackgroundImage(UIImage.FromFile("chatBar_keyboard.png"), UIControlState.Normal);
                _moreButton.SetBackgroundImage(UIImage.FromFile("chatBar_keyboardSelected.png"), UIControlState.Selected);
            } else {
                _moreButton.SetBackgroundImage(UIImage.FromFile("chatBar_more.png"), UIControlState.Normal);
                _moreButton.SetBackgroundImage(UIImage.FromFile("chatBar_moreSelected.png"), UIControlState.Selected);
            }

            if (newChatEntryBarHeight < _chatToolBarHeight)
                newChatEntryBarHeight = _chatToolBarHeight;
                
            var chatFrame = _discussionViewController.View.Frame;
            chatFrame.Height = View.Bounds.Height - newChatEntryBarHeight;
            if (_isMoreViewShowing)
                chatFrame.Height = chatFrame.Height - _moreViewHeight;
            if (_isEmojiShowing) {
                chatFrame.Height = chatFrame.Height - _emojiViewHeight;
            }

            UIView.Animate(.3, () => {
                _discussionHost.Frame = chatFrame;
                
                _chatToolBar.Frame = new CGRect(_chatToolBar.Frame.X, chatFrame.Height, chatFrame.Width, newChatEntryBarHeight);
                _txtEntry.Frame = new CGRect(5, 5, _chatToolBar.Bounds.Width - 80, newChatEntryBarHeight - 10);
                _moreButton.Frame = new CGRect(_chatToolBar.Bounds.Width - 35, 5, 30, 30);
                _emojiViewButton.Frame = new CGRect(_chatToolBar.Bounds.Width - 70, 5, 30, 30);

                if (_isMoreViewShowing) {
                    _moreView.Frame = new CGRect(View.Bounds.X, chatFrame.Height + _chatToolBar.Frame.Height,
                        View.Bounds.Width, _moreViewHeight);
                    _emojiView.Frame = new CGRect(View.Bounds.X, View.Bounds.Height,
                        View.Bounds.Width, _moreViewHeight);

                    SetMoreViewButtonFrame();
                    SetEmojiViewButtonFrame();
                } else if (_isEmojiShowing) {
                    _emojiView.Frame = new CGRect(View.Bounds.X, chatFrame.Height + _chatToolBar.Frame.Height,
                        View.Bounds.Width, _emojiViewHeight);
                    _moreView.Frame = new CGRect(View.Bounds.X, View.Bounds.Height,
                        View.Bounds.Width, _moreViewHeight);
                    SetEmojiViewButtonFrame();
                    SetMoreViewButtonFrame();
                } else {
                    _moreView.Frame = new CGRect(View.Bounds.X, View.Bounds.Height,
                        View.Bounds.Width, _moreViewHeight);
                    _emojiView.Frame = new CGRect(View.Bounds.X, View.Bounds.Height,
                        View.Bounds.Width, _moreViewHeight);
                    SetMoreViewButtonFrame();
                    SetEmojiViewButtonFrame();
                }
            });
        }

        //
        // When the keyboard appears, animate the new position for the entry
        // and scroll the chat to the bottom
        //
        void PlaceKeyboard(object sender, UIKeyboardEventArgs args)
        {
            UIView.BeginAnimations("");
            {
                UIView.SetAnimationCurve(args.AnimationCurve);
                UIView.SetAnimationDuration(args.AnimationDuration);
                var viewFrame = View.Frame;
                var endRelative = View.ConvertRectFromView(args.FrameEnd, null);
                viewFrame.Height = endRelative.Y;
                View.Frame = viewFrame;
            }
            UIView.CommitAnimations();

            ScrollToBottom(true);
        }

        #endregion
    }
}