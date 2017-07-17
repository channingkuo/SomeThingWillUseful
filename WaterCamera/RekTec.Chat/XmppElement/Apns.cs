using System;
using agsXMPP.Xml.Dom;

namespace RekTec.Chat.XmppElement
{
    public class Apns : Element
    {
        public Apns()
        {
            this.TagName = "query";
            this.Namespace = "urn:xmpp:apns";
        }

        public string Token {
            get{ return GetTag("token"); }
            set{ SetTag("token", value); }
        }
    }
}
