using System;

namespace RekTec.Chat.XmppElement
{
    internal class XmppMessageBody
    {
        public string type { get; set; }

        public string msg { get; set; }

        public string ext { get; set; }

        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static XmppMessageBody ParseJsonString(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<XmppMessageBody>(json);
        }
    }
}

