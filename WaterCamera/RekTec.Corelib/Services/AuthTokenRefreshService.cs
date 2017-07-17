#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Channing Kuo
创建时间 : 2017-02-21
说明     : SSO Token刷新
****************/
#endregion

using System;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using System.Threading;
using RekTec.Corelib.Views;
using System.Threading.Tasks;
using RekTec.Corelib.Rest;

namespace RekTec.MyProfile.Services
{
	/// <summary>
	/// SSO Token 定时刷新Service
	/// </summary>
	public static class AuthTokenRefreshService
	{
		private const int _syncTimeSpan = 7200 * 1000 - 30 * 1000;
		private const string APP_KEY = "app_key_test_K5A7fvk";
		private const string APP_SECRET = "app_secret_test_5Nh31wk";
		private const string ADDR = "connect/token";

		/// <summary>
		/// 刷新SSO Token
		/// </summary>
		private static async Task RefreshSSOToken ()
		{
			var apiUrl = "api/homeinnssso/refresh?refreshtoken=" + GlobalAppSetting.ReFreshToken;
			var refresh = await RestClient.GetAsync<ResponseData> (apiUrl).ConfigureAwait (continueOnCapturedContext: false);

			// 更新SSO Token
			if (refresh != null && refresh.RefreshToken != null && refresh.AccessToken != null && refresh.ExpiresIn != 0) {
				GlobalAppSetting.SsoToken = refresh.AccessToken;
				GlobalAppSetting.ReFreshToken = refresh.RefreshToken;
				GlobalAppSetting.SsoTokenExpiredTime = refresh.ExpiresIn;
				WebViewController.RefreshSSOToken ();
			}
		}

		static Timer _syncTimer;

		/// <summary>
		/// 开始刷新SSO Token
		/// </summary>
		public static void StartRefreshToken ()
		{
			try {
				_syncTimer = null;
				_syncTimer = new Timer (SyncRefreshTokenTimerCallback, null, _syncTimeSpan, 7200 * 1000);
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
			}
		}

		/// <summary>
		/// 停止刷新SSO Token
		/// </summary>
		public static void StopRefreshToken ()
		{
			try {
				if (_syncTimer != null)
					_syncTimer.Dispose ();

				_syncTimer = null;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
			}
		}

		private static void SyncRefreshTokenTimerCallback (object state)
		{
			var t = RefreshSSOToken ();
			t.Wait ();
		}
	}

	public class ResponseData
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public int ExpiresIn { get; set; }
		public string TokenType { get; set; }
	}
}
