﻿#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Channing Kuo
创建时间 : 2017-04-28
说明     : 身份证识别
****************/
#endregion

using UIKit;
using System;
using AVFoundation;
using Foundation;
using CoreGraphics;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Intents;
using OCR;

namespace RekTec.Corelib.Utils
{
	/// <summary>
	/// 身份证识别
	/// </summary>
	public class OCRVideoUtil : UIViewController
	{
		public UIWebView webView;

		private static AVCaptureSession session;
		private static AVCaptureDeviceInput input;
		private static AVCaptureStillImageOutput output;

		private UILabel borderLeftTop, borderTopLeft, borderRightTop, borderTopRight, borderBottomLeft, borderLeftBottom, borderRightBottom, borderBottomRight;

		UIView layerView;

		static bool isScanning;

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
			StopTimer ();

			UIApplication.SharedApplication.SetStatusBarHidden (false, false);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			UIApplication.SharedApplication.SetStatusBarHidden (true, false);

			#region 强制将页面设置成竖屏
			var value = UIInterfaceOrientation.Portrait;
			UIDevice.CurrentDevice.SetValueForKey ((NSNumber)(int)value, (NSString)"orientation");
			#endregion

			layerView = new UIView (View.Bounds);
			View.AddSubview (layerView);

			SetBorder ();
			SetButtons ();
			SetupCapture ();
		}

		void SetButtons ()
		{
			var rect1 = new CGRect (View.Bounds.Width - 10 - 40, 40, 40, 30);
			var back = new UIButton (rect1);
			back.SetTitle ("返回", UIControlState.Normal);
			back.SetTitleColor (UIColor.White, UIControlState.Normal);
			back.SetTitleColor (UIColor.Gray, UIControlState.Selected);
			back.Layer.CornerRadius = 16;
			back.Font = UIFont.SystemFontOfSize (14F);
			back.BackgroundColor = UIColor.FromRGBA (34, 34, 34, 168);
			back.Transform = CGAffineTransform.MakeRotation ((nfloat)(3.1515926 / 2));
			back.TouchUpInside += (sender, e) => {
				if (!isScanning) {
					session.StopRunning ();
					StopTimer ();

					NavigationController.PopViewController (true);
				}
			};
			layerView.AddSubview (back);

			var rect2 = new CGRect (View.Bounds.Width - 10 - 40, View.Bounds.Height - 100 + 30, 40, 30);
			var handwrite = new UIButton (rect2);
			handwrite.SetTitle ("手录", UIControlState.Normal);
			handwrite.SetTitleColor (UIColor.White, UIControlState.Normal);
			handwrite.SetTitleColor (UIColor.Gray, UIControlState.Selected);
			handwrite.Layer.CornerRadius = 16;
			handwrite.Font = UIFont.SystemFontOfSize (14F);
			handwrite.BackgroundColor = UIColor.FromRGBA (34, 34, 34, 168);
			handwrite.Transform = CGAffineTransform.MakeRotation ((nfloat)(3.1515926 / 2));
			handwrite.TouchUpInside += (sender, e) => {
				if (!isScanning) {
					session.StopRunning ();
					StopTimer ();

					var returnData = new OCRModel {
						Base64Image = "",
						IDCode = "",
						PersonName = ""
					};
					var imageData = JsonConvert.SerializeObject (returnData);
					webView.EvaluateJavascript ("window.XrmOCRData={OCRData:'" + imageData + "',getXrmOCRData:function(){return this.OCRData;},clearOCRData:function(){this.OCRData='';}}");

					NavigationController.PopViewController (false);
				}
			};
			layerView.AddSubview (handwrite);
		}

		/// <summary>
		/// 设置相机
		/// </summary>
		void SetupCapture ()
		{
			// configure the capture session for low resolution, change this if your code
			// can cope with more data or volume
			session = new AVCaptureSession () {
				SessionPreset = AVCaptureSession.PresetMedium
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
					device.FocusMode = AVCaptureFocusMode.AutoFocus;
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

			AutoFetchImageToOCR ();
		}

		/// <summary>
		/// 边框
		/// </summary>
		void SetBorder ()
		{
			var tipString = "请将证件放置在此区域内";//，并尝试对其边框
			var textSize = tipString.StringSize (UIFont.SystemFontOfSize (20F));
			var tip = new UILabel (new CGRect ((View.Bounds.Width - textSize.Width) / 2, (View.Bounds.Height - textSize.Height) / 2, textSize.Width, textSize.Height * 2 + 6));
			tip.TextColor = UIColor.White;
			tip.Text = "请将证件放置在此区域内并尝试对齐边框";
			tip.TextAlignment = UITextAlignment.Center;
			tip.LineBreakMode = UILineBreakMode.CharacterWrap;
			tip.Lines = 0;
			tip.Font = UIFont.SystemFontOfSize (20F);
			tip.Transform = CGAffineTransform.MakeRotation ((nfloat)(3.1415926 / 2));
			layerView.AddSubview (tip);

			var borderLenght = 50;
			borderLeftTop = new UILabel (new CGRect (40, 100, 1, borderLenght)) {
				BackgroundColor = UIColor.Green
			};
			borderTopLeft = new UILabel (new CGRect (40, 100, borderLenght, 1)) {
				BackgroundColor = UIColor.Green
			};
			borderRightTop = new UILabel (new CGRect (View.Bounds.Width - 40, 100, 1, borderLenght)) {
				BackgroundColor = UIColor.Green
			};
			borderTopRight = new UILabel (new CGRect (View.Bounds.Width - 40 - borderLenght, 100, borderLenght, 1)) {
				BackgroundColor = UIColor.Green
			};
			borderBottomLeft = new UILabel (new CGRect (40, View.Bounds.Height - 100, borderLenght, 1)) {
				BackgroundColor = UIColor.Green
			};
			borderLeftBottom = new UILabel (new CGRect (40, View.Bounds.Height - 100 - borderLenght, 1, borderLenght)) {
				BackgroundColor = UIColor.Green
			};
			borderRightBottom = new UILabel (new CGRect (View.Bounds.Width - 40, View.Bounds.Height - 100 - borderLenght, 1, borderLenght)) {
				BackgroundColor = UIColor.Green
			};
			borderBottomRight = new UILabel (new CGRect (View.Bounds.Width - 40 - borderLenght, View.Bounds.Height - 100, borderLenght, 1)) {
				BackgroundColor = UIColor.Green
			};

			var left = new UIView (new CGRect (0, 100, 40, View.Bounds.Height - 200));
			left.BackgroundColor = UIColor.FromRGBA (34, 34, 34, 90);
			layerView.AddSubview (left);
			var right = new UIView (new CGRect (View.Bounds.Width - 40, 100, 40, View.Bounds.Height - 200));
			right.BackgroundColor = UIColor.FromRGBA (34, 34, 34, 90);
			layerView.AddSubview (right);
			var top = new UIView (new CGRect (0, 0, View.Bounds.Width, 100));
			top.BackgroundColor = UIColor.FromRGBA (34, 34, 34, 90);
			layerView.AddSubview (top);
			var bottom = new UIView (new CGRect (0, View.Bounds.Height - 100, View.Bounds.Width, 100));
			bottom.BackgroundColor = UIColor.FromRGBA (34, 34, 34, 90);
			layerView.AddSubview (bottom);

			layerView.AddSubviews (borderLeftTop, borderTopLeft, borderRightTop, borderTopRight, borderBottomLeft, borderLeftBottom, borderRightBottom, borderBottomRight);
		}

		static Timer _syncTimer;

		/// <summary>
		/// 每隔5s就抓取屏幕中指定区域的图片去识别内容
		/// </summary>
		void AutoFetchImageToOCR ()
		{
			session.StartRunning ();
			try {
				_syncTimer = null;
				_syncTimer = new Timer (FetchImage, null, 5 * 1000, 5 * 1000);
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
			}
		}

		private void FetchImage (object state)
		{
			AVCaptureConnection connection = output.ConnectionFromMediaType (AVMediaType.Video);
			output.CaptureStillImageAsynchronously (connection, (imageDataSampleBuffer, error) => {
				if (error == null) {
					StopTimer ();
					NSData imageData = AVCaptureStillImageOutput.JpegStillToNSData (imageDataSampleBuffer);
					UIImage image = UIImage.LoadFromData (imageData);

					session.StopRunning ();

					// 获取指定区域的部分图片
					var imageCg = image.CGImage;
					var subImage = imageCg.WithImageInRect (new CGRect (100 - 20, 40, View.Bounds.Height - 200 + 20, View.Bounds.Width - 80 + 20));
					var newImage = UIImage.FromImage (subImage);
					// OCR识别
					ReadInfoFromImage (newImage);
				}
			});
		}

		void ReadInfoFromImage (UIImage image)
		{
			var ocr = new YMIDCardEngine ((int)OCR_Language.Chinese, "rjidios");
			if (ocr == null) {
				AutoFetchImageToOCR ();
				return;
			}
			var isSuccess = ocr.AllocBImage (image);
			var idCardInfo = ocr.DoBCR;
			if (idCardInfo == null) {
				AutoFetchImageToOCR ();
				return;
			}
			if (isSuccess && idCardInfo.Length == 10
				&& idCardInfo [0].ToString () != string.Empty
				&& idCardInfo [1].ToString () != string.Empty) {
				var base64str = ImageUtil.ConvertImage2Base64String (image, 80, 800);
				var returnData = new OCRModel {
					Base64Image = base64str
				};
				returnData.PersonName = idCardInfo [0].ToString ();
				returnData.IDCode = idCardInfo [1].ToString ();
				var imageData = JsonConvert.SerializeObject (returnData);
				webView.EvaluateJavascript ("window.XrmOCRData={OCRData:'" + imageData + "',getXrmOCRData:function(){return this.OCRData;},clearOCRData:function(){this.OCRData='';}}");
				NavigationController.PopViewController (false);
			} else {
				AutoFetchImageToOCR ();
			}
		}

		/// <summary>
		/// 停止Timer
		/// </summary>
		public static void StopTimer ()
		{
			try {
				if (_syncTimer != null)
					_syncTimer.Dispose ();

				_syncTimer = null;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
			}
		}
	}

	public class OCRModel
	{
		public string Base64Image { get; set; }
		public string PersonName { get; set; }
		public string IDCode { get; set; }
	}

	public class OCRResponse
	{
		public string status { get; set; }
		public OCRData data { get; set; }
	}

	public class OCRData
	{
		public string facade { get; set; }
		public OCRItem item { get; set; }
	}
	public class OCRItem
	{
		public string name { get; set; }
		public string cardno { get; set; }
	}
}
