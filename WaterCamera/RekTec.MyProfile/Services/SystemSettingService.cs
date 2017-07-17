#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-07-28
说明     : 系统设置相关的服务
****************/
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RekTec.Corelib.Rest;
using RekTec.Corelib.Utils;
using RekTec.MyProfile.ViewModels;

namespace RekTec.MyProfile.Services
{
	/// <summary>
	/// 系统设置相关的服务
	/// </summary>
	public static class SystemSettingService
	{
		private static Dictionary<string,Action> _cleanupActions = new Dictionary<string,Action> ();

		public static void AddCleanupAction (string name, Action action)
		{
			if (!_cleanupActions.ContainsKey (name)) {
				_cleanupActions.Add (name, action);
			} else {
				_cleanupActions [name] = action;
			}
		}

		public static void Cleanup ()
		{
			RekTec.Corelib.Configuration.GlobalAppSetting.WwwVersion = string.Empty;
			foreach (var key in _cleanupActions.Keys) {
				_cleanupActions [key] ();
			}
		}

		/// <summary>
		/// 获取当前人员信息
		/// </summary>
		public static async Task<bool> TestConnect ()
		{
			try {
				await RestClient.GetAsync ("api/app/connect")
                    .ConfigureAwait (continueOnCapturedContext: false);

				return true;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// 获取当前人员信息
		/// </summary>
		public static async Task SaveSuggest (SuggestionModel m)
		{
			try {
				await RestClient.PostAsync<SuggestionModel> ("api/Setting/save", m)
                    .ConfigureAwait (continueOnCapturedContext: false);

			} catch (Exception ex) {
				ErrorHandlerUtil.ReportException (ex);
			}
		}
	}
}
