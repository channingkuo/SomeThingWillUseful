#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-07
说明 : 联系人的列表ViewController
****************/
#endregion

using System;
using System.Collections.Generic;
using CoreGraphics;
using System.Linq;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Views;
using RekTec.MyProfile.Services;

namespace RekTec.Contacts.Views
{
	/// <summary>
	/// 联系人的列表ViewController
	/// </summary>
	public class ContactsViewController : BaseViewController
	{
		private UITableView _tableView;
		private UISearchBar _searchBar;
		private bool _isSearching;
		private bool _isLoaded;

		/// <summary>
		/// 页面每次加载的时候执行
		/// </summary>
		public override void ViewDidLoad ()
		{
			NavigationItem.Title = "通讯录";
			this.View.BackgroundColor = UiStyleSetting.ViewControllerColor;

			base.ViewDidLoad ();
			CreateUiElement ();

			BindUiElementEvent ();
		}

		/// <summary>
		/// 页面将要出现时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			NavigationController.NavigationBarHidden = false;
			/*if (!_isLoaded) {
				LoadUiData ();
				_isLoaded = true;
			}*/
			LoadUiData ();
			_tableView.ReloadData ();
			ContactsDataRepository.SubscribeContactChange (ChatContactDataChange);
		}

		/// <summary>
		/// 页面将要消失的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			ContactsDataRepository.UnSubscribeContactChange (ChatContactDataChange);
		}

		/// <summary>
		/// 在页面上添加控件
		/// </summary>
		private void CreateUiElement ()
		{
			var builder = new UIViewBuilder (this.View);

			_tableView = builder.CreateTableView (new CGRect (View.Bounds.X, View.Bounds.Y,
				View.Frame.Width, View.Frame.Height - UiStyleSetting.StatusBarHeight - UiStyleSetting.NavigationBarHeight - UiStyleSetting.TabBarHeight));

			_searchBar = builder.CreateSearchBar (new CGRect (_tableView.Bounds.X, _tableView.Bounds.Y, _tableView.Bounds.Width, UiStyleSetting.SearchBarHeight));
			_tableView.TableHeaderView = _searchBar;
			//修改索引的背景颜色
			_tableView.SectionIndexBackgroundColor = UIColor.Clear;

		}

		/// <summary>
		/// 设置控件相关事件
		/// </summary>
		private void BindUiElementEvent ()
		{
			_searchBar.OnEditingStarted += (sender, e) => {
				_searchBar.ShowsCancelButton = true;
				_tableView.AllowsSelection = false;
				_tableView.ScrollEnabled = false;
				_isSearching = true;
			};
			_searchBar.CancelButtonClicked += (sender, e) => {
				_searchBar.ShowsCancelButton = false;
				_tableView.AllowsSelection = true;
				_tableView.ScrollEnabled = true;
				_searchBar.ResignFirstResponder ();
				_isSearching = false;
				_tableView.ReloadData ();
			};
			_searchBar.TextChanged += (sender, e) => {
				_searchBar.ShowsCancelButton = true;
				_tableView.AllowsSelection = true;
				_tableView.ScrollEnabled = true;
				_tableView.ReloadData ();
			};
			_searchBar.SearchButtonClicked += (sender, e) => {
				_tableView.AllowsSelection = true;
				_tableView.ScrollEnabled = true;
				_searchBar.ResignFirstResponder ();
				_tableView.ReloadData ();
			};
		}

		/// <summary>
		/// 页面每次将要出现的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ErrorHandlerUtil.Subscribe (HandleError);
			NavigationItem.SetRightBarButtonItem (null, false);

		}

		private void ChatContactDataChange (ContactViewModel c)
		{
			InvokeOnMainThread (LoadUiData);
		}

		/// <summary>
		/// 处理错误方法
		/// </summary>
		/// <returns><c>true</c>, if error was handled, <c>false</c> otherwise.</returns>
		/// <param name="msg">Message.</param>
		private bool HandleError (string msg)
		{
			AlertUtil.Error (msg);
			return true;
		}

		/// <summary>
		///加载数据
		/// </summary>
		private void LoadUiData ()
		{
			_tableView.Source = new SourceContact (this);
			_tableView.ReloadData ();
		}

		private class SourceContact : UITableViewSource
		{
			private ContactsViewController _controller;
			private List<ContactViewModel> _resultContacts;
			private string [] _sectionTitles;

			public SourceContact (ContactsViewController controller)
			{
				_controller = controller;
			}

			/// <Docs>Table view displaying the sections.</Docs>
			/// <returns>Number of sections required to display the data. The default is 1 (a table must have at least one section).</returns>
			/// <para>Declared in [UITableViewDataSource]</para>
			/// <summary>
			/// 当前tab的节的个数
			/// </summary>
			/// <param name="tableView">Table view.</param>
			public override nint NumberOfSections (UITableView tableView)
			{
				if (_controller._isSearching && !string.IsNullOrWhiteSpace (_controller._searchBar.Text))
					_resultContacts = ContactsDataRepository.SearchContactsLikeName (_controller._searchBar.Text)
						.Where (c => c.XrmUserId.ToLower () != AuthenticationService.CurrentUserInfo.SystemUserId.ToLower ()
					&& !string.IsNullOrWhiteSpace (c.ContactNamePinYinFirst) && c.ContactNamePinYinFirst.Length > 0 && !c.IsDisabled)
						.ToList ();
				else
					_resultContacts = ContactsDataRepository.GetAllContacts ()
						.Where (c => c.XrmUserId.ToLower () != AuthenticationService.CurrentUserInfo.SystemUserId.ToLower ()
					&& !string.IsNullOrWhiteSpace (c.ContactNamePinYinFirst) && c.ContactNamePinYinFirst.Length > 0 && !c.IsDisabled)
						.ToList ();

				_sectionTitles = _resultContacts
					.OrderBy (c => c.ContactNamePinYinFirst [0].ToString ())
					.Select (c => c.ContactNamePinYinFirst [0].ToString ())
					.Distinct ()
					.ToArray ();

				return _sectionTitles.Length;
			}

			/// <Docs>Table view containing the row.</Docs>
			/// <summary>
			/// 每行选中的点击事件
			/// </summary>
			/// <param name="tableView">Table view.</param>
			/// <param name="indexPath">Index path.</param>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var sectionTitle = _sectionTitles [indexPath.Section];
				var contact = _resultContacts
					.Where (c => c.ContactNamePinYinFirst [0].ToString () == sectionTitle)
					.ToList () [indexPath.Row];

				this._controller.NavigationController.PushViewController (
					new ContactDetailViewController (new RootElement ("详细资料") { UnevenRows = true }, contact)
					, true);

				tableView.DeselectRow (indexPath, false);
			}

			/// <Docs>Table view.</Docs>
			/// <summary>
			/// 每行的高度
			/// </summary>
			/// <returns>The height for row.</returns>
			/// <param name="tableView">Table view.</param>
			/// <param name="indexPath">Index path.</param>
			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return 60;
			}

			/// <Docs>Table view containing the section.</Docs>
			/// <summary>
			/// 每个节的标题
			/// </summary>
			/// <see langword="null"></see>
			/// <returns>The for header.</returns>
			/// <param name="tableView">Table view.</param>
			/// <param name="section">Section.</param>
			public override string TitleForHeader (UITableView tableView, nint section)
			{
				return _sectionTitles [(int)section];
			}

			/// <Docs>Table view that is displaying the index.</Docs>
			/// <returns>Array of titles, for example to display an alphabetized list return an array of strings from "A" to "Z".</returns>
			/// <para>The index list appears along the right edge of a table view.</para>
			/// <see cref="F:MonoTouch.UIKit.UITableViewStyle.Plain"></see>
			/// <para>Declared in [UITableViewDataSource]</para>
			/// <see cref="P:MonoTouch.UIKit.UITableView.IndexSearch"></see>
			/// <see cref="P:MonoTouch.UIKit.UITableView.IndexSearch"></see>
			/// <summary>
			/// 每个section的标题
			/// </summary>
			/// <param name="tableView">Table view.</param>
			public override string [] SectionIndexTitles (UITableView tableView)
			{
				return _sectionTitles;
			}

			/// <Docs>Table view displaying the rows.</Docs>
			/// <summary>
			/// 当前tab的索引
			/// </summary>
			/// <returns>The in section.</returns>
			/// <param name="tableView">Table view.</param>
			/// <param name="section">Section.</param>
			public override nint RowsInSection (UITableView tableView, nint section)
			{
				var sectionTitle = _sectionTitles [(int)section];

				return _resultContacts.Count (c => c.ContactNamePinYinFirst [0].ToString () == sectionTitle);

			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var sectionTitle = _sectionTitles [indexPath.Section];
				var contact = _resultContacts
					.Where (c => c.ContactNamePinYinFirst [0].ToString () == sectionTitle)
					.ToList () [indexPath.Row];

				var cellIdentifier = contact.ContactId;
				var cell = tableView.DequeueReusableCell (cellIdentifier);

				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);
					cell.Frame = _controller.View.Frame;
					cell.TextLabel.Text = contact.ContactName;
					cell.DetailTextLabel.Text = contact.Department + '-' + contact.Position;
					cell.DetailTextLabel.TextColor = UIColor.LightGray;
					cell.ImageView.Image = contact.GetAvatarImage ().Scale (new CGSize (30, 30));

					if (!string.IsNullOrWhiteSpace (contact.Phone)) {
						UIButton phoneBtn = new UIButton (new CGRect (_controller.View.Frame.Width - 45, 15, 25, 25));
						phoneBtn.SetBackgroundImage (UIImage.FromFile ("phone_contact.png"), UIControlState.Normal);
						phoneBtn.TouchUpInside += (sender, e) => {
							var urlToSend = new NSUrl ("tel:" + contact.Phone); // phonenum is in the format 1231231234

							if (UIApplication.SharedApplication.CanOpenUrl (urlToSend)) {
								UIApplication.SharedApplication.OpenUrl (urlToSend);
							} else {
								AlertUtil.Error ("not support in simulator!");
							}
						};
						cell.Add (phoneBtn);
					}
				}

				return cell;
			}
		}
	}
}

