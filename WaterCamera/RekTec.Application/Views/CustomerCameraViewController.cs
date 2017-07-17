using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using RekTec.Corelib.Utils;
using UIKit;
using RekTec.Corelib.Configuration;
using CoreLocation;
using RekTec.Corelib;

namespace RekTec.Application
{
	public class CustomerCameraViewController : UIViewController
	{
		public Action<UIImage> cameraCallBack;

		private static AVCaptureSession session;
		private static AVCaptureDeviceInput input;
		private static AVCaptureStillImageOutput output;

		UIView layerView;
		UIButton cancelButton, okButton;
		UIImage outputImage;
		UIImageView locationLogo;
		UILabel userName, location;
		static readonly System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo ("zh-CN");

		public override bool ShouldAutorotate ()
		{
			return false;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait;
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			return UIInterfaceOrientation.Portrait;
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			session.StopRunning ();

			UIApplication.SharedApplication.SetStatusBarHidden (false, false);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			UIApplication.SharedApplication.SetStatusBarHidden (true, false);

			// location
			// 检测定位权限
			var locationStatus = CLLocationManager.Status;
			if (locationStatus == CLAuthorizationStatus.Denied || locationStatus == CLAuthorizationStatus.Restricted) {
				var alertAction = UIAlertController.Create ("请在iPhone的\"设置-隐私-定位\"选项中，允许APP使用定位。", "", UIAlertControllerStyle.Alert);
				alertAction.AddAction (UIAlertAction.Create ("确认", UIAlertActionStyle.Default, alert => { }));
				PresentViewController (alertAction, true, null);
			} else {
				// start location
				var locationMgr = new CLLocationManagerService ();
				locationMgr.GetLocationCallBack += (l) => {
					if (l != null) {
						var addressSize = l.CameraAddress.StringSize (UIFont.SystemFontOfSize (15F));
						location.Frame = new CGRect (View.Bounds.Width - 20 - addressSize.Width, 25, addressSize.Width, addressSize.Height);
						locationLogo.Frame = new CGRect (location.Frame.X - 25, 25, addressSize.Height, addressSize.Height);
						location.Text = l.CameraAddress;
					}
				};
			}

			#region 强制将页面设置成竖屏
			var value = UIInterfaceOrientation.Portrait;
			UIDevice.CurrentDevice.SetValueForKey ((NSNumber)(int)value, (NSString)"orientation");
			#endregion

			layerView = new UIView (View.Bounds);
			View.AddSubview (layerView);

			SetTheCustomerView ();
			SetButtons ();
			SetupCapture ();

			// cancel/ok button setting
			okButton = new UIButton (new CGRect (View.Bounds.Width - 35 - 20, 20, 45, 30));
			okButton.SetTitle ("确认", UIControlState.Normal);
			okButton.BackgroundColor = UiStyleSetting.RektecBlueColor;
			okButton.Layer.CornerRadius = 15;
			okButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			okButton.TouchUpInside += (sender, e) => {
				// save the image to album
				outputImage.SaveToPhotosAlbum (null);

				// deal with the image
				if (cameraCallBack != null) {
					cameraCallBack (outputImage);
				}
				NavigationController.PopViewController (true);
			};
			okButton.Hidden = true;
			layerView.AddSubview (okButton);

			cancelButton = new UIButton (new CGRect (20, View.Bounds.Height - 20 - 20, 40, 30));
			cancelButton.SetTitle ("取消", UIControlState.Normal);
			cancelButton.BackgroundColor = UIColor.Clear;
			cancelButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			cancelButton.SetTitleColor (UIColor.Gray, UIControlState.Focused);
			cancelButton.SetTitleColor (UIColor.Gray, UIControlState.Highlighted);
			cancelButton.SetTitleColor (UIColor.Gray, UIControlState.Selected);
			cancelButton.TouchUpInside += (sender, e) => {
				NavigationController.PopViewController (true);
			};
			layerView.AddSubview (cancelButton);
		}

		/// <summary>
		/// 设置相机
		/// </summary>
		void SetupCapture ()
		{
			// configure the capture session for low resolution, change this if your code
			// can cope with more data or volume
			session = new AVCaptureSession () {
				SessionPreset = AVCaptureSession.PresetHigh
			};

			// create a device input and attach it to the session
			var captureDevice = AVCaptureDevice.DevicesWithMediaType (AVMediaType.Video);
			AVCaptureDevice device = null;
			foreach (AVCaptureDevice camera in captureDevice) {
				if (camera.Position == AVCaptureDevicePosition.Back) {
					device = camera;
				}
			}

			if (device.IsFlashModeSupported (AVCaptureFlashMode.Auto)) {
				NSError error;
				if (device.LockForConfiguration (out error)) {
					device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				}
			}

			input = AVCaptureDeviceInput.FromDevice (device);
			if (input == null) {
				AlertUtil.Error ("后置摄像头存在异常!");
				return;
			}
			session.AddInput (input);

			output = new AVCaptureStillImageOutput {
				OutputSettings = new NSDictionary (AVVideoCodec.JPEG, "AVVideoCodecKey")
			};
			session.AddOutput (output);

			AVCaptureVideoPreviewLayer videoLayer = new AVCaptureVideoPreviewLayer (session);
			View.Layer.MasksToBounds = true;
			videoLayer.Frame = View.Bounds;
			videoLayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
			View.Layer.InsertSublayer (videoLayer, 0);

			session.StartRunning ();
		}

		void SetTheCustomerView ()
		{
			// 水印信息
			var timeLogo = new UIView (new CGRect (0, 20, View.Bounds.Width, 80));
			timeLogo.BackgroundColor = UIColor.Clear;

			var border = new UILabel (new CGRect (20, 3, 2, 80));
			border.BackgroundColor = UIColor.White;
			timeLogo.AddSubview (border);

			var time = new UILabel (new CGRect (30, 0, 120, 40));
			time.TextColor = UIColor.White;
			time.TextAlignment = UITextAlignment.Left;
			time.Font = UIFont.SystemFontOfSize (40F);
			time.Text = DateTime.Now.ToString ("HH:mm");
			timeLogo.AddSubview (time);

			var dateWeek = new UILabel (new CGRect (30, 38, 220, 20));
			dateWeek.TextColor = UIColor.White;
			dateWeek.TextAlignment = UITextAlignment.Left;
			dateWeek.Font = UIFont.SystemFontOfSize (20F);
			dateWeek.Text = DateTime.Now.ToString ("yyyy.MM.dd") + " " + Culture.DateTimeFormat.GetDayName (DateTime.Now.DayOfWeek);
			timeLogo.AddSubview (dateWeek);

			var imgLogo = new UIImageView (new CGRect (30, 60, 20, 20));
			imgLogo.Image = UIImage.FromFile ("Icon.png");
			imgLogo.ContentMode = UIViewContentMode.ScaleAspectFit;
			imgLogo.Layer.MasksToBounds = true;
			imgLogo.Layer.CornerRadius = 10;
			imgLogo.Alpha = (nfloat)0.9;
			timeLogo.AddSubview (imgLogo);

			var appName = new UILabel (new CGRect (55, 62.5, 120, 15));
			appName.TextColor = UIColor.White;
			appName.Alpha = (nfloat)0.8;
			appName.TextAlignment = UITextAlignment.Left;
			appName.Font = UIFont.SystemFontOfSize (15F);
			appName.Text = NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleName").ToString ();
			timeLogo.AddSubview (appName);

			layerView.AddSubview (timeLogo);

			var infoLogo = new UIView (new CGRect (0, View.Bounds.Height - 60, View.Bounds.Width, 45));
			infoLogo.BackgroundColor = UIColor.Clear;

			// TODO get the name from GlobalAppSetting
			var nameStr = "秦进";
			var strSize = nameStr.StringSize (UIFont.SystemFontOfSize (15F));
			userName = new UILabel (new CGRect (View.Bounds.Width - 20 - strSize.Width, 0, strSize.Width, strSize.Height));
			userName.Text = nameStr;
			userName.TextColor = UIColor.White;
			userName.Alpha = (nfloat)0.8;
			userName.TextAlignment = UITextAlignment.Left;
			userName.Font = UIFont.SystemFontOfSize (15F);
			infoLogo.AddSubview (userName);

			var userLogo = new UIImageView (new CGRect (userName.Frame.X - 25, 0, strSize.Height, strSize.Height));
			userLogo.Image = UIImage.FromFile ("setting_normal@2x.png");
			userLogo.ContentMode = UIViewContentMode.ScaleAspectFit;
			infoLogo.AddSubview (userLogo);

			var address = "定位中...";
			var addressSize = address.StringSize (UIFont.SystemFontOfSize (15F));
			location = new UILabel (new CGRect (View.Bounds.Width - 20 - addressSize.Width, 25, addressSize.Width, addressSize.Height));
			location.Text = address;
			location.TextColor = UIColor.White;
			location.Alpha = (nfloat)0.8;
			location.TextAlignment = UITextAlignment.Left;
			location.Font = UIFont.SystemFontOfSize (15F);
			infoLogo.AddSubview (location);

			locationLogo = new UIImageView (new CGRect (location.Frame.X - 25, 25, addressSize.Height, addressSize.Height));
			locationLogo.Image = UIImage.FromFile ("setting_normal@2x.png");
			locationLogo.ContentMode = UIViewContentMode.ScaleAspectFit;
			infoLogo.AddSubview (locationLogo);

			layerView.AddSubview (infoLogo);
		}

		void SetButtons ()
		{
			var rect = new CGRect (View.Bounds.Width / 2 - 30, View.Bounds.Height - 100, 60, 60);
			var takePhoto = new UIButton (rect);
			takePhoto.BackgroundColor = UIColor.White;
			takePhoto.Alpha = (nfloat)0.5;
			takePhoto.SetBackgroundImage (ImageUtil.CreateImageWithColor (UIColor.White, rect), UIControlState.Normal);
			takePhoto.TouchDown += (sender, e) => {
				UIView.Animate (0.3, () => {
					((UIButton)sender).Transform = CGAffineTransform.MakeScale ((nfloat)1.2, (nfloat)1.2);
					((UIButton)sender).Alpha = 1;
				});
			};
			takePhoto.TouchUpOutside += (sender, e) => {
				UIView.Animate (0.3, () => {
					((UIButton)sender).Transform = CGAffineTransform.MakeScale (1, 1);
					((UIButton)sender).Alpha = (nfloat)0.5;
				});
			};
			takePhoto.Layer.CornerRadius = 30;
			takePhoto.TouchUpInside += (sender, e) => {
				takePhoto.Transform = CGAffineTransform.MakeScale (1, 1);
				takePhoto.Alpha = (nfloat)0.5;

				AVCaptureConnection connection = output.ConnectionFromMediaType (AVMediaType.Video);
				output.CaptureStillImageAsynchronously (connection, (imageDataSampleBuffer, error) => {
					if (error == null) {

						takePhoto.Hidden = true;
						cancelButton.Hidden = true;

						NSData imageData = AVCaptureStillImageOutput.JpegStillToNSData (imageDataSampleBuffer);
						UIImage image = UIImage.LoadFromData (imageData);

						session.StopRunning ();
						var size = UIScreen.MainScreen.Bounds.Size;
						UIGraphics.BeginImageContextWithOptions (size, false, 0);
						var context = UIGraphics.GetCurrentContext ();
						UIGraphics.PushContext (context);
						image.Draw (UIScreen.MainScreen.Bounds);
						UIGraphics.PopContext ();
						View.Layer.RenderInContext (context);
						outputImage = UIGraphics.GetImageFromCurrentImageContext ();
						UIGraphics.EndImageContext ();

						okButton.Hidden = false;
						cancelButton.Hidden = false;
					}
				});
			};
			layerView.AddSubview (takePhoto);
		}
	}
}