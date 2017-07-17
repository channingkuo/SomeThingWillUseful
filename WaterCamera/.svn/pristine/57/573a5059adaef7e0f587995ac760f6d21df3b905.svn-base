using System;
using agsXMPP.Xml.Dom;

namespace RekTec.Chat.XmppElement
{
    public class ChatFileOpenElement:Element
    {
        public static string ElementTagName = "chatfileopen";

        public ChatFileOpenElement()
            : base("chatfileopen")
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

        public string FileType
        {
            get
            {
                if (HasAttribute("filetype"))
                    return GetAttribute("filetype");
                else
                    return string.Empty;
            }

            set
            {
                SetAttribute("filetype", value);
            }
        }

        public int PackageCount
        {
            get
            {
                if (HasAttribute("count"))
                    return GetAttributeInt("count");
                else
                    return 0;
            }

            set
            {
                SetAttribute("count", value);
            }
        }

        public static ChatFileOpenElement Parse(agsXMPP.Xml.Dom.Element e)
        {
            var file = new ChatFileOpenElement();
            file.FileId = e.GetAttribute("fileid");
            file.FileType = e.GetAttribute("filetype");
            file.PackageCount = e.GetAttributeInt("count");

            return file;
        }
    }



}
