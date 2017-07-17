using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using RekTec.Chat.Service;
using RekTec.Contacts.Services;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Views;

namespace RekTec.Chat.Views.Contact
{
    public class ContactChooseViewController : BaseViewController
    {
        private UITableView _tableView;
        private UISearchBar _searchBar;
        private bool _isSearching = false;
        private UIToolbar _choosedContactToolBar;
        private List<ContactViewModel> _choosedContacts = new List<ContactViewModel>();
        private UIButton _okButton;
        private UIScrollView _scrollView;
        private static int _maxChoosed = 30;
        private UIToolbar _topToolBar;
        private UIBarButtonItem _closeButton;
        public Action<List<ContactViewModel>> ContactChooseCompleteAction;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
         
            CreateUIElement();

            BindUIElementEvent();
        }

        private void CreateUIElement()
        {   
            var builder = new UIViewBuilder(this.View);
            _topToolBar = builder.CreateToolbar(new CGRect(View.Bounds.X, 
                View.Bounds.Y + UiStyleSetting.StatusBarHeight, 
                View.Bounds.Width, 
                UiStyleSetting.NavigationBarHeight), UiStyleSetting.NavigationBarColor);

            _closeButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            _topToolBar.SetItems(new []{ _closeButton }, false);

            _tableView = builder.CreateTableView(new CGRect(View.Bounds.X, 
                View.Bounds.Y + UiStyleSetting.NavigationBarHeight + UiStyleSetting.StatusBarHeight,
                View.Bounds.Width, 
                View.Bounds.Height - UiStyleSetting.StatusBarHeight - UiStyleSetting.NavigationBarHeight));
           
            _searchBar = builder.CreateSearchBar(new CGRect(_tableView.Bounds.X,
                _tableView.Bounds.Y,
                _tableView.Bounds.Width, 
                UiStyleSetting.SearchBarHeight));

            _tableView.TableHeaderView = _searchBar;

            _choosedContactToolBar = builder.CreateToolbar(new CGRect(View.Bounds.X, View.Bounds.Height - UiStyleSetting.ToolBarHeight,
                View.Bounds.Width, UiStyleSetting.ToolBarHeight));

            _okButton = UIButton.FromType(UIButtonType.Custom);
            _okButton.ClearsContextBeforeDrawing = false;
            _okButton.Frame = new CGRect(_choosedContactToolBar.Bounds.Width - UiStyleSetting.NormalButtonWidth - UiStyleSetting.PaddingSizeSmall,
                UiStyleSetting.PaddingSizeSmall, UiStyleSetting.NormalButtonWidth, UiStyleSetting.NormalButtonHeight);
            _okButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin;
            _okButton.SetTitle("确认", UIControlState.Normal);
            _okButton.BackgroundColor = UiStyleSetting.ButtonBackgroundColor;
            _choosedContactToolBar.AddSubview(_okButton);

            _scrollView = new UIScrollView(new CGRect(_choosedContactToolBar.Bounds.X, _choosedContactToolBar.Bounds.Y,
                _choosedContactToolBar.Bounds.Width - UiStyleSetting.NormalButtonWidth, UiStyleSetting.ToolBarHeight));
            _scrollView.ContentSize = new SizeF(_maxChoosed * (UiStyleSetting.HeadIconSizeSmall + UiStyleSetting.PaddingSizeSmall)
                , UiStyleSetting.ToolBarHeight);
            _scrollView.BackgroundColor = UIColor.Clear;
            _choosedContactToolBar.Add(_scrollView);
        }

        private void BindUIElementEvent()
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
                _searchBar.ResignFirstResponder();
                _isSearching = false;
                _tableView.ReloadData();
            };

            _searchBar.SearchButtonClicked += (object sender, EventArgs e) => {
                _tableView.AllowsSelection = true;
                _tableView.ScrollEnabled = true;
                _searchBar.ResignFirstResponder();
                _tableView.ReloadData();
            };

            _closeButton.Clicked += (sender, e) => {
                this.DismissViewController(true, () => {
                    _choosedContacts.Clear();
                    if (ContactChooseCompleteAction != null) {
                        ContactChooseCompleteAction(_choosedContacts);
                    }
                });
            };
            _okButton.AddTarget((sender, e) => {
                if (_choosedContacts.Count == 0)
                    return;
                        
                this.DismissViewController(false, () => {
                    if (ContactChooseCompleteAction != null) {
                        ContactChooseCompleteAction(_choosedContacts);
                    }
                });
                  
            }, UIControlEvent.TouchUpInside);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _tableView.Source = new Source(this);
            _tableView.ReloadData();


            ContactsDataRepository.SubscribeContactChange((contact) => {
                InvokeOnMainThread(() => {
                    _tableView.Source = new Source(this);
                    _tableView.ReloadData();
                });
            });
        }

        private class Source : UITableViewSource
        {
            private ContactChooseViewController _controller;
            private List<ContactViewModel> _resultContacts;
            private string[] _sectionTitles;

            public Source(ContactChooseViewController controller)
            {
                _controller = controller;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                if (!_controller._isSearching)
                    _resultContacts = ContactsDataRepository.GetAllContacts()
                        .Where(c => c.ContactId != ChatClient.CurrentUserContact.ContactId
                    && !string.IsNullOrWhiteSpace(c.ContactNamePinYinFirst) && c.ContactNamePinYinFirst.Length > 0)
                        .ToList();
                else
                    _resultContacts = ContactsDataRepository.SearchContactsLikeName(_controller._searchBar.Text)
                        .Where(c => c.ContactId != ChatClient.CurrentUserContact.ContactId
                    && !string.IsNullOrWhiteSpace(c.ContactNamePinYinFirst) && c.ContactNamePinYinFirst.Length > 0)
                        .ToList();
                    
                _sectionTitles = _resultContacts
                    .OrderBy((c) => c.ContactNamePinYinFirst[0].ToString())
                    .Select((c) => c.ContactNamePinYinFirst[0].ToString())
                    .Distinct()
                    .ToArray();

                return _sectionTitles.Length;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var sectionTitle = _sectionTitles[indexPath.Section];
                var contact = _resultContacts
                    .Where((c) => c.ContactNamePinYinFirst[0].ToString() == sectionTitle)
                    .ToList()[indexPath.Row];

                if (!_controller._choosedContacts.Exists((c) => c.ContactId == contact.ContactId))
                    _controller._choosedContacts.Add(contact);
                else
                    _controller._choosedContacts.Remove(contact);

                ReloadSelectedImage();

                _controller._okButton.SetTitle(string.Format("确认({0})", _controller._choosedContacts.Count), UIControlState.Normal);

                tableView.DeselectRow(indexPath, false);

                tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.None);
            }

            private void ReloadSelectedImage()
            {
                foreach (var sub in _controller._scrollView.Subviews) {
                    sub.RemoveFromSuperview();
                }

                for (var i = 0; i < _controller._choosedContacts.Count; i++) {
                    var view = new UIImageView(new CGRect(5 + 45 * i, 5, 40, 40));
                    view.Image = _controller._choosedContacts[i].GetAvatarImage();
                    _controller._scrollView.AddSubview(view);
                }
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 50;
            }

            public override string TitleForHeader(UITableView tableView, nint section)
            {
                return _sectionTitles[section];
            }

            public override string[] SectionIndexTitles(UITableView tableView)
            {
                return _sectionTitles;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                var sectionTitle = _sectionTitles[section];

                return _resultContacts
                    .Where((c) => c.ContactNamePinYinFirst[0].ToString() == sectionTitle)
                    .Count();

            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var sectionTitle = _sectionTitles[indexPath.Section];
                var contact = _resultContacts
                    .Where((c) => c.ContactNamePinYinFirst[0].ToString() == sectionTitle)
                    .ToList()[indexPath.Row];

                var cellIdentifier = "TableViewCellUserName";
                ContactChooseTableViewCell cell = tableView.DequeueReusableCell(cellIdentifier) as ContactChooseTableViewCell;
                if (cell == null) {
                    cell = new ContactChooseTableViewCell((NSString)cellIdentifier);
                }

                var choosed = _controller._choosedContacts.Exists((c) => c.ContactId == contact.ContactId);
                cell.UpdateCell(contact.GetAvatarImage(), contact.ContactName, choosed);
                return cell;
            }
        }
    }
}

