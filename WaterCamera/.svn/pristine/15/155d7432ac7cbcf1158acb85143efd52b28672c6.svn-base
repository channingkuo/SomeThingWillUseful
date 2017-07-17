#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 : joesong
日期 : 2015-05-04
说明 : 对话消息的主题正文部分
****************/
#endregion

namespace RekTec.Messages.ViewModels
{
    /// <summary>
    /// 对话消息的主题正文部分
    /// </summary>
    public class MessageBody
    {
        public string type { get; set; }

        public string msg { get; set; }

        public string ext { get; set; }

        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static MessageBody ParseJsonString(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageBody>(json);
        }
    }
}

