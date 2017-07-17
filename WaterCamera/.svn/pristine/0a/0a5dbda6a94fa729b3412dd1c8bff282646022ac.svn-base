#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : Dropdown list Control
****************/
#endregion

using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;
using RekTec.Corelib.Configuration;
using ObjCRuntime;
using System.Diagnostics;

namespace RekTec.Corelib.Views
{
	public delegate void DelegateMethod (string value);
	/// <summary>
	/// Drop down list Control
	/// </summary>
	public class DropdownListView : UITextField
	{
		public DelegateMethod OnDelegate;
		public Action<DropdownListViewItem> delegateCallBack;
		private static readonly UIImage _image;

		static DropdownListView ()
		{
			_image = UIImage.FromFile ("ic_dropdown.png");
		}


		private UIImageView _iconImage;
		private UIButton _btn;
		private UIViewController _c;
		private string tempAreaCode;
		private int TempPlaceHolder;

		public DropdownListView (CGRect rect, string placeholder, UIViewController c)
			: base (rect)
		{
			this._c = c;
			this.Placeholder = placeholder;
			this.AdjustsFontSizeToFitWidth = true;
			this.Font = UIFont.SystemFontOfSize ((nfloat)UiStyleSetting.FontTitleSize);
			this.TextColor = UIColor.Gray;
			this.TextAlignment = UITextAlignment.Left;
			this.BackgroundColor = UIColor.White;

			this.ShouldBeginEditing += (t) => {
				return false;
			};

			_iconImage = new UIImageView (new CGRect (0, 0, 16, 16));
			_iconImage.Image = _image;

			_btn = new UIButton (new CGRect (0, (rect.Height - 16) / 2, 16 + 10, 16));

			_btn.SetTitle ("", UIControlState.Normal);
			_btn.SetTitleColor (UIColor.Black, UIControlState.Normal);
			_btn.Font = UIFont.SystemFontOfSize ((nfloat)UiStyleSetting.FontTitleSize);
			_btn.AddSubview (_iconImage);

			this.RightView = _btn;
			this.RightViewMode = UITextFieldViewMode.Always;

			var leftimage = new UIImageView (new CGRect (0, (rect.Height - 16) / 2, 20, 20));
			leftimage.Image = UIImage.FromFile ("login_hotel.png");
			this.LeftView = new UIView (new CGRect (0, 0, 20 + 14, rect.Height));
			this.LeftView.AddSubview (leftimage);
			this.LeftViewMode = UITextFieldViewMode.Always;

			var btn = new UIButton (new CGRect (30, 0, 50, 16));
			btn.TouchUpInside += (sender, e) => {
				Debug.WriteLine ("123erds");
				PopupSelection ();
			};
			this.AddSubview (btn);

			_btn.TouchUpInside += (sender, e) => PopupSelection ();
			this.TouchUpInside += (sender, e) => PopupSelection ();
			//viewOfLeft.AddGestureRecognizer (new BTTapGestureRecognizer (this, new Selector ("Tap")));
		}

		[Export ("Tap")]
		public void Tap ()
		{
			PopupSelection ();
		}

		/// <summary>
		/// Tap手势监视类
		/// </summary>
		public class BTTapGestureRecognizer : UITapGestureRecognizer
		{
			public BTTapGestureRecognizer (NSObject target, Selector action) : base (target, action)
			{
				ShouldReceiveTouch += (sender, touch) => {
					return true;
				};
			}
		}

		public DropdownListView (CGRect rect, string placeholder, UIViewController c, IList<DropdownListViewItem> items)
			: this (rect, placeholder, c)
		{
			this.Items = items;
			if (this.Items != null && this.Items.Count > 0) {
				this.SelectedItem = this.Items [0];
			}
			if (placeholder == "大区") {
				TempPlaceHolder = 1;

			} else if (placeholder == "二区") {
				TempPlaceHolder = 2;
			} else {
				TempPlaceHolder = 0;
			}
		}
		private void testMethod (DropdownListViewItem item)
		{

			this.SelectedItem = item;
			if (delegateCallBack != null) {
				//OnDelegate (this.SelectedItem.Value);
				delegateCallBack (item);
			}

		}
		private void PopupSelection ()
		{
			if (Items == null || Items.Count == 0) {
				return;
			}

			UIAlertController actionSheetAlert = UIAlertController.Create ("请选择", "", UIAlertControllerStyle.ActionSheet);

			foreach (var item in Items) {
				actionSheetAlert.AddAction (UIAlertAction.Create (item.Text,
					UIAlertActionStyle.Default,
					//(action) => this.SelectedItem = item
					(action) => testMethod (item)
				));
			}
			actionSheetAlert.AddAction (UIAlertAction.Create ("取消", UIAlertActionStyle.Cancel, (action) => { }));

			// Required for iPad - You must specify a source for the Action Sheet since it is
			// displayed as a popover
			UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
			if (presentationPopover != null) {
				presentationPopover.SourceView = _c.View;
				presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			}

			// Display the alert
			_c.PresentViewController (actionSheetAlert, true, null);
		}

		public IList<DropdownListViewItem> Items;

		private DropdownListViewItem _selectedItem;

		public DropdownListViewItem SelectedItem {
			get {
				return _selectedItem;
			}
			set {
				_selectedItem = value;
				if (_selectedItem != null) {
					this.Text = _selectedItem.Text;
				}
			}
		}

		public string SelectedValue {
			get {
				if (SelectedItem != null) {
					return SelectedItem.Value;
				} else {
					return string.Empty;
				}
			}
			set {
				if (Items == null || Items.Count == 0) {
					return;
				}

				foreach (var item in Items) {
					if (item.Value == value) {
						SelectedItem = item;
					}
				}
			}
		}
	}

	public class DropdownListViewItem
	{
		public string Value { get; set; }

		public string Text { get; set; }

		public string BrandCd { get; set; }
	}
}

