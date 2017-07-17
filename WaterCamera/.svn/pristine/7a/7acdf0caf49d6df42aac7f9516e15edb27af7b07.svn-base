#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-14
说明 : api接口的封装
****************/
#endregion
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RekTec.Corelib.Rest;
using RekTec.Contacts.ViewModels;

namespace RekTec.Chat.WebApi
{
	/// <summary>
	/// api接口的封装
	/// </summary>
	public static class WebApiFacade
	{
		/// <summary>
		/// 登录接口
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="user">User.</param>
		public static async Task<AuthUser> LoginAsync (UserModel user)
		{
			var apiUrl = "api/Authentication/login";
			var authUser = await RestClient.PostAsync<AuthUser> (apiUrl, user)
                .ConfigureAwait (continueOnCapturedContext: false);
			return authUser;
		}

		/// <summary>
		/// 获取联系人接口
		/// </summary>
		/// <returns>The rosters.</returns>
		/// <param name="timeStamp">最后更新时间</param>
		public static async Task<UiContact> GetRosters (string lastUpdateTime)
		{
			var apiUrl = "api/contact/basic?lastUpdateTime={0}";
			apiUrl = string.Format (apiUrl, lastUpdateTime);
			var rosters = await RestClient.GetAsync<UiContact> (apiUrl)
                .ConfigureAwait (continueOnCapturedContext: false);
			return rosters;
		}

		/// <summary>
		/// 获取联系人头像
		/// </summary>
		/// <returns>The avatars.</returns>
		/// <param name="userArray">User array.</param>
		public static async Task<List<UiRoster>> GetAvatars (string[] userArray)
		{
			var apiUrl = "api/contact/avatar";
			apiUrl = string.Format (apiUrl, userArray);
			var rostersList = await RestClient.PostAsync<List<UiRoster>> (apiUrl, userArray)
				.ConfigureAwait (continueOnCapturedContext: false);
			return rostersList;
		}

		/// <summary>
		/// 服务器端获取菜单
		/// </summary>
		/// <returns>The avatars.</returns>
		/// <param name="userArray">User array.</param>
		public static async Task<List<UiMenu>> GetMenu (string lastUpdateTime)
		{
			var apiUrl = "api/SystemMenu/GetSystemMenusForMobile?lastQueryTime={0}";
			apiUrl = string.Format (apiUrl, lastUpdateTime);
			var menu = await RestClient.GetAsync<List<UiMenu>> (apiUrl)
				.ConfigureAwait (continueOnCapturedContext: false);
			return menu;
		}

		/// <summary>
		/// 获取当前人员信息
		/// </summary>
		/// <returns>The avatars.</returns>
		/// <param name="userArray">User array.</param>
		public static async Task<UserInfoModel> GetCurrentUserInfo ()
		{
			var apiUrl = "api/SystemUser/GetCurrentUserInfo";
			apiUrl = string.Format (apiUrl);
			var userInfo = await RestClient.GetAsync<UserInfoModel> (apiUrl)
				.ConfigureAwait (continueOnCapturedContext: false);
			return userInfo;
		}

		/// <summary>
		/// 获取当前人员头像
		/// </summary>
		/// <returns>The avatars.</returns>
		/// <param name="userArray">User array.</param>
		public static async Task<string> GetCurrentUserImg (string userId)
		{
			var apiUrl = "api/AvatarFile/GetBase64FileContentByObjectId?moduleType=SystemUser&objectid={0}";
			apiUrl = string.Format (apiUrl, userId);
			var avatar = await RestClient.GetAsync (apiUrl)
				.ConfigureAwait (continueOnCapturedContext: false);
			return avatar;
		}

		/// <summary>
		/// 修改头像
		/// </summary>
		/// <returns>The avatar.</returns>
		/// <param name="avatar">Avatar.</param>
		public static async Task UpdateAvatar (UiAvatarModel avatar)
		{
			var apiUrl = "api/AvatarFile/UploadBase64FileContent";
			await RestClient.PostAsync (apiUrl, avatar)
                .ConfigureAwait (continueOnCapturedContext: false);
		}

		/// <summary>
		/// 修改电话号码
		/// </summary>
		/// <returns>The phone number.</returns>
		/// <param name="phoneNumber">Phone number.</param>
		public static async Task UpdatePhoneNumber (UiPhoneModel phoneNumber)
		{
			var apiUrl = "/api/SystemUser/SetUserInfo";
			await RestClient.PostAsync (apiUrl, phoneNumber)
                .ConfigureAwait (continueOnCapturedContext: false);
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <returns>The phone number.</returns>
		/// <param name="phoneNumber">Phone number.</param>
		public static async Task ChangePassword (UiPasswordModel password)
		{
			var apiUrl = "api/Authentication/ChangePassword";
			await RestClient.PostAsync (apiUrl, password)
				.ConfigureAwait (continueOnCapturedContext: false);
		}

		public static async Task<string> UploadChatFile (string id, byte[] fileData)
		{
			var apiUrl = "api/chatfile/";
			return await RestClient.PostFileAsync (apiUrl, id, fileData)
                .ConfigureAwait (continueOnCapturedContext: false);
		}

		public static async Task<byte[]> DownloadChatFile (string fileId)
		{
			var apiUrl = "api/chatfile/?filename=" + fileId;
			return await RestClient.DownloadFileAsync (apiUrl)
                .ConfigureAwait (continueOnCapturedContext: false);
		}
	}
}

