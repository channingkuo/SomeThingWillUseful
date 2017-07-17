#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-14
说明 : 保存修改头像post请求所要传的model
****************/
#endregion
using System;

namespace RekTec.Chat.WebApi
{
	/// <summary>
	/// 保存修改头像post请求所要传的model
	/// </summary>
	public class UiAvatarModel
	{
		/// <summary>
		/// 当前人员id
		/// </summary>
		public String ObjectId;

		/// <summary>
		/// 文件名称
		/// </summary>
		public String FileName;

		/// <summary>
		/// 文件内容
		/// </summary>
		public String FileBase64Content;

		/// <summary>
		/// The type of the module.
		/// </summary>
		public String moduleType;
	}
}

