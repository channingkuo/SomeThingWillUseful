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
using RekTec.Corelib.Views;

namespace RekTec.Chat.Views.Contact
{
	public class ContactsViewController : BaseViewController
	{
		private UITableView _tableView;
		private UISearchBar _searchBar;
		private bool _isSearching = false;
		private bool _isLoaded = false;

		/// <summary>
		/// 页面每次加载的时候执行
		/// </summary>
		public override void ViewDidLoad ()
		{
			NavigationItem.Title = "通讯录";
			this.View.BackgroundColor = UiStyleSetting.ViewControllerColor;

			base.ViewDidLoad ();
			CreateUIElement ();

			BindUIElementEvent ();
		}

		/// <summary>
		/// 页面将要出现时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			NavigationController.NavigationBarHidden = false;
			if (!_isLoaded) {
				LoadUIData ();
				_isLoaded = true;
			}

			ContactsDataRepository.SubscribeContactChange (ChatContactDataChange);
			ChatDataRepository.SubscribeChatRoomChange (ChatRoomDataChange);
		}

		/// <summary>
		/// 页面将要消失的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			ChatDataRepository.UnSubscribeChatRoomChange (ChatRoomDataChange);
            ContactsDataRepository.UnSubscribeContactChange(ChatContactDataChange);
		}

		/// <summary>
		/// 在页面上添加控件
		/// </summary>
		private void CreateUIElement ()
		{
			var builder = new UIViewBuilder (this.View);

			_tableView = builder.CreateTableView (new CGRect (View.Bounds.X, View.Bounds.Y,
				View.Frame.Width, View.Frame.Height - UiStyleSetting.StatusBarHeight - UiStyleSetting.NavigationBarHeight - UiStyleSetting.TabBarHeight));

			_searchBar = builder.CreateSearchBar (new CGRect (_tableView.Bounds.X, _tableView.Bounds.Y, _tableView.Bounds.Width, UiStyleSetting.SearchBarHeight));
			_tableView.TableHeaderView = _searchBar;
		}

		/// <summary>
		/// 设置控件相关事件
		/// </summary>
		private void BindUIElementEvent ()
		{
			_searchBar.OnEditingStarted += (object sender, EventArgs e) => {
				_searchBar.ShowsCancelButton = true;
				_tableView.AllowsSelection = false;
				_tableView.ScrollEnabled = false;
				_isSearching = true;
			};
			_searchBar.CancelButtonClicked += (object sender, EventArgs e) => {
				_searchBar.ShowsCancelButton = false;
				_tableView.AllowsSelection = true;
				_tableView.ScrollEnabled = true;
				_searchBar.ResignFirstResponder ();
				_isSearching = false;
				_tableView.ReloadData ();
			};

			_searchBar.SearchButtonClicked += (object sender, EventArgs e) => {
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

        private void ChatRoomDataChange(ChatRoomViewModel r, SqlDataChangeType t)
		{
			InvokeOnMainThread (() => {
				LoadUIData ();
			});
		}

		private void ChatContactDataChange (ContactViewModel c)
		{
			InvokeOnMainThread (() => {
				LoadUIData ();
			});
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
		private void LoadUIData ()
		{
			_tableView.Source = new SourceContact (this);
			_tableView.ReloadData ();
		}

		private class SourceContact: UITableViewSource
		{
			private ContactsViewController _controller;
			private List<ContactViewModel> _resultContacts;
			private string[] _sectionTitles;

			public SourceContact (ContactsViewController controller)
			{
				_controller = controller;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				if (!_controller._isSearching)
					_resultContacts = ContactsDataRepository.GetAllContacts ()
						.Where (c => c.ContactId != ContactClient.CurrentUserContact.ContactId
							&& !string.IsNullOrWhiteSpace (c.ContactNamePinYinFirst) && c.ContactNamePinYinFirst.Length > 0&&!c.IsDisabled)
						.ToList ();
				else
					_resultContacts = ContactsDataRepository.SearchContactsLikeName (_controller._searchBar.Text)
						.Where (c => c.ContactId != ContactClient.CurrentUserContact.ContactId
							&& !string.IsNullOrWhiteSpace (c.ContactNamePinYinFirst) && c.ContactNamePinYinFirst.Length > 0&&!c.IsDisabled)
						.ToList ();

				_sectionTitles = _resultContacts
					.OrderBy ((c) => c.ContactNamePinYinFirst [0].ToString ())
					.Select ((c) => c.ContactNamePinYinFirst [0].ToString ())
					.Distinct ()
					.ToArray ();

				return _sectionTitles.Length;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var sectionTitle = _sectionTitles [indexPath.Section];
				var contact = _resultContacts
					.Where ((c) => c.ContactNamePinYinFirst [0].ToString () == sectionTitle)
					.ToList () [indexPath.Row];

				this._controller.NavigationController.PushViewController (
					new ContactDetailViewController (new RootElement ("详细信息"){ UnevenRows = true }, contact)
					, true);

				tableView.DeselectRow (indexPath, false);
			}

			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return 50;
			}

			public override string TitleForHeader (UITableView tableView, nint section)
			{
				return _sectionTitles [section];
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
			public override string[] SectionIndexTitles (UITableView tableView)
			{
				return _sectionTitles;
			}

			/// <Docs>Table view displaying the rows.</Docs>
			/// <summary>
			/// 每个节包含行数
			/// </summary>
			/// <returns>The in section.</returns>
			/// <param name="tableView">Table view.</param>
			/// <param name="section">Section.</param>
			public override nint RowsInSection (UITableView tableView, nint section)
			{
				var sectionTitle = _sectionTitles [section];

				return _resultContacts
					.Where ((c) => c.ContactNamePinYinFirst [0].ToString () == sectionTitle)
					.Count ();

			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var sectionTitle = _sectionTitles [indexPath.Section];
				var contact = _resultContacts
					.Where ((c) => c.ContactNamePinYinFirst [0].ToString () == sectionTitle)
					.ToList () [indexPath.Row];

				var cellIdentifier = contact.ContactId;
				UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Subtitle,cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);
				}

				cell.TextLabel.Text = contact.ContactName;
				cell.DetailTextLabel.Text = contact.Department+'-'+contact.Position;
				cell.DetailTextLabel.TextColor = UIColor.LightGray;
				cell.ImageView.Image = contact.GetAvatarImage ().Scale (new SizeF (30, 30));
				return cell;
			}
		}
	}
}

