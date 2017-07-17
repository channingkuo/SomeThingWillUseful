#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-13
说明 : 修改服务器地址
****************/
#endregion
using MonoTouch.Dialog;
using UIKit;
using RekTec.Corelib.Utils;
using RekTec.Corelib.Configuration;
using RekTec.MyProfile.Services;

namespace RekTec.MyProfile.Views
{
	/// <summary>
	/// 修改服务器地址
	/// </summary>
	public class SeverAddressEditViewController : DialogViewController
	{
		private string _title;
		private readonly EntryElement _element;

		public SeverAddressEditViewController (RootElement r, string title, string value)
			: base (UITableViewStyle.Grouped, r, true)
		{
			_title = title;
			var root1 = r;
			_element = new EntryElement (string.Empty, "请输入" + root1.Caption, value);
			root1.Add (new Section { _element });
		}

		/// <summary>
		/// 错误处理
		/// </summary>
		/// <returns><c>true</c>, if error was handled, <c>false</c> otherwise.</returns>
		/// <param name="msg">Message.</param>
		private bool HandleError (string msg)
		{
			AlertUtil.Error (msg);
			return true;
		}

		/// <summary>
		/// 页面将要出现的时候
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.TableView.BackgroundView = null;
			this.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;
			this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			if (string.IsNullOrWhiteSpace (GlobalAppSetting.IsFirstOpen)) {
				NavigationItem.HidesBackButton = true;
				_title = "请设置服务器地址";
			}

			if (NavigationItem != null) {
				NavigationItem.Title = _title;
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Save), false);
				NavigationItem.RightBarButtonItem.Clicked += async (sender, e) => {
					var oldValue = GlobalAppSetting.XrmWebApiBaseUrl;
					GlobalAppSetting.XrmWebApiBaseUrl = _element.Value;
					if (GlobalAppSetting.XrmWebApiBaseUrl.EndsWith ("/")) {
						GlobalAppSetting.XrmWebApiBaseUrl = GlobalAppSetting.XrmWebApiBaseUrl;
					} else {
						GlobalAppSetting.XrmWebApiBaseUrl = GlobalAppSetting.XrmWebApiBaseUrl + "/";
					}
					if (oldValue.ToLower () != _element.Value.ToLower ()) {
						#region 调用API检查服务器地址是否正确
						bool isServerUrlCorrect;
						try {
							AlertUtil.ShowWaiting ("正在验证服务器地址...");
							isServerUrlCorrect = await SystemSettingService.TestConnect ();
						} finally {
							AlertUtil.DismissWaiting ();
						}

						if (!isServerUrlCorrect) {
							ErrorHandlerUtil.ReportError ("尝试连接失败，请检查地址是否正确配置！");
							GlobalAppSetting.XrmWebApiBaseUrl = oldValue;
							return;
						}
						#endregion

						AuthenticationService.Logout ();
						SystemSettingService.Cleanup ();
					}

					GlobalAppSetting.IsFirstOpen = "N";
					NavigationController.PopViewController (false);
				};
			}

			ErrorHandlerUtil.Subscribe (HandleError);
		}

		/// <summary>
		/// 页面消失的时候
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			ErrorHandlerUtil.UnSubscribe (HandleError);
		}
	}
}







