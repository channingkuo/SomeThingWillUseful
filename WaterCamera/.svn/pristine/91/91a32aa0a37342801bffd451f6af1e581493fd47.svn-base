#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-23 
说明     : 根据服务器端传入的数组的Type，转换为TypeName
****************/
#endregion

using System.Collections.Generic;

namespace RekTec.Messages.Services
{
    /// <summary>
    ///根据服务器端传入的数组的Type，转换为TypeName 
    /// </summary>
    public static class NotifactionTypeName
    {
        private static readonly Dictionary<string, string> TypeNameDictionary;

        static NotifactionTypeName()
        {
            TypeNameDictionary = new Dictionary<string, string> {
                { "1","公告通知" },
                { "2","待办事项" },
                { "3","工作提醒" }
            };
        }

        /// <summary>
        /// 根据推送的消息类型获取类型的名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(string type)
        {
            if (!TypeNameDictionary.ContainsKey(type))
                return null;

            return TypeNameDictionary[type];
        }

    }
}