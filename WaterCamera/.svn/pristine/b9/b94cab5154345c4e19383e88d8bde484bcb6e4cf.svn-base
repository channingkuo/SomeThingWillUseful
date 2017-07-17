//
// MultilineEntryElement.cs: multi-line element entry for MonoTouch.Dialog
// 
// Author:
// Aleksander Heintz (alxandr@alxandr.me)
// Based on the code for the EntryElement by Miguel de Icaza
//

using System;
using System.Drawing;
using CoreGraphics;
using MonoTouch.Dialog;
using UIKit;

namespace RekTec.Corelib.Views
{
    public class MultilineEntryElement : UIViewElement
    {
        public MultilineEntryElement(string caption, string placeholder, string value)
            : this(caption, placeholder, value, 200)
        {
        }

        public MultilineEntryElement(string caption, string placeholder, string value, float height)
            : base(caption, CreateUIView(caption, placeholder, value, height), false)
        {
        }

        static UIView CreateUIView(string caption, string placeholder, string value, float height)
        {
            const float containerWidth = 414;

            // create a container
            UIView c = new UIView(new RectangleF(3, 2, containerWidth, height)) {BackgroundColor = UIColor.Clear};

            // calculate width of caption
            nfloat captionWidth = caption.StringSize(UIFont.BoldSystemFontOfSize(16)).Width;

            // create placeholder
            UILabel ph = new UILabel(new CGRect(6 + captionWidth, 2, containerWidth - captionWidth - 6, 30))
            {
                BackgroundColor = UIColor.Clear,
                TextColor = UIColor.FromRGB(190, 190, 190),
                Text = placeholder,
                ShadowColor = UIColor.FromRGB(230, 230, 230)
            };
            c.AddSubview(ph);

            // create actual text view
            UITextView v = new UITextView(new CGRect(0 + captionWidth, 0, containerWidth - captionWidth, height - 12))
            {
                Text = value ?? "",
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear,
                Font = UIFont.SystemFontOfSize(16),
                AutocapitalizationType = UITextAutocapitalizationType.None,
                KeyboardType = UIKeyboardType.EmailAddress,
                AutocorrectionType = UITextAutocorrectionType.No
            };
            v.Changed += (sender, e) =>
            {
                ph.Hidden = !string.IsNullOrEmpty(v.Text);
            };
            c.AddSubview(v);

            return c;
        }

        public override string Summary()
        {
            UITextView tv = View.Subviews[1] as UITextView;
            if (tv != null)
                return tv.Text;
            return "";
        }
    }
}

