#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-08
说明 : 通讯录接口返回的数据模型
****************/
#endregion
using System.Collections.Generic;

namespace RekTec.Contacts.ViewModels
{
    /// <summary>
    /// 联系人同步
    /// </summary>
    public class UiContact
    {
        /// <summary>
        /// 本次同步的时间
        /// </summary>
        public string SysnTime { get; set; }

        /// <summary>
        /// 联系人列表
        /// </summary>
        public List<UiRoster> Contacts { get; set; }
    }

    public class UiRoster
    {
		/// <summary>
		/// 用户id
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }

		/// <summary>
		/// 用户拼音
		/// </summary>
		/// <value>The pin yin.</value>
        public string PinYin { get; set; }

		/// <summary>
		/// 用户头像
		/// </summary>
		/// <value>The avatar.</value>
        public string Avatar { get; set; }

		/// <summary>
		///  用户编码
		/// </summary>
		/// <value>The code.</value>
		public string Code{ get; set;}

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		/// <value><c>true</c> if this instance is disabled; otherwise, <c>false</c>.</value>
		public bool IsDisabled{ get; set;}
    }
}