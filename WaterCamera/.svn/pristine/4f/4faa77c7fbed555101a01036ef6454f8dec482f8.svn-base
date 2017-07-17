#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Channing Kuo
创建时间 : 2017-03-06
说明    : 身份证驾驶证识别相关的帮助类
****************/
#endregion

using System;
using UIKit;
using Foundation;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace RekTec.Corelib.Utils
{
	/// <summary>
	/// 身份证驾驶证识别相关的帮助类
	/// </summary>
	public static class OCRUtil
	{
		private static UIImagePickerController _picker;
		private static Action<string> _callback;

		static string m_api = "http://www.yunmaiocr.com/SrvXMLAPI";
		static string m_t = "MOBILE";
		static string m_key = "";
		static string m_time = "";
		const string m_loginid = "1ec43863-cad8-469b-bec2-939ec16cb710";
		const string m_loginpwd = "DiRIOOweiHcRoEtyCjZFBejinCsWgT";
		#region 身份证识别
		static string m_action = "idcard.scan";// driver.scan驾照识别
		#endregion

		static string base64image = "";

		static void Init ()
		{
			if (_picker != null)
				return;

			_picker = new UIImagePickerController ();
			_picker.AllowsEditing = true;
			_picker.Delegate = new CameraDelegate ();
		}

		class CameraDelegate : UIImagePickerControllerDelegate
		{
			public override void FinishedPickingMedia (UIImagePickerController picker, NSDictionary info)
			{
				if (info [UIImagePickerController.MediaType].ToString ().ToLower () != "public.image") {
					var cb = _callback;
					_callback = null;

					_picker.DismissViewController (true, (Action)null);
					cb (string.Empty);
				}

				UIImage pickedImage = info [UIImagePickerController.OriginalImage] as UIImage;
				var base64str = ImageUtil.ConvertImage2Base64String (pickedImage, 80, 800);
				byte [] buffer = Convert.FromBase64String (base64str);

				base64image = base64str;

				WebClient webclient = new WebClient ();
				webclient.Headers.Add ("Content-Type", "application/x-www-form-urlencoded");
				webclient.UploadDataCompleted += webclient_UploadDataCompleted;

				DateTime oldTime = new DateTime (1970, 1, 1);
				TimeSpan span = DateTime.Now.Subtract (oldTime);
				long milliSecondsTime = (long)span.TotalMilliseconds;
				m_time = milliSecondsTime.ToString ();

				m_key = Guid.NewGuid ().ToString ();

				MD5 md5 = new MD5CryptoServiceProvider ();
				byte [] md5buf = md5.ComputeHash (Encoding.UTF8.GetBytes (m_action + m_loginid + m_key + m_time + m_loginpwd));

				string verifystr = BitConverter.ToString (md5buf);
				verifystr = verifystr.Replace ("-", "");

				byte [] md5pwd = md5.ComputeHash (Encoding.UTF8.GetBytes (m_loginpwd));
				string password = BitConverter.ToString (md5pwd);
				password = password.Replace ("-", "");

				string filename = Guid.NewGuid ().ToString () + ".jpg";

				int index = 0;
				string poststr = "";
				poststr = String.Format ("<action>{0}</action><client>{1}</client><system>{2}</system><password>{3}</password><key>{4}</key><time>{5}</time><verify>{6}</verify><file>",
					m_action, m_loginid, m_t, password, m_key, m_time, verifystr, filename);
				byte [] firstbuf = Encoding.UTF8.GetBytes (poststr);
				byte [] endbuf = Encoding.UTF8.GetBytes ("</file><ext>jpg</ext><header>1</header><json>1</json>");
				byte [] postbuf = new byte [firstbuf.Length + buffer.Length + endbuf.Length + 1];

				firstbuf.CopyTo (postbuf, index);
				index += firstbuf.Length;
				buffer.CopyTo (postbuf, index);
				index += buffer.Length;
				endbuf.CopyTo (postbuf, index);
				webclient.UploadDataAsync (new Uri (m_api), "POST", postbuf);
				AlertUtil.ShowWaiting ("正在识别证件内容...");
			}

			void webclient_UploadDataCompleted (object sender, UploadDataCompletedEventArgs e)
			{
				var cb = _callback;
				var returnData = new OCRModel1 {
					Base64Image = base64image
				};

				_callback = null;
				string returnMessage = string.Empty;
				try {
					if (e.Error == null) {
						returnMessage = Encoding.UTF8.GetString (e.Result);
						var jsonData = JsonConvert.DeserializeObject<OCR1> (returnMessage);
						if (jsonData != null && jsonData.data != null && jsonData.data.item != null) {
							returnData.PersonName = jsonData.data.item.name;
							returnData.IDCode = jsonData.data.item.cardno;
						}
					}
				} catch {
					AlertUtil.DismissWaiting ();
					AlertUtil.Error ("证件识别失败！");
					returnData.Base64Image = "";
				} finally {
					AlertUtil.DismissWaiting ();
					_picker.DismissViewController (true, (Action)null);
					cb (JsonConvert.SerializeObject (returnData));
				}

			}
		}

		/// <summary>
		/// 打开手机的摄像头拍照
		/// </summary>
		/// <param name="parent">使用拍照功能的ViewController</param>
		/// <param name="callback">拍照成功后的回调函数</param>
		/// <param name="OCR">证件类型</param>
		public static void TakePicture (UIViewController parent, Action<string> callback, string OCR = "IdCard")
		{
			Init ();
			_picker.AllowsEditing = false;
			if (OCR == "IDCard") {
				m_action = "idcard.scan";
			} else if (OCR == "DriverLisence") {
				m_action = "driver.scan";
			}
			_picker.SourceType = UIImagePickerControllerSourceType.Camera;
			_callback = callback;
			parent.PresentViewController ((UIViewController)_picker, true, (Action)null);
		}

		/// <summary>
		/// 打开手机的图库选择照片
		/// </summary>
		/// <param name="parent">使用拍照功能的ViewController</param>
		/// <param name="callback">拍照成功后的回调函数</param>
		/// <param name="OCR">证件类型</param>
		public static void SelectPicture (UIViewController parent, Action<string> callback, string OCR = "IdCard")
		{
			Init ();
			_picker.AllowsEditing = false;
			if (OCR == "IDCard") {
				m_action = "idcard.scan";
			} else if (OCR == "DriverLisence") {
				m_action = "driver.scan";
			}
			_picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			_callback = callback;
			parent.PresentViewController ((UIViewController)_picker, true, (Action)null);
		}
	}

	public class OCRModel1
	{
		public string Base64Image { get; set; }
		public string PersonName { get; set; }
		public string IDCode { get; set; }
	}

	public class OCR1
	{
		public string status { get; set; }
		public OCRData1 data { get; set; }
	}

	public class OCRData1
	{
		public string facade { get; set; }
		public OCRItem1 item { get; set; }
	}
	public class OCRItem1
	{
		public string name { get; set; }
		public string cardno { get; set; }
	}
}

