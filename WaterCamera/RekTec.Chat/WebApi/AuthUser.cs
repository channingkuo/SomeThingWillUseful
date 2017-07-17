#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-07
说明 : 登录返回的对象
****************/
#endregion
using System;

namespace RekTec.Chat.WebApi
{
	/// <summary>
	/// 登录返回的对象
	/// </summary>
	public class AuthUser
	{
		/// <summary>
		/// token
		/// </summary>
		/// <value>The auth token.</value>
		public string AuthToken { get; set; }

		/// <summary>
		/// 用户类型
		/// </summary>
		/// <value>The type of the auth.</value>
		public int AuthType{ get; set; }

		/// <summary>
		/// 友好名称
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string FriendlyName { get; set; }

		/// <summary>
		/// 是否首次登录
		/// </summary>
		/// <value>The name of the friendly.</value>
		public Boolean IsLoginFirst { get; set; }

		/// <summary>
		/// 用户id
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string SystemUserId { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		/// <value>The name of the friendly.</value>
		public string UserName { get; set; }

		/// <summary>
		/// 用户编码
		/// </summary>
		/// <value>The user code.</value>
		public string UserCode{ get; set; }
	}
}

