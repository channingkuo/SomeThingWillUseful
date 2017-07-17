#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 与通讯录相关的设置项目
****************/
#endregion
using RekTec.Corelib.Configuration;

namespace RekTec.Contacts.Configuration
{
    /// <summary>
    ///与通讯录相关的设置项目 
    /// </summary>
    public static class ContactsAppSetting
    {
        /// <summary>
        /// 联系人上次更新的时间
        /// </summary>
        public static string ContactsLastUpdateTime
        {
            get
            {
                return GlobalAppSetting.GetValue("contacts_" + GlobalAppSetting.LocalDbVersion + "_" + GlobalAppSetting.UserCode + "_ContactsLastUpdateTime");
            }
            set
            {
                GlobalAppSetting.SetValue("contacts_" +  GlobalAppSetting.LocalDbVersion + "_" + GlobalAppSetting.UserCode + "_ContactsLastUpdateTime", value);
            }
        }

    }
}
