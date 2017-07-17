#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-13
说明 : 点击设置进去显示的页面
****************/

using MonoTouch.Dialog;

#endregion

using System;
using System.Globalization;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.MyProfile.Services;
using UIKit;
using RekTec.Contacts.Services;
using Foundation;
using LocalAuthentication;

namespace RekTec.MyProfile.Views
{
	/// <summary>
	/// 点击设置进去显示的页面
	/// </summary>
	public class SystemSettingViewController : DialogViewController
	{
		public Action OnLogoutButtonClick;

		readonly RootElement _root;
		StyledStringElement _severAddressElement, _clearCacheElement, _suggestElement, _aboutElement, _changePasswordElement, _logoutButton;

		private BooleanElement _isEnableHtml5DebugElement;

        private BooleanElement _isEnableTouchIDElement;


        public SystemSettingViewController (RootElement r)
			: base (UITableViewStyle.Grouped, r, true)
		{
			_root = r;
			_root.Clear ();
		}

		/// <summary>
		/// 页面加载的时候执行
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavigationItem.BackBarButtonItem = new UIBarButtonItem (string.Empty, UIBarButtonItemStyle.Plain, null);

			//服务器地址
			_severAddressElement = new StyledStringElement ("服务器") {
				Accessory = UITableViewCellAccessory.DisclosureIndicator,
				Image = UIImage.FromFile ("ic_change_serverurl.png"),
				BackgroundColor = UIColor.White
			};
			//版本检查
			//_versionElment = new StyledStringElement("应用版本", versioncode){ Accessory = UITableViewCellAccessory.None };
			//_versionElment.BackgroundColor = UIColor.White;
			//清空缓存
			_clearCacheElement = new StyledStringElement ("清空缓存", FileSystemUtil.GetFileSize (FileSystemUtil.TmpFolder) + "kb") {
				Accessory = UITableViewCellAccessory.DisclosureIndicator,
				Image = UIImage.FromFile ("ic_clear_cache.png"),
				BackgroundColor = UIColor.White
			};
			//意见反馈
			_suggestElement = new StyledStringElement ("意见反馈") {
				Accessory = UITableViewCellAccessory.DisclosureIndicator,
				Image = UIImage.FromFile ("ic_suggestion.png"),
				BackgroundColor = UIColor.White
			};
			_root.Add (new [] { new Section () {
					_severAddressElement, /*_versionElment,*/
					_clearCacheElement,
					_suggestElement
				}
			});


			//修改密码
			_changePasswordElement = new StyledStringElement ("修改密码") {
				Accessory = UITableViewCellAccessory.DisclosureIndicator,
				Image = UIImage.FromFile ("ic_change_password.png"),
				BackgroundColor = UIColor.White
			};
			//_root.Add (new[] { new Section () { _changePasswordElement } });

			//关于
			_aboutElement = new StyledStringElement ("关于") {
				Accessory = UITableViewCellAccessory.DisclosureIndicator,
				Image = UIImage.FromFile ("ic_aboutus.png"),
				BackgroundColor = UIColor.White
			};
			_root.Add (new [] { new Section () { _changePasswordElement } });

            // 设置是否启用 TouchID
            _isEnableTouchIDElement = new BooleanElement("启用TouchID", GlobalAppSetting.isTouchID);
            _isEnableTouchIDElement.ValueChanged += (s, e) => {
                GlobalAppSetting.isTouchID = _isEnableTouchIDElement.Value;
                EnableTouchID();
            };
            _root.Add(new Section[] { new Section() { _isEnableTouchIDElement } });


#if DEBUG
            _isEnableHtml5DebugElement = new BooleanElement ("启用HTML调试模式", GlobalAppSetting.IsHTML5Debug);
			_isEnableHtml5DebugElement.ValueChanged += (s, e) => {
				GlobalAppSetting.IsHTML5Debug = _isEnableHtml5DebugElement.Value;
			};
			_root.Add (new [] { new Section () { _isEnableHtml5DebugElement } });
#endif

			//退出登录
			_logoutButton = new StyledStringElement ("退出登录") {
				TextColor = UIColor.Red,
				Alignment = UITextAlignment.Center,
				BackgroundColor = UIColor.White
			};
			_root.Add (new [] { new Section () { _logoutButton } });

			BindEvent ();
			LoadUIData ();
		}

		/// <summary>
		/// 页面将要出现的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ErrorHandlerUtil.Subscribe (HandleError);
			if (!AuthenticationService.IsLogOn ()) {
				AuthenticationService.Logout ();
				NavigationController.PopViewController (true);
				if (OnLogoutButtonClick != null) {
					OnLogoutButtonClick ();
				}
			}
		}

		/// <summary>
		/// 页面将要消失的时候执行
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear (bool animated)
		{
			ViewDidDisappear (animated);
			ErrorHandlerUtil.UnSubscribe (HandleError);
		}

		/// <summary>
		/// 处理错误的方法
		/// </summary>
		/// <returns><c>true</c>, if error was handled, <c>false</c> otherwise.</returns>
		/// <param name="msg">Message.</param>
		private bool HandleError (string msg)
		{
			AlertUtil.Error (msg);
			return true;
		}

		/// <summary>
		/// 绑定各个点击事件
		/// </summary>
		private void BindEvent ()
		{
			//服务器地址修改
			_severAddressElement.Tapped += () => {
				NavigationController.PushViewController (new SeverAddressEditViewController (new RootElement ("服务器地址"), "服务器", GlobalAppSetting.XrmWebApiBaseUrl), true);
			};

			//清空缓存
			_clearCacheElement.Tapped += () => {
				var size = FileSystemUtil.GetFileSize (FileSystemUtil.TmpFolder);
				FileSystemUtil.DeleteFile (FileSystemUtil.TmpFolder);
				_clearCacheElement.Value = FileSystemUtil.GetFileSize (FileSystemUtil.TmpFolder).ToString (CultureInfo.InvariantCulture) + "kb";

				// 删除通讯录的本地缓存
				ContactsDataRepository.DeleteAll ();

				ReloadData ();
				AlertUtil.Success (string.Format ("清除缓存成功,共清除了{0}kb", size));
			};

			//意见建议
			_suggestElement.Tapped += () => {
				NavigationController.PushViewController (new SuggestEditViewController (new RootElement ("意见反馈"), "意见反馈"), true);
			};

			/* App中不允许有版本检查的相关内容
            //版本检查
            _versionElment.Tapped += async() => {
                using (Toast t = new Toast()) {
                    //尝试更新HTMl5程序的版本
                    t.ProgressWaiting("正在检查更新版本...");
                    var isupdate = await VersionService.TryUpgradeWww();
                    ReloadData();
                    if (!isupdate) {
                        AlertUtil.Error("当前已经是最新版本了");
                    }
                }
            };
            */

			//退出登录
			_logoutButton.Tapped += () => {
				AuthenticationService.Logout ();
				NavigationController.PopViewController (true);
				if (OnLogoutButtonClick != null) {
					OnLogoutButtonClick ();
				}
			};

			//密码修改
			_changePasswordElement.Tapped += () => {
				NavigationController.PushViewController (new UpdatePwdViewController (), true);
			};

			//关于
			_aboutElement.Tapped += () => {
				NavigationController.PushViewController (new AboutUsViewController (), true);
			};

		}

        NSError error;
        LAContext context = new LAContext();
        /// <summary>
        /// 判断TouchID是否启用成功
        /// </summary>
        #region 检查设备是否支持TouchID
        private void EnableTouchID()
        {
            //确认设备是否支持 Touch ID
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
            {
                var replyHandler = new LAContextReplyHandler((success, error) =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (success)
                        {
                            var okAlertController = UIAlertController.Create("提示", "TouchID 启用成功！", UIAlertControllerStyle.Alert);
                            okAlertController.AddAction(UIAlertAction.Create("好的", UIAlertActionStyle.Default, null));
                            //PresentViewController (okAlertController, true, null);

                            var okCancelAlertController = UIAlertController.Create("提示", "TouchID 启用成功！", UIAlertControllerStyle.Alert);
                            okCancelAlertController.AddAction(UIAlertAction.Create("好的", UIAlertActionStyle.Default, alert => Console.WriteLine("Okay was clicked")));
                            PresentViewController(okCancelAlertController, true, null);
                        }
                        else
                        {
                            _isEnableTouchIDElement.Value = false;
                            GlobalAppSetting.isTouchID = false;
                            var okAlertController = UIAlertController.Create("提示", "TouchID 未启用成功，请稍后重试！", UIAlertControllerStyle.Alert);
                            okAlertController.AddAction(UIAlertAction.Create("好的", UIAlertActionStyle.Default, null));
                            PresentViewController(okAlertController, true, null);
                        }
                    });
                });

            }
            else
            {
                _isEnableTouchIDElement.Value = false;
                GlobalAppSetting.isTouchID = false;
                var okAlertController = UIAlertController.Create("提示", "您的设备不支持TouchID，\n指纹登录将不会启用！", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("好的", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);

            }


        }
        #endregion

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadUIData ()
		{
			//_versionElment.Value = versioncode;

			ReloadData ();
		}
	}
}

