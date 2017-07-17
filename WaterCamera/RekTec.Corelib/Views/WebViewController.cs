#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-09
说明 : 用于内嵌html5功能的webview页面
****************/
#endregion

using System;
using System.Diagnostics;
using System.IO;
using CoreGraphics;
using Foundation;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using UIKit;
using RekTec.MyProfile.Services;
using RekTec.Version.Services;
using ObjCRuntime;

namespace RekTec.Corelib.Views
{
	/// <summary>
	/// 用于内嵌html5功能的webview页面
	/// </summary>
	public class WebViewController : BaseViewController
	{
		public Action OnLogoutButtonClick;
		//菜单编码
		public string _menuUrl;
		public string _special;
		//是否首次加载
		private bool _isFirstLoad = true;
		static UIWebView _webView;

		static bool isFirstScan = true;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			UIView status = new UIView (new CGRect (0, 0, View.Frame.Width, UiStyleSetting.StatusBarHeight));
			status.BackgroundColor = UiStyleSetting.RektecBlueColor;
			if (_menuUrl == "pms/home")
				status.BackgroundColor = UIColor.White;
			View.AddSubview (status);
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.Default, false);
			//隐藏导航栏
			if (_menuUrl.Contains ("tmp")) {
				this.NavigationController.NavigationBarHidden = false;
				_webView.ScalesPageToFit = true;
				NavigationItem.Title = "附件";
				UILabel label = new UILabel (new CGRect (View.Bounds.Width / 2 - 100, View.Bounds.Height / 2 - 200, 250, 250));
				label.BackgroundColor = UIColor.Clear;
				label.TextColor = UIColor.FromRGB (217, 234, 211);
				label.Font = UIFont.SystemFontOfSize (50F);
				label.TextAlignment = UITextAlignment.Center;
				label.Transform = CGAffineTransform.MakeRotation ((float)Math.PI / 4);
				label.Text = GlobalAppSetting.UserCode;
				_webView.AddSubview (label);
			} else {
				this.NavigationController.NavigationBarHidden = true;
			}

			//NavigationItem返回按钮控制webView的返回
			NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (UIImage.FromFile ("ios_back.png"), UIBarButtonItemStyle.Plain, null), false);
			NavigationItem.LeftBarButtonItem.Clicked += (sender, e) => {
				if (_webView.CanGoBack && !_menuUrl.Contains ("tmp")) {
					_webView.GoBack ();
				} else {
					_webView.ShouldStartLoad = null;
					_webView.StopLoading ();

					if (_webView != null) {
						_webView.RemoveFromSuperview ();
						_webView.Dispose ();
						_webView = null;
					}
					if (_webView == null) {
						NavigationController.PopViewController (false);
					}
				}
			};

			isFirstScan = true;
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//获取手机UUID，存到GlobalAppSetting中
			GlobalAppSetting.UUID = UIDevice.CurrentDevice.IdentifierForVendor.AsString ().GetHashCode ().ToString ();

			// 读取手机剩余空间，过小时给出提示
			var freeSpacePath = NSFileManager.DefaultManager.GetFileSystemAttributes (Environment.GetFolderPath (Environment.SpecialFolder.Personal));
			var freeSpace = freeSpacePath.FreeSize;
			freeSpace = (freeSpace / 1024) / 1024;
			var isOverFlow = ((freeSpace - 200) < 50) || (freeSpace <= 200);
			Debug.WriteLine (freeSpace);
			//if (freeSpace - 200 < 50 || freeSpace - 200 < 0) {
			//	var alertAction = UIAlertController.Create ("存储空间不足", "你的存储空间不足请清理一些空间再使用!", UIAlertControllerStyle.Alert);
			//	alertAction.AddAction (UIAlertAction.Create ("确认", UIAlertActionStyle.Default, alert => {
			//		NavigationController.PopToRootViewController (false);
			//	}));
			//	PresentViewController (alertAction, true, null);
			//}

			var destFolderName = Path.Combine (FileSystemUtil.CachesFolder, "www");
			if (Directory.Exists (destFolderName)) {
				File.Copy (Path.Combine (NSBundle.MainBundle.ResourcePath, "_blank.html")
					, Path.Combine (destFolderName, "_blank.html"), true);
			} else {
				if (isOverFlow) {
					var alertAction = UIAlertController.Create ("存储空间不足", "你的存储空间不足请清理一些空间再使用!", UIAlertControllerStyle.Alert);
					alertAction.AddAction (UIAlertAction.Create ("确认", UIAlertActionStyle.Default, alert => {
						TerminateWithSuccess ();
						//NavigationController.PopToRootViewController (false);
					}));
					PresentViewController (alertAction, true, null);
				}
				GlobalAppSetting.WwwVersion = "1.0.0.0";
				await VersionService.TryUpgradeWww ();
				NavigationController.PopToRootViewController (false);
			}

			if (this.ParentViewController != null && this.ParentViewController.NavigationItem != null) {
				this.ParentViewController.NavigationItem.SetRightBarButtonItem (null, false);
			}

			//清除缓存
			NSUrlCache.SharedCache.RemoveAllCachedResponses ();

			//修改状态栏背景颜色
			UIView statusview = new UIView (new CGRect (0, 0, View.Frame.Width, UiStyleSetting.StatusBarHeight));
			statusview.BackgroundColor = UiStyleSetting.RektecBlueColor;
			View.AddSubview (statusview);

			//设置cookie的接受政策
			NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;

			_webView = new UIWebView (new CGRect (0, UiStyleSetting.StatusBarHeight, View.Bounds.Width, View.Bounds.Height - UiStyleSetting.StatusBarHeight));
			_webView.ScalesPageToFit = false;
			if (_menuUrl.Contains ("tmp")) {
				_webView = new UIWebView (new CGRect (0, 0, View.Bounds.Width, View.Bounds.Height));
				_webView.ScalesPageToFit = true;
			}
			_webView.LoadError += (sender, e) => {
				Debug.WriteLine (e.Error);
			};
			//设置页面不允许自动缩放
			//_webView.ScalesPageToFit = false;

			_webView.ShouldStartLoad = (webView, request, navType) => {
				var requesturl = request.Url.ToString ();

				// 检测URL里包含{username}，则替换成带域名的UserCode
				if (requesturl.Contains ("{username}")) {
					requesturl = requesturl.Replace ("{username}", GlobalAppSetting.DomainUserCode);
				}

				if (requesturl.Contains ("/app/close")) { //检测到URL里包含app/close,则关闭应用
					NavigationController.NavigationBarHidden = false;
					NavigationController.PopViewController (false);

					_webView.ShouldStartLoad = null;
					_webView.StopLoading ();

					if (_webView != null) {
						_webView.RemoveFromSuperview ();
						_webView.Dispose ();
						_webView = null;
					}

					return false;
				}

				if (requesturl.Contains ("/app/exit")) { //检测到URL里包含app/exit,则退出登录
					AuthenticationService.Logout ();
					this.NavigationController.PopViewController (true);
					if (OnLogoutButtonClick != null) {
						OnLogoutButtonClick ();
					}
					return false;
				}

				if (requesturl.Contains ("app:take-photo")) {//检测到URL里包含app:take-photo,则打开相机进行拍照
					#region 获取Url里传入的px和kb参数
					float kb = 80;
					float px = 800;

					var kbParam = string.Empty;
					var pxParam = string.Empty;
					var parameter = requesturl.Split ('?');
					if (parameter.Length > 1) {
						var parameters = parameter [1].Split ('&');
						foreach (var param in parameters) {
							var nv = param.Split ('=');
							if (nv.Length < 2)
								continue;

							if (nv [0].ToLower () == "kb") {
								kbParam = nv [1];
							}

							if (nv [0].ToLower () == "px") {
								pxParam = nv [1];
							}
						}

						if (!string.IsNullOrWhiteSpace (kbParam)) {
							try {
								kb = float.Parse (kbParam);
							} catch {
							}
						}

						if (!string.IsNullOrWhiteSpace (pxParam)) {
							try {
								px = float.Parse (pxParam);
							} catch {
							}
						}
					}
					#endregion

					CameraUtil.TakePicture (this, img => {
						var isNeedRotate = _menuUrl.Contains ("pms/home");
						var imgBase64 = ImageUtil.ConvertImage2Base64String (img, kb, px, isNeedRotate);
						_webView.EvaluateJavascript ("window.XrmImageData={imageData:'" + imgBase64 + "',getXrmImageData:function(){return this.imageData;},clearImageData:function(){this.imageData='';}}");
					});

					return false;
				}

				if (requesturl.Contains ("app:choose-photo")) {//检测到URL里包含app:choose-photo,则打开相册选择照片
					#region 获取Url里传入的px和kb参数
					float kb = 80;
					float px = 800;

					var kbParam = string.Empty;
					var pxParam = string.Empty;
					var parameter = requesturl.Split ('?');
					if (parameter.Length > 1) {
						var parameters = parameter [1].Split ('&');
						foreach (var param in parameters) {
							var nv = param.Split ('=');
							if (nv.Length < 2)
								continue;

							if (nv [0].ToLower () == "kb") {
								kbParam = nv [1];
							}

							if (nv [0].ToLower () == "px") {
								pxParam = nv [1];
							}
						}

						if (!string.IsNullOrWhiteSpace (kbParam)) {
							try {
								kb = float.Parse (kbParam);
							} catch {
							}
						}

						if (!string.IsNullOrWhiteSpace (pxParam)) {
							try {
								px = float.Parse (pxParam);
							} catch {
							}
						}
					}
					#endregion

					CameraUtil.SelectPicture (this, img => {
						var imgBase64 = ImageUtil.ConvertImage2Base64String (img, kb, px);
						_webView.EvaluateJavascript ("window.XrmImageData={imageData:'" + imgBase64 + "',getXrmImageData:function(){return this.imageData;},clearImageData:function(){this.imageData='';}}");
					});

					return false;
				}

				if (requesturl.Contains ("app:ocr-takephoto-scan")) {//检测到URL里包含app:ocr-takephoto-scan,则识别证件信息
					#region 获取识别的证件类型
					//string ocrType = string.Empty;

					//var parameter = requesturl.Split ('?');
					//if (parameter.Length > 1) {
					//	var parameters = parameter [1].Split ('=');
					//	if (parameters.Length > 1 && parameters [0] == "OCRType") {
					//		ocrType = parameters [1];
					//	}
					//}

					//if (string.IsNullOrWhiteSpace (ocrType)) {
					//	ocrType = "IDCard";
					//}
					#endregion
					//OCRUtil.TakePicture (this, imageData => {
					//	_webView.EvaluateJavascript ("window.XrmOCRData={OCRData:'" + imageData + "',getXrmOCRData:function(){return this.OCRData;},clearOCRData:function(){this.OCRData='';}}");
					//}, ocrType);
					var scrViewController = new OCRVideoUtil {
						webView = _webView
					};
					if (isFirstScan) {
						NavigationController.PushViewController (scrViewController, true);
						isFirstScan = false;
					}

					return false;
				}

				if (requesturl.Contains ("app:ocr-choosephoto-scan")) {//检测到URL里包含app:ocr-choosephoto--scan,则识别证件信息
					#region 获取识别的证件类型
					//string ocrType = string.Empty;

					//var parameter = requesturl.Split ('?');
					//if (parameter.Length > 1) {
					//	var parameters = parameter [1].Split ('=');
					//	if (parameters.Length > 1 && parameters [0] == "OCRType") {
					//		ocrType = parameters [1];
					//	}
					//}

					//if (string.IsNullOrWhiteSpace (ocrType)) {
					//	ocrType = "IDCard";
					//}
					#endregion
					//OCRUtil.SelectPicture (this, imageData => {
					//	_webView.EvaluateJavascript ("window.XrmOCRData={OCRData:'" + imageData + "',getXrmOCRData:function(){return this.OCRData;},clearOCRData:function(){this.OCRData='';}}");
					//}, ocrType);
					var scrViewController = new OCRVideoUtil {
						webView = _webView
					};
					if (isFirstScan) {
						NavigationController.PushViewController (scrViewController, true);
						isFirstScan = false;
					}

					return false;
				}

				//检测到URL里包含 app:scan
				if (requesturl.Contains ("app:scan")) {
					BarcodeUtil.Scan (result => {
						InvokeOnMainThread (() => {
							_webView.EvaluateJavascript ("window.XrmScanData={scanData:'" + result + "',getResult:function(){return this.scanData;},clearResult:function(){this.scanData='';}}");
						});
					});

					return false;
				}
				//检测到URL里包含FileDownloadHandler.ashx,则打开浏览器进行附件下载
				if (requesturl.Contains ("FileDownloadHandler.ashx")) {
					var fileUrl = requesturl;
					fileUrl = fileUrl.Substring (GlobalAppSetting.XrmWebApiBaseUrl.Length);
					#region 获取Url里传入的fileId和fileName参数
					string fileId = string.Empty;
					string fileExt = string.Empty;
					var parameter = requesturl.Split ('?');
					if (parameter.Length > 1) {
						var parameters = parameter [1].Split ('&');
						foreach (var param in parameters) {
							var nv = param.Split ('=');
							if (nv.Length < 2)
								continue;

							if (nv [0].ToLower () == "fileid") {
								fileId = nv [1];
							}

							if (nv [0].ToLower () == "fileext") {
								fileExt = nv [1];
							}
						}
					}

					#endregion
					//先到本地查找附件，存在则直接通过一个新的webview打开，不存在就下载附件到本地然后再通过新webview打开
					string localDocUrl = Path.Combine (FileSystemUtil.TmpFolder, fileId + "." + fileExt);
					if (!File.Exists (localDocUrl)) {
						AlertUtil.ShowWaiting ("正在下载附件...");
						VersionService.DownloadAttachmentFile (fileUrl, fileId, fileExt, () => {
							AlertUtil.DismissWaiting ();
							NavigationController.PushViewController (new WebViewController () { _menuUrl = localDocUrl }, false);
						}, (msg) => {
							AlertUtil.DismissWaiting ();
							AlertUtil.Error (msg);
						});
					} else {
						NavigationController.PushViewController (new WebViewController () { _menuUrl = localDocUrl }, false);
					}
					//UIApplication.SharedApplication.OpenUrl (request.Url);
					return false;
				}
				return true;
			};

			string address;
			if (GlobalAppSetting.XrmWebApiBaseUrl.EndsWith ("/")) {
				address = GlobalAppSetting.XrmWebApiBaseUrl;
			} else {
				address = GlobalAppSetting.XrmWebApiBaseUrl + "/";
			}
			_webView.LoadFinished += (sender, e) => {
				if (_isFirstLoad) {

					// 给H5的各种变量预先赋值
					_webView.EvaluateJavascript ("window.localStorage.setItem('HotelCd','" + GlobalAppSetting.HotelCD + "');");
					_webView.EvaluateJavascript ("window.localStorage.setItem('HotelName','" + GlobalAppSetting.HotelName + "');");
					_webView.EvaluateJavascript ("window.localStorage.setItem('BrandCd','" + GlobalAppSetting.BrandCd + "');");
					_webView.EvaluateJavascript ("window.localStorage.setItem('PmsServerUrl','" + GlobalAppSetting.PmsServerUrl + "');");
					_webView.EvaluateJavascript ("window.localStorage.setItem('UUID','" + GlobalAppSetting.UUID + "');");
					_webView.EvaluateJavascript ("window.localStorage.setItem('SSOToken','" + GlobalAppSetting.SsoToken + "');");
					_webView.EvaluateJavascript ("window.localStorage.setItem('UserName','" + GlobalAppSetting.UserCode + "');");

					var urls = _menuUrl.Split ('@');

					var webviewUrl = urls.Length <= 1 ? urls [0] : urls [1];

					if (webviewUrl.ToLower ().StartsWith ("http")) {
						var website = "window.location=" + "'" + webviewUrl.Replace ("{username}", GlobalAppSetting.DomainUserCode).Replace ("\\", "\\\\") + "';";
						_webView.EvaluateJavascript (website);
					} else {
						_webView.EvaluateJavascript ("window.localStorage.setItem('XrmBaseUrl','" + address + "');");
						_webView.EvaluateJavascript ("window.localStorage.setItem('XrmAuthToken','" + GlobalAppSetting.XrmAuthToken + "');");
						_webView.EvaluateJavascript ("window.localStorage.setItem('UserId','" + GlobalAppSetting.UserId + "');");

						var js = "window.location.href='index.html#/" + webviewUrl + "?v=1';";
						if (_menuUrl == "pms/home") {
							js = js.Replace ("index", "pms");
						}
						_webView.EvaluateJavascript (js);
					}

					_isFirstLoad = false;
				}
			};
#if DEBUG
			var url = string.Empty;
			if (GlobalAppSetting.IsHTML5Debug) {
				url = Path.Combine (address, "debug/index.html");
				if (_menuUrl == "pms/home")
					url = url.Replace ("index", "pms");
			} else {
				if (File.Exists (Path.Combine (FileSystemUtil.CachesFolder, "www/_blank.html"))) {
					url = Path.Combine (FileSystemUtil.CachesFolder, "www/_blank.html");
				} else {
					url = Path.Combine (NSBundle.MainBundle.ResourcePath, "_blank.html");
				}
			}

#else
						var url = string.Empty;
						if (File.Exists (Path.Combine (FileSystemUtil.CachesFolder, "www/_blank.html"))) {
							url = Path.Combine (FileSystemUtil.CachesFolder, "www/_blank.html");
						} else {
							url = Path.Combine (NSBundle.MainBundle.ResourcePath, "_blank.html");
						}
#endif
			if (!string.IsNullOrWhiteSpace (_special)) {
				url = _special;
			}

			if (_menuUrl.Contains ("tmp")) {
				url = _menuUrl;
			}
			_webView.LoadRequest (new NSUrlRequest (new NSUrl (url)));//跳转自定义url
			_webView.AutoresizingMask = (UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth);

			View.AddSubview (_webView);
		}

		//退出程序
		static void TerminateWithSuccess ()
		{
			Selector selector = new Selector ("terminateWithSuccess");
			UIApplication.SharedApplication.PerformSelector
				(selector, UIApplication.SharedApplication, 0);
		}

		/// <summary>
		/// Refreshs the SSO Token.
		/// </summary>
		public static void RefreshSSOToken ()
		{
			if (_webView != null)
				_webView.EvaluateJavascript ("window.localStorage.setItem('SSOToken','" + GlobalAppSetting.SsoToken + "');");
		}
	}
}

