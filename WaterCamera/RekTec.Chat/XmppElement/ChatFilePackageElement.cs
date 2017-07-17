using System;
using agsXMPP.Xml.Dom;

namespace RekTec.Chat.XmppElement
{
    public class ChatFilePackageElement:Element
    {
        public static string ElementTagName = "chatfilepackage";

        public ChatFilePackageElement()
            : base("chatfilepackage")
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

        public int Seq
        {
            get
            {
                if (HasAttribute("seq"))
                    return GetAttributeInt("seq");
                else
                    return -1;
            }
            set
            {
                SetAttribute("seq", value);
            }
        }


        public static ChatFilePackageElement Parse(agsXMPP.Xml.Dom.Element e)
        {
            var file = new ChatFilePackageElement();
            file.FileId = e.GetAttribute("fileid");
            file.Seq = e.GetAttributeInt("seq");
            file.Value = e.Value;

            return file;
        }
    }
}

