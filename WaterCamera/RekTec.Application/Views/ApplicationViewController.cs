#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-09
说明 : 应用页面
****************/
#endregion
using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;
using RekTec.Application.Services;
using RekTec.Application.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Views;
using RekTec.Application;
using RekTec.MyProfile.Services;

namespace RekTec.Application.Views
{
	/// <summary>
	/// 应用页面
	/// </summary>
	public class ApplicationViewController : BaseViewController
	{
		UITableView _tableView;
		UIScrollView _scrollView;
		UIViewBuilder _builder;
		List<UIButton> _menuButtons;
		UIViewBuilder _metroMenubuilder;

		/// <summary>
		/// 页面加载的时候
		/// </summary>
		public override void ViewDidLoad ()
		{
			_builder = new UIViewBuilder (View);
			View.BackgroundColor = UIColor.White;

			//设置当前页面的标题
			NavigationItem.Title = "应用";
			RenderMenu ();
		}

		public override async void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//隐藏导航栏右侧的按钮
			if (ParentViewController != null && ParentViewController.NavigationItem != null) {
				ParentViewController.NavigationItem.SetRightBarButtonItem (null, false);
			}

			await MenuService.LoadMenuBagesToSqlLite ();

			RenderMenu ();
		}

		/// <summary>
		/// 生成功能列表的菜单结构，如果类型是1，则是列表菜单；如果是2，则为9宫格的菜单
		/// </summary>
		private void RenderMenu ()
		{
			if (GlobalAppSetting.MenuStyle == "1") {
				if (_menuButtons != null) {
					_menuButtons.ForEach (btn => btn.RemoveFromSuperview ());
					_menuButtons = null;
				}

				if (_tableView == null) {
					_tableView = _builder.CreateTableView (new CGRect (View.Frame.X,
						View.Frame.Y,
						View.Frame.Width,
						View.Frame.Height - UiStyleSetting.StatusBarHeight - UiStyleSetting.NavigationBarHeight - UiStyleSetting.TabBarHeight));
				}
				_tableView.Source = new Source (this);
				_tableView.ReloadData ();

				AuthenticationService.AddLogoutAction (("ApplicationViewController_TableMenu"), () => {
					InvokeOnMainThread (() => {
						_tableView.Source = null;
						_tableView.ReloadData ();
					});
				});


			} else if (GlobalAppSetting.MenuStyle == "2") {
				if (_tableView != null) {
					_tableView.RemoveFromSuperview ();
					_tableView = null;
				}


				if (_menuButtons == null) {
					_menuButtons = new List<UIButton> ();
				}

				RenderMetroMenu ();

				AuthenticationService.AddLogoutAction (("ApplicationViewController_MetroMenu"), () => {
					InvokeOnMainThread (() => {
						if (_menuButtons != null) {
							_menuButtons.ForEach (btn => btn.RemoveFromSuperview ());
							_menuButtons = null;
						}
					});
				});
			} else {
				if (_tableView == null) {
					_tableView = _builder.CreateTableView (View.Bounds);
				}
				_tableView.Source = new Source (this);
				_tableView.ReloadData ();
			}
		}

		#region Metro Menu

		private int _metroMenuColCount = 3;
		private nfloat _metroMenuPadding = 25;
		private nfloat _metroMenuIconWidth = 20;

		/// <summary>
		/// 实现类似微信钱包中的菜单样式
		/// </summary>
		private void RenderMetroMenu ()
		{
			//iPad一行显示5个菜单
			if (View.Bounds.Width >= 768) {
				//_metroMenuColCount = 5;
				_metroMenuIconWidth = 25;
				_metroMenuPadding = 45;
			}

			if (_scrollView != null) {
				_scrollView.RemoveFromSuperview ();
				_scrollView = null;
			}
			_scrollView = new UIScrollView (View.Bounds);
			View.AddSubview (_scrollView);
			_metroMenubuilder = new UIViewBuilder (_scrollView);
			var menuButtonWidth = UiStyleSetting.ScreenWidth / _metroMenuColCount;
			var col = 0;
			var row = 0;
			nfloat menuButtonHeight = 0.0F;
			var pMenus = MenuDataRepository.GetParentMenu ();
			foreach (var pMenu in pMenus) {
				var cMenus = MenuDataRepository
					.GetChildMenuByParentMenuId (pMenu.SystemMenuId);

				foreach (var cMenu in cMenus) {
					menuButtonHeight = GetMetroMenuHeight (cMenu);
					var button = CreateMetroMenuButton (cMenu, menuButtonWidth);
					button.TouchUpInside += (sender, e) => {
						MenuClick (cMenu.MenuUrl);
					};
					button.Frame = new CGRect (col * menuButtonWidth,
						row * menuButtonHeight,
						menuButtonWidth, menuButtonHeight);
					AddBottomBorder (button, menuButtonWidth, menuButtonHeight);
					if (col != _metroMenuColCount) {
						AddRightBorder (button, menuButtonWidth, menuButtonHeight);
					}
					_menuButtons.Add (button);

					col++;
					if (col == _metroMenuColCount) {
						col = 0;
						row++;
					}
				}
			}

			_scrollView.ContentSize = new CGSize (View.Bounds.Width, (row + 1) * menuButtonHeight);
		}

		private nfloat GetMetroMenuHeight (ChildMenuViewModel menu)
		{
			var iconWidth = _metroMenuIconWidth;
			var labelFont = UIFont.SystemFontOfSize ((nfloat)UiStyleSetting.FontDetailSize);
			var stringSize = menu.MenuName.StringSize (labelFont);
			return _metroMenuPadding * 2 + stringSize.Height + iconWidth + 10;
		}

		private UIButton CreateMetroMenuButton (ChildMenuViewModel menu, nfloat menuButtonWidth)
		{
			var iconWidth = _metroMenuIconWidth;
			var iconTop = _metroMenuPadding;

			var labelFont = UIFont.SystemFontOfSize ((nfloat)UiStyleSetting.FontDetailSize);
			var stringSize = menu.MenuName.StringSize (labelFont);
			var menuButtonHeight = GetMetroMenuHeight (menu);

			var button = _metroMenubuilder.CreateButton (string.Empty);
			button.SetTitleColor (UIColor.Black, UIControlState.Normal);
			button.SetBackgroundImage (ImageUtil.CreateImageWithColor (UiStyleSetting.ViewControllerColor,
				new CGRect (0, 0, menuButtonWidth, menuButtonHeight)),
				UIControlState.Highlighted);

			var base64 = menu.MenuIcon;
			UIImage iconImage = string.IsNullOrWhiteSpace (base64) ? null : ImageUtil.ConvertBase64String2Image (base64);
			if (iconImage != null) {
				var iconView = new UIImageView (new CGRect (
								   (menuButtonWidth - iconWidth) / 2,
								   iconTop,
								   iconWidth,
								   iconWidth));
				iconView.Image = iconImage;
				button.AddSubview (iconView);
			}



			var buttonLabel = new UILabel (new CGRect (0,
								  iconTop + iconWidth + 10,
								  menuButtonWidth, stringSize.Height));
			buttonLabel.Text = menu.MenuName;
			buttonLabel.TextAlignment = UITextAlignment.Center;
			buttonLabel.Font = labelFont;
			button.AddSubview (buttonLabel);

			//添加Badge
			var badgeView = new BadgeView (string.Empty);
			badgeView.Hidden = menu.MenuBadge > 0 ? false : true;
			badgeView.Frame = new CGRect (
				(menuButtonWidth - iconWidth) / 2 + iconWidth - 6,
				iconTop - 6,
				12,
				12);
			button.AddSubview (badgeView);

			return button;
		}

		private void AddRightBorder (UIButton btn, nfloat menuButtonWidth, nfloat menuButtonHeight)
		{
			var border = new UIView (new CGRect (menuButtonWidth - 1, 0, 1, menuButtonHeight));
			border.BackgroundColor = UiStyleSetting.ViewControllerColor;
			btn.AddSubview (border);
		}

		private void AddBottomBorder (UIButton btn, nfloat menuButtonWidth, nfloat menuButtonHeight)
		{
			var border = new UIView (new CGRect (0, menuButtonHeight - 1, menuButtonWidth, 1));
			border.BackgroundColor = UiStyleSetting.ViewControllerColor;
			btn.AddSubview (border);
		}

		#endregion

		private void MenuClick (string menuUrl)
		{
			if (GlobalAppSetting.IsPMSUser) {
				if (menuUrl != "pms/home") {
					var alertAction = UIAlertController.Create ("PMS用户不能使用HMS功能", "", UIAlertControllerStyle.Alert);
					alertAction.AddAction (UIAlertAction.Create ("确认", UIAlertActionStyle.Default, alert => { }));
					PresentViewController (alertAction, true, null);
					return;
				} else {
					if (GlobalAppSetting.IsRunOrVestibuleManager) {
						//
					} else if (GlobalAppSetting.IsMultiHotel) {
						var alertAction = UIAlertController.Create ("没有此功能的权限", "", UIAlertControllerStyle.Alert);
						alertAction.AddAction (UIAlertAction.Create ("确认", UIAlertActionStyle.Default, alert => { }));
						PresentViewController (alertAction, true, null);
						return;
					}
				}
			}

			var wvc = new WebViewController () { _menuUrl = menuUrl };
			NavigationController.PushViewController (wvc, false);
		}

		/// <summary>
		/// tableview的 数据源
		/// </summary>
		public class Source : UITableViewSource
		{
			readonly List<ParentMenuViewModel> _parents;
			readonly ApplicationViewController _c;

			/// <summary>
			/// 构造函数
			/// </summary>
			public Source (ApplicationViewController c)
			{
				_c = c;
				_parents = MenuDataRepository.GetParentMenu ();
			}

			/// <summary>
			/// 设置每一行的高度
			/// </summary>
			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return UiStyleSetting.HeightTableViewRowDefault;
			}

			/// <summary>
			/// 设置有多少个节
			/// </summary>
			public override nint NumberOfSections (UITableView tableView)
			{
				return _parents.Count;
			}

			/// <summary>
			/// 设置没个节有多少行
			/// </summary>
			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return MenuDataRepository
					.GetChildMenuByParentMenuId (_parents [(int)section].SystemMenuId)
					.Count;
			}

			/// <summary>
			/// 设置每个节的标题 用于不同菜单组之间的间隔
			/// </summary>
			public override string TitleForHeader (UITableView tableView, nint section)
			{
				return " ";
			}

			/// <summary>
			/// 设置每个section的背景色
			/// </summary>
			public override UIView GetViewForHeader (UITableView tableView, nint section)
			{
				return new UIView (new CGRect (0, 0, tableView.Bounds.Width, UiStyleSetting.HeightTableViewHeaderDefault)) {
					BackgroundColor = UiStyleSetting.ViewControllerColor
				};
			}

			/// <summary>
			/// 设置每一行的内容
			/// </summary>
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var childs = MenuDataRepository.GetChildMenuByParentMenuId (_parents [indexPath.Section].SystemMenuId);

				var childMenu = childs [indexPath.Row];

				var cellId = childMenu.MenuCode + "cell";
				var base64 = childMenu.MenuIcon;
				UIImage menuImg = string.IsNullOrWhiteSpace (base64) ? null : ImageUtil.ConvertBase64String2Image (base64);
				if (menuImg != null)
					menuImg = menuImg.Scale (UiStyleSetting.SizeTableViewIconDefault);

				var cell = _c._builder.GetTableViewCell (tableView, cellId, childMenu.MenuName, menuImg);

				//添加Badge
				var badgeView = new BadgeView (string.Empty);
				badgeView.Hidden = childMenu.MenuBadge > 0 ? false : true;

				var left = ((NSString)childMenu.MenuName).StringSize (UIFont.SystemFontOfSize ((nfloat)UiStyleSetting.FontDetailSize)).Width + 70;
				badgeView.Frame = new CGRect (left, 12, 8, 8);

				cell.AddSubview (badgeView);

				return cell;
			}

			/// <summary>
			/// 每行选中时执行的事件
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var childs = MenuDataRepository.GetChildMenuByParentMenuId (_parents [indexPath.Section].SystemMenuId);
				_c.MenuClick (childs [indexPath.Row].MenuUrl);
				tableView.DeselectRow (indexPath, true);
			}
		}
	}
}

