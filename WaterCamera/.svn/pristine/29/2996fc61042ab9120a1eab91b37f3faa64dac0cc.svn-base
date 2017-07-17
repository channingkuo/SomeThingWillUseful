#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-07
说明 : 登录请求所要传入的对象
****************/
#endregion

using System;
using UIKit;
using RekTec.Version.Services;

namespace RekTec.MyProfile.ViewModels
{
	public class UserModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		/// <value>The type of the auth.</value>
		public string uid { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		/// <value>The auth token.</value>
		public string pwd { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		/// <value>The auth token.</value>
		public string pluginKey { get { return "LoginLog"; } }

		/// <summary>
		/// 密码
		/// </summary>
		/// <value>The auth token.</value>
		public string pluginContent { get; set; }

		public string checkCode { get; set; }

		public string verifyStr { get; set; }
	}

	public class LoginLogModel
	{
		public string LoginLogId { get { return Guid.NewGuid ().ToString (); } }

		public string OsVersion { get { return UIDevice.CurrentDevice.SystemVersion; } }

		public string AppVersion { get { return VersionService.RawAppVersion; } }

		public string Html5Version { get { return VersionService.WwwAppVersion; } }

		public int ClientType { get { return 2; } }
	}
}

