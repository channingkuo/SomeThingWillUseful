#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-30
说明     : 用户信息相关操作的类
****************/
#endregion

using System;
using System.Threading.Tasks;
using RekTec.Corelib.Rest;
using RekTec.Corelib.Utils;
using RekTec.MyProfile.ViewModels;

namespace RekTec.MyProfile.Services
{
	/// <summary>
	/// 用户信息相关操作的类
	/// </summary>
	public static class UserService
	{
		/// <summary>
		/// 获取当前人员信息
		/// </summary>
		public static async Task<UserInfoViewModel> GetCurrentUserInfo ()
		{
			try {
				var apiUrl = "api/SystemUser/GetCurrentUserInfo";
				apiUrl = string.Format (apiUrl);
				var userInfo = await RestClient.GetAsync<UserInfoViewModel> (apiUrl)
                    .ConfigureAwait (continueOnCapturedContext: false);
				return userInfo;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return new UserInfoViewModel ();
			}
           
		}

		/// <summary>
		/// 修改电话号码
		/// </summary>
		/// <returns>The phone number.</returns>
		/// <param name="phoneNumber">Phone number.</param>
		public static async Task UpdatePhoneNumber (UserPhoneViewModel phoneNumber)
		{
			try {
				var apiUrl = "/api/SystemUser/SetUserInfo";
				await RestClient.PostAsync (apiUrl, phoneNumber)
                    .ConfigureAwait (continueOnCapturedContext: false);
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);   
			}
           
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <returns>The phone number.</returns>
		public static async Task<bool> ChangePassword (UserPasswordViewModel password)
		{
			try {
				var apiUrl = "api/Authentication/ChangePassword2";
				await RestClient.PostAsync (apiUrl, password)
                    .ConfigureAwait (continueOnCapturedContext: false);
				return true;
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return false;
			}
            
		}

		/// <summary>
		/// 获取当前人员头像
		/// </summary>
		/// <returns>The avatars.</returns>
		public static async Task<string> GetCurrentUserAvatar (string userId)
		{
			try {
				var apiUrl = "api/AvatarFile/GetBase64FileContentByObjectId?moduleType=SystemUser&objectid={0}";
				apiUrl = string.Format (apiUrl, userId);
				var avatar = await RestClient.GetAsync (apiUrl)
                    .ConfigureAwait (continueOnCapturedContext: false);
				return avatar;

			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
				return null;
			}
		}


		/// <summary>
		/// 修改头像
		/// </summary>
		/// <returns>The avatar.</returns>
		/// <param name="avatar">Avatar.</param>
		public static async Task UpdateAvatar (UserAvatarViewModel avatar)
		{
			try {
				var apiUrl = "api/AvatarFile/UploadBase64FileContent";
				await RestClient.PostAsync (apiUrl, avatar)
                    .ConfigureAwait (continueOnCapturedContext: false);
			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);   
			}
		}
	}
}
