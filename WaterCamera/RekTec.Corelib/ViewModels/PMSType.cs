#region 文件头部
/**********
Copyright @ Homeinns All rights reserved. 
****************
作者 :xxpang
日期 : 2017-05-11
说明 : 验证PMS类型返回的对象
****************/
#endregion

using System;

namespace RekTec.MyProfile.ViewModels
{
    /// <summary>
    /// 登录返回的对象
    /// </summary>
    public class IsPMSType
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The auth token.</value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <value>The type of the auth.</value>
        public string Message{ get; set; }

        /// <summary>
        /// PMS类型
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string LabelCd { get; set; }

    }
}

