#region 类文件描述
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
创建人   : Joe Song
创建时间 : 2015-07-20 
说明     : 用户的默认设定文件，包括登录名，服务器地址等
****************/
#endregion

using System.Collections.Generic;
using System.IO;
using RekTec.Corelib.Utils;

namespace RekTec.Corelib.Configuration
{
    /// <summary>
    /// 用户的默认设定文件，包括登录名，服务器地址等
    /// </summary>
	public class AppSettingStorage
    {
        private const string _fileName = "AppSetting.config";
        private static readonly Dictionary<string,string> _configKeyValues = new Dictionary<string, string>();
        private static readonly object ReadWriteLocker = new object();

        /// <summary>
        /// 设置Key/Value格式的配置项目
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void SetValue(string key, string value)
        {
            lock (ReadWriteLocker)
            {
                if(!_configKeyValues.ContainsKey((key)))
                    _configKeyValues.Add(key,value);
                else
                {
                    _configKeyValues[key] = value;
                }

                string path = FileSystemUtil.CachesFolder;
                string fullFilePath = Path.Combine(path, _fileName);
                using (StreamWriter sw = new StreamWriter(fullFilePath,false))
                {
                    foreach (var configKey in _configKeyValues.Keys)
                    {
                        sw.WriteLine(configKey + "=" + _configKeyValues[configKey]);
                    }
                }
            }
        }

        /// <summary>
        /// 根据Key获取设置项目的值
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public static string GetValue(string key)
        {
            lock (ReadWriteLocker)
            {
                string path = FileSystemUtil.CachesFolder;
                string fullFilePath = Path.Combine(path, _fileName);
                _configKeyValues.Clear();
                if (!File.Exists(fullFilePath))
                {
                    return string.Empty;
                }

                using (StreamReader reader = new StreamReader(fullFilePath))
                {
                    var lineString = reader.ReadLine();
                    while (!string.IsNullOrWhiteSpace(lineString))
                    {
                        var keyValues = lineString.Split('=');
                        if (keyValues.Length == 2)
                        {
                            if (!_configKeyValues.ContainsKey(keyValues[0]))
                            {
                                _configKeyValues.Add(keyValues[0], keyValues[1]);
                            }
                        }
                        lineString = reader.ReadLine();
                    }
                }

                if (!_configKeyValues.ContainsKey(key))
                {
                    return string.Empty;
                }

                return _configKeyValues[key];
            }
        }
    }
}
