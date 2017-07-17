#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-09
说明 : 首页
****************/
#endregion
using System;
using MonoTouch.Dialog;
using UIKit;
using RekTec.Corelib.Configuration;
using RekTec.Contacts.Views;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;
using RekTec.Messages.Views;
using RekTec.MyProfile.Services;
using RekTec.MyProfile.Views;

namespace RekTec.Application.Views
{
	/// <summary>
	/// 首页
	/// </summary>
	public class HomeViewController : UITabBarController
	{
		//对话页面
		private ChatListViewController _tabChatListViewController;
		//联系人页面
		private ContactsViewController _tabContactViewController;
		//应用页面
		private ApplicationViewController _tabApplicationViewController;
		//设置页面
		private MyProfileViewController _tabSettingViewController;

		/// <summary>
		/// 页面首次加载的时候
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.TabBar.Translucent = false;
			_tabChatListViewController = new ChatListViewController ();
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				_tabChatListViewController.TabBarItem = new UITabBarItem ("消息", UIImage.FromFile ("message_normal.png"), UIImage.FromFile ("message_selected.png"));
			else {
				_tabChatListViewController.TabBarItem = new UITabBarItem ("消息", UIImage.FromFile ("message_normal.png"), 0);
				_tabChatListViewController.TabBarItem.SelectedImage = UIImage.FromFile ("message_selected.png");
			}

			_tabContactViewController = new ContactsViewController ();
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				_tabContactViewController.TabBarItem = new UITabBarItem ("通讯录", UIImage.FromFile ("contact_normal.png"), UIImage.FromFile ("contact_selected.png"));
			else {
				_tabContactViewController.TabBarItem = new UITabBarItem ("通讯录", UIImage.FromFile ("contact_normal.png"), 0);
				_tabContactViewController.TabBarItem.SelectedImage = UIImage.FromFile ("contact_selected.png");
			}

			_tabApplicationViewController = new ApplicationViewController ();
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				_tabApplicationViewController.TabBarItem = new UITabBarItem ("应用", UIImage.FromFile ("application_normal.png"), UIImage.FromFile ("application_selected.png"));
			else {
				_tabApplicationViewController.TabBarItem = new UITabBarItem ("应用", UIImage.FromFile ("application_normal.png"), 0);
				_tabApplicationViewController.TabBarItem.SelectedImage = UIImage.FromFile ("application_selected.png");
			}

			_tabSettingViewController = new MyProfileViewController (new RootElement ("我") { UnevenRows = true });
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				_tabSettingViewController.TabBarItem = new UITabBarItem ("我", UIImage.FromFile ("setting_normal.png"), UIImage.FromFile ("setting_selected.png"));
			else {
				_tabSettingViewController.TabBarItem = new UITabBarItem ("我", UIImage.FromFile ("setting_normal.png"), 0);
				_tabSettingViewController.TabBarItem.SelectedImage = UIImage.FromFile ("setting_selected.png");
			}

			ViewControllers = new UIViewController [] {
				_tabChatListViewController, _tabContactViewController, _tabApplicationViewController, _tabSettingViewController
			};

			this.ViewControllerSelected += (sender, e) => {
				NavigationItem.Title = e.ViewController.NavigationItem.Title ?? string.Empty;

				NavigationItem.HidesBackButton = e.ViewController.NavigationItem.HidesBackButton;
				NavigationItem.RightBarButtonItem = e.ViewController.NavigationItem.RightBarButtonItem;
			};

			NavigationItem.BackBarButtonItem = new UIBarButtonItem (string.Empty, UIBarButtonItemStyle.Plain, null);

			_tabSettingViewController.OnLogoutButtonClick += () => {
				AuthenticationService.Logout ();
				SelectedIndex = 0;
			};
		}

		/// <summary>
		///设置默认选择的页面
		/// </summary>
		public override nint SelectedIndex {
			get {
				return base.SelectedIndex;
			}
			set {
				base.SelectedIndex = value;
				if (!AuthenticationService.IsLogOn () && _isAppeared) {
					NavigationController.PushViewController (new LoginViewController (), true);
				}
			}
		}

		/// <summary>
		/// 页面是否出现
		/// </summary>
		private bool _isAppeared;

		/// <summary>
		/// 页面将要出现的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (AuthenticationService.IsLogOn ()) {
				ReloadBadge (null);
			}

			SetNavBar ();
		}

		/// <summary>
		/// 设置导航栏
		/// </summary>
		private void SetNavBar ()
		{
			if (SelectedIndex == 0) {
				NavigationItem.Title = _tabChatListViewController.NavigationItem.Title;
				NavigationItem.TitleView = null;
				NavigationItem.HidesBackButton = _tabChatListViewController.NavigationItem.HidesBackButton;
				NavigationItem.RightBarButtonItem = _tabChatListViewController.NavigationItem.RightBarButtonItem;
				//ReloadBadge (null);
			} else if (SelectedIndex == 1) {
				NavigationItem.Title = _tabContactViewController.NavigationItem.Title;
				NavigationItem.TitleView = null;
				NavigationItem.HidesBackButton = _tabContactViewController.NavigationItem.HidesBackButton;
				NavigationItem.RightBarButtonItem = _tabContactViewController.NavigationItem.RightBarButtonItem;
				//ReloadBadge (null);
			} else if (SelectedIndex == 2) {
				NavigationItem.Title = _tabApplicationViewController.NavigationItem.Title;
				NavigationItem.TitleView = null;
				NavigationItem.HidesBackButton = _tabContactViewController.NavigationItem.HidesBackButton;
				NavigationItem.RightBarButtonItem = _tabContactViewController.NavigationItem.RightBarButtonItem;
				//ReloadBadge (null);
			} else if (SelectedIndex == 3) {
				NavigationItem.Title = _tabSettingViewController.NavigationItem.Title;
				NavigationItem.TitleView = null;
				NavigationItem.HidesBackButton = _tabSettingViewController.NavigationItem.HidesBackButton;
				NavigationItem.RightBarButtonItem = _tabSettingViewController.NavigationItem.RightBarButtonItem;
				//ReloadBadge (null);
			}
		}

		/// <summary>
		/// 页面将要出现的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear (bool animated)
		{
			_isAppeared = true;
			base.ViewDidAppear (animated);

			this.NavigationController.NavigationBarHidden = false;

			//首次打开时，引导用户设置服务器地址
			//			if (string.IsNullOrWhiteSpace (GlobalAppSetting.IsFirstOpen)) {
			//				NavigationController.PushViewController (new SeverAddressEditViewController (new RootElement ("服务器地址"),
			//					"服务器",
			//					GlobalAppSetting.XrmWebApiBaseUrl), true);
			//				return;
			//			}

			UiStyleSetting.RequestAlwaysAuthorizationOfLocationManager ();

			if (!AuthenticationService.IsLogOn ()) {
				NavigationController.PushViewController (new LoginViewController (), true);
			} else {
				ReloadBadge (null);
				MessagesDataRepository.SubscribeChatListChange (ReloadBadge);
			}
		}

		/// <summary>
		/// 页面已经出现的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidDisappear (bool animated)
		{
			_isAppeared = false;
			base.ViewDidDisappear (animated);
			if (AuthenticationService.IsLogOn ())
				MessagesDataRepository.UnSubscribeChatListChange (ReloadBadge);
		}

		/// <summary>
		///设置tab栏消息条数
		/// </summary>
		/// <param name="chatList">Chat list.</param>
		private void ReloadBadge (ChatListViewModel chatList)
		{
			//InvokeOnMainThread (() => {
			var count = 0;
			MessagesDataRepository.GetAllChatLists ()
					 .ForEach (l => count += l.UnReadCount);
			_tabChatListViewController.TabBarItem.BadgeValue = count <= 0 ? null : count.ToString ();
			//});
		}
	}
}

