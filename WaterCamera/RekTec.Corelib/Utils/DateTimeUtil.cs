#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-04-16 
说明     : 日期的相关工具类
****************/
#endregion

using System;

namespace RekTec.Corelib.Utils
{
    /// <summary>
    ///日期的相关工具类 
    /// </summary>
    public static class DateTimeUtil
    {
        static readonly System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo("zh-CN");

        /// <summary>
        /// 获取日期的格式化字符串，包含昨天、今天这种中文字符串
        /// </summary>
        /// <param name="dt">待格式化的日期时间</param>
        /// <param name="isShowTime">最终结果是否显示时间</param>
        /// <returns>格式化后的字符串</returns>
        public static string GetDateTimeString(DateTime dt, bool isShowTime)
        {
            var now = DateTime.Now;
            if (dt.Date == DateTime.Now.Date)
                return dt.ToString("HH:mm");
            else if (dt.Date == DateTime.Now.AddDays(-1).Date)
                return "昨天" + (isShowTime ? " " + dt.ToString("HH:mm") : "");
            else if (dt.Date == DateTime.Now.AddDays(-2).Date)
                return "前天" + (isShowTime ? " " + dt.ToString("HH:mm") : "");
            else if ((now.Date - dt.Date).Days < 7) {
                return  Culture.DateTimeFormat.GetDayName(dt.DayOfWeek) + (isShowTime ? " " + dt.ToString("HH:mm") : "");
            } else
                return dt.ToString("yy-MM-dd") + (isShowTime ? " " + dt.ToString("HH:mm") : "");
        }
    }
}

