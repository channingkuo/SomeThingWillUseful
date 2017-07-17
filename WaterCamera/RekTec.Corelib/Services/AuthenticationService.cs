#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-30
说明     : 登陆进行身份验证的服务
****************/
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.DataRepository;
using RekTec.Corelib.Rest;
using RekTec.Corelib.Utils;
using RekTec.MyProfile.ViewModels;
using RekTec.Version.Services;
using RestSharp.Serializers;

namespace RekTec.MyProfile.Services
{
	/// <summary>
	/// 登陆进行身份验证的服务
	/// </summary>
	public static class AuthenticationService
	{
		private static UserInfoViewModel _currentUserInfo;
		private static Dictionary<string, Action> _logoutActions = new Dictionary<string, Action> ();

		public static UserInfoViewModel CurrentUserInfo {
			get { return _currentUserInfo ?? (_currentUserInfo = new UserInfoViewModel ()); }
		}

		/// <summary>
		/// Check the type of PMS.
		/// </summary>
		/// <returns>The PMS type.</returns>
		/// <param name="hotelCode">HotelCode.</param>
		public static async Task<string> CheckPMSType (string hotelCode)
		{
			try {
				var apiUrl = "api/cityHome/GetIsUserNewPmsInfo?HotelCode=" + hotelCode;
				var PMSType = await RestClient.GetAsync (apiUrl).ConfigureAwait (continueOnCapturedContext: false);
				//var PMSType = await RestClient.GetAsync<IsPMSType> (apiUrl).ConfigureAwait (continueOnCapturedContext: false);;
				return PMSType;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return null;
			}
		}

		/// <summary>
		/// 登录接口
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="user">User.</param>
		public static async Task<AuthUser> LoginAsync (UserModel user)
		{
			try {
				user.pluginContent = (new JsonSerializer ()).Serialize (new LoginLogModel ());
				var apiUrl = "api/Authentication/login";
				var authUser = await RestClient.PostAsync<AuthUser> (apiUrl, user)
					.ConfigureAwait (continueOnCapturedContext: false);

				CurrentUserInfo.UserName = authUser.UserName;
				CurrentUserInfo.UserCode = authUser.UserCode;
				CurrentUserInfo.SystemUserId = authUser.SystemUserId;
				return authUser;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return null;
			}
		}

		public static async Task<List<string>> GetCheckCode (long seed)
		{
			try {
				var api = "api/Authentication/GetCheckCode?seed=" + seed;
				var checkCode = await RestClient.GetAsync<List<string>> (api)
					.ConfigureAwait (continueOnCapturedContext: false);
				if (checkCode.Count == 3)
					return checkCode;
				return null;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return null;
			}
		}

		/// <summary>
		/// Gets the hotel info.
		/// </summary>
		/// <returns>The hotel info.</returns>
		/// <param name="userName">User name.</param>
		public static async Task<HotelCode> GetHotelInfo (string userName)
		{
			try {
				var apiUrl = "api/v1/users/" + userName + "/hotels";
				var hotelInfo = await RestClientForHotel.GetAsync<HotelCode> (apiUrl)
												.ConfigureAwait (continueOnCapturedContext: false);
				if (hotelInfo != null && hotelInfo.IsError == "False" && hotelInfo.Data != null
					&& hotelInfo.Data.Hotels != null && hotelInfo.Data.Hotels.Count >= 1) {
					//GlobalAppSetting.HotelCD = hotelInfo.Data.UserMappingHotel [0].HotelCd;
					//GlobalAppSetting.HotelName = hotelInfo.Data.UserMappingHotel [0].HotelName;
					//GlobalAppSetting.BrandCd = hotelInfo.Data.UserMappingHotel [0].BrandCd;
					GlobalAppSetting.IsMultiHotel = hotelInfo.Data.Hotels.Count > 1;
					// YYJL 运营经理   QTJL  前厅经理
					if (hotelInfo.Data.JobID == "YYJL" || hotelInfo.Data.JobID == "QTJL") {
						GlobalAppSetting.IsRunOrVestibuleManager = true;
					}
					return hotelInfo;
				}
				GlobalAppSetting.IsMultiHotel = false;
				GlobalAppSetting.IsRunOrVestibuleManager = false;
				return null;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return null;
			}
		}

		/// <summary>
		/// Gets the chat service info.
		/// </summary>
		/// <returns>The chat service info.</returns>
		public static async Task<ChatConfigModel> GetChatServiceInfo ()
		{
			try {
				var apiUrl = "api/chat/config";
				var chatConfigModel = await RestClient.GetAsync<ChatConfigModel> (apiUrl)
					.ConfigureAwait (continueOnCapturedContext: false);
				return chatConfigModel;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return null;
			}

		}

		/// <summary>
		/// Checks the type of the the user.
		/// </summary>
		/// <returns>The the user type.</returns>
		/// <param name="userCode">User code.</param>
		public static async Task<string> CheckTheUserType (string userCode)
		{
			try {
				var apiUrl = "api/QCSystemUser/GetUserType?userName=" + userCode;
				var type = await RestClient.GetAsync (apiUrl).ConfigureAwait (continueOnCapturedContext: false);
				return type;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return "-1";
			}

		}

		public static Action BeginConnecting;
		public static Action EndConnecting;

		public static void Initialize (string webApiBaseUrl, string authToken, string userName, string password)
		{
			GlobalAppSetting.XrmWebApiBaseUrl = webApiBaseUrl;
			GlobalAppSetting.XrmAuthToken = authToken;
			GlobalAppSetting.UserCode = userName;
			GlobalAppSetting.Password = password;
		}

		#region 登录与登出相关

		private static bool _isLogOn { get; set; }

		public static bool IsLogOn ()
		{
			return _isLogOn;
		}

		public static async Task<bool> Logon ()
		{
			_isLogOn = true;

			//初始化本地sqlite数据库
			await Task.Run (() => SqlDataRepository.Initialize (GlobalAppSetting.UserCode));

			return true;
		}

		public static void AddLogoutAction (string name, Action action)
		{
			if (!_logoutActions.ContainsKey (name)) {
				_logoutActions.Add (name, action);
			} else {
				_logoutActions [name] = action;
			}
		}

		public static void Logout ()
		{
			_isLogOn = false;
			foreach (var key in _logoutActions.Keys) {
				_logoutActions [key] ();
			}
		}

		#endregion
	}
}
