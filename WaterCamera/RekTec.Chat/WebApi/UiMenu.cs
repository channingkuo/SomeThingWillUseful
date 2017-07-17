#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-09
说明 : 服务器端返回菜单数据模型
****************/
#endregion
using System;
using System.Collections.Generic;

namespace RekTec.Chat.WebApi
{
	/// <summary>
	/// 服务器端返回菜单数据模型
	/// </summary>
	public class UiMenu
	{
		public int IsActive{ get; set;}

		public string MenuCode{ get; set;}

		public string MenuIcon{ get; set;}

		public string MenuName { get; set;}

		public int MenuSeq{ get; set;}

		public string MenuType{ get; set;}

		public string MenuUrl{ get; set;}

		public string ParentMenuCode{get;set;}

		public string ParentMenuId{ get; set;}

		public string SystemMenuId{ get; set;}

		public List<MenuModel> Children{ get; set;}
	}

	public class MenuModel
	{
		public int IsActive{ get; set;}

		public string MenuCode{ get; set;}

		public string MenuIcon{ get; set;}

		public string MenuName { get; set;}

		public int MenuSeq{ get; set;}

		public string MenuType{ get; set;}

		public string MenuUrl{ get; set;}

		public string ParentMenuCode{get;set;}

		public string ParentMenuId{ get; set;}

		public string SystemMenuId{ get; set;}

	}
}

