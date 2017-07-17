using MonoTouch.Dialog;
using Foundation;
using UIKit;
using RekTec.Contacts.ViewModels;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Views;
using ProfileElement = RekTec.Chat.Views.Common.ProfileElement;

namespace RekTec.Chat.Views.Contact
{
	public class ContactDetailViewController : DialogViewController
	{
		RootElement _root;
		ProfileElement _profileElement;
		StyledStringElement _departmentElement, _phoneElement, _emailElement;
		//StyledStringElement _callPhoneButton;
		ButtonElement _callPhoneButton;

		ContactViewModel _contact;

		public ContactDetailViewController (RootElement r, ContactViewModel c)
			: base (UITableViewStyle.Grouped, r, true)
		{
			this.TableView.BackgroundView = null;
			this.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;

			_root = r;
			_root.UnevenRows = true;
			_contact = c;
			_root.Clear ();

			_profileElement = new ProfileElement (_contact.ContactName, "职位: " + _contact.Position, UITableViewCellStyle.Subtitle) {
				Image = _contact.GetAvatarImage ()
			};
			_profileElement.BackgroundColor = UiStyleSetting.NavigationBarColor;    

			_departmentElement = new StyledStringElement ("部门", _contact.Department);
			_departmentElement.BackgroundColor = UiStyleSetting.NavigationBarColor;
			_departmentElement.Image = UIImage.FromFile ("department.png");

			_phoneElement = new StyledStringElement ("手机", _contact.Phone);
			_phoneElement.BackgroundColor = UiStyleSetting.NavigationBarColor;
			_phoneElement.Image = UIImage.FromFile ("phone.png");

			_emailElement = new StyledStringElement ("邮箱", _contact.Email);
			_emailElement.BackgroundColor = UiStyleSetting.NavigationBarColor;
			_emailElement.Image = UIImage.FromFile ("mail.png");

			_callPhoneButton = new ButtonElement("拨打电话"){Flags=ButtonElement.CellFlags.Transparent};

			_root.Add (new Section[] {
                
				new Section () {
					_profileElement, _departmentElement, _phoneElement, _emailElement
				},
				new Section () {
					_callPhoneButton
				},
			});

			BindUIEvent ();
		}

		private void BindUIEvent ()
		{
			_callPhoneButton.Tapped += () => {
				var urlToSend = new NSUrl ("tel:" + _phoneElement.Value); // phonenum is in the format 1231231234

				if (UIApplication.SharedApplication.CanOpenUrl (urlToSend)) {
					UIApplication.SharedApplication.OpenUrl (urlToSend);
				} else {
					AlertUtil.Error ("not support in simulator!");
				}
			};

		}

		private bool HandleError (string msg)
		{
			AlertUtil.Error (msg);
			return true;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ErrorHandlerUtil.Subscribe (HandleError);

//            if (NavigationItem != null) {
//                NavigationItem.Title = "联系人详情";
//            }
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			ErrorHandlerUtil.UnSubscribe (HandleError);

//            if (NavigationItem != null) {
//                NavigationItem.Title = string.Empty;
//            }
		}
	}
}

