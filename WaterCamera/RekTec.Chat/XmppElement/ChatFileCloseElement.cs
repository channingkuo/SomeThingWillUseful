using System;
using agsXMPP.Xml.Dom;

namespace RekTec.Chat.XmppElement
{
    public class ChatFileCloseElement:Element
    {
        public static string ElementTagName = "chatfileclose";

        public ChatFileCloseElement()
            : base("chatfileclose")
        {
        }


        public string FileId
        {
            get
            {
                if (HasAttribute("fileid"))
                    return GetAttribute("fileid");
                else
                    return string.Empty;
            }
            set
            {
                SetAttribute("fileid", value);
            }
        }

        public static ChatFileCloseElement Parse(agsXMPP.Xml.Dom.Element e)
        {
            var file = new ChatFileCloseElement();
            file.FileId = e.GetAttribute("fileid");
            return file;
        }
    }
}

