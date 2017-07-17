#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-14
说明 : 修改密码页面
****************/
#endregion
using System;
using UIKit;
using CoreGraphics;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Views;
using RekTec.MyProfile.Services;
using RekTec.MyProfile.ViewModels;

namespace RekTec.MyProfile.Views
{
	/// <summary>
	/// 修改密码页面
	/// </summary>
	public class UpdatePwdViewController:BaseViewController
	{
		private UITextField _pwdtxt;
		private UITextField _firstpwdtxt;
		private UITextField _secondpwdtxt;
		private UIViewBuilder _builder;

		/// <summary>
		/// 页面加载的时候执行
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			_builder = new UIViewBuilder (this.View);

			this.Title = "修改密码";

			var rowHeight = UiStyleSetting.NormalButtonHeight + 10;

			_pwdtxt = CreateTextFieldRow ("原密码", UiStyleSetting.PaddingSizeLarge + rowHeight * 0); 
			_firstpwdtxt = CreateTextFieldRow ("新密码", UiStyleSetting.PaddingSizeLarge + rowHeight * 1); 
			_secondpwdtxt = CreateTextFieldRow ("确认密码", UiStyleSetting.PaddingSizeLarge + rowHeight * 2);     

			var btnLogin = _builder.CreateButton (new CGRect (
				                        UiStyleSetting.PaddingSizeMedium,
				                        UiStyleSetting.PaddingSizeLarge * 2 + rowHeight * 3, 
				                        View.Frame.Width - UiStyleSetting.PaddingSizeMedium * 2, 
				                        UiStyleSetting.NormalButtonHeight), "确  定");
          
			btnLogin.TouchUpInside += delegate(object sender, EventArgs e) {
				CheckPassword ();
			};
			View.AddSubview (btnLogin);

			#region Keyboard Hide
			var tap = new UITapGestureRecognizer ();
			tap.AddTarget (() => {
				View.EndEditing (true);
			});
			View.AddGestureRecognizer (tap);
			#endregion

			this.View.BackgroundColor = UiStyleSetting.ViewControllerColor;
		}

		private UITextField CreateTextFieldRow (string caption, nfloat top)
		{
			nfloat labelLeft = UiStyleSetting.PaddingSizeMedium;
			nfloat labelWidth = 80;
			nfloat labelHeight = 40;
			var labelRect = new CGRect (labelLeft, 
				                         UiStyleSetting.PaddingSizeSmall, 
				                         labelWidth, 
				                         labelHeight);

			var rowHeight = labelHeight + 10;

			nfloat textLeft = labelWidth + UiStyleSetting.PaddingSizeMedium;
			var textRect = new CGRect (textLeft, 
				                        UiStyleSetting.PaddingSizeSmall, 
				                        View.Frame.Width - textLeft - UiStyleSetting.PaddingSizeMedium,
				                        40);

			var rowView = new UIView (new CGRect (0, top, View.Bounds.Width, rowHeight));
			rowView.BackgroundColor = UIColor.White;
			rowView.Layer.BorderWidth = 1;
			rowView.Layer.BorderColor = UiStyleSetting.ViewControllerColor.CGColor;

			var lbl = new UILabel (labelRect);
			lbl.Text = caption;
			lbl.AutoresizingMask = UIViewAutoresizing.All;
			lbl.Font = UIFont.SystemFontOfSize ((nfloat)18);
			rowView.AddSubview (lbl);

			var txtBox = new UITextField (textRect);
			txtBox.AutoresizingMask = UIViewAutoresizing.All;
			txtBox.Placeholder = "请输入" + caption;
			txtBox.SecureTextEntry = true;
			txtBox.BackgroundColor = UIColor.White;
			rowView.AddSubview (txtBox);

			View.AddSubview (rowView);

			return txtBox;
		}

		/// <summary>
		/// 确认按钮检查
		/// </summary>
		public async void CheckPassword ()
		{
			if (string.IsNullOrEmpty (_pwdtxt.Text)
			             || string.IsNullOrEmpty (_firstpwdtxt.Text) || string.IsNullOrEmpty (_secondpwdtxt.Text)) {
				AlertUtil.Error ("请输入原密码");
				return;
			}

			if (_firstpwdtxt.Text != _secondpwdtxt.Text) {
				AlertUtil.Error ("新密码与确认密码不一致！请重新输入！");
				return;
			}
			var password = new UserPasswordViewModel () {
				userName = GlobalAppSetting.UserCode,
				oldPwd = EncryptionUtil.DESDefaultEncryption (_pwdtxt.Text),
				newPwd = EncryptionUtil.DESDefaultEncryption (_secondpwdtxt.Text)
			};

			var b = await UserService.ChangePassword (password);
			if (b) {
				AlertUtil.Success ("修改成功");
				NavigationController.PopViewController (false);
			}
		}
	}
}

