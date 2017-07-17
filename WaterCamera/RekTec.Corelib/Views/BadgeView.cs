#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 : Joe Song
日期 : 2015-04-14
说明 : 显示未读消息条数的View
****************/
#endregion
using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace RekTec.Corelib.Views
{
    /// <summary>
    /// 显示未读消息条数的View
    /// </summary>
    public class BadgeView : UIView
    {
        private string text;
        private UIColor textColor;
        private UIColor badgeColor;
        private UIColor borderColor;
        private bool border;
        private bool shining;
        private bool autoResize;
        private float cornerRoundness;
        private float scaleFactor;

        public BadgeView(string text)
            : this(text, true)
        {
        }

        public BadgeView(string text, bool autoResize)
            : base(new CGRect(0, 0, 25, 25))
        {
            this.ContentScaleFactor = (float)UIScreen.MainScreen.Scale;
            this.BackgroundColor = UIColor.Clear;
            this.text = text;
            this.textColor = UIColor.White;
            this.border = false;
            this.borderColor = UIColor.Clear;
            this.badgeColor = UIColor.Red;
            this.cornerRoundness = 0.4f;
            this.scaleFactor = 1.0f;
            this.shining = false;
            this.autoResize = autoResize;
            if (this.autoResize)
                this.AutoResizeBadge();
            base.UserInteractionEnabled = false;
        }

        public string Text {
            get {
                return text;
            }
            set {
                if (text != value) {
                    text = value;
                    Redraw();
                }
            }
        }

        public UIColor TextColor {
            get {
                return textColor;
            }
            set {
                if (textColor != value) {
                    textColor = value;
                    Redraw(false);
                }
            }
        }

        public UIColor BadgeColor {
            get {
                return badgeColor;
            }
            set {
                if (badgeColor != value) {
                    badgeColor = value;
                    Redraw(false);
                }
            }
        }

        public UIColor BorderColor {
            get {
                return borderColor;
            }
            set {
                if (borderColor != value) {
                    borderColor = value;
                    Redraw(false);
                }
            }
        }

        public bool Border {
            get {
                return border;
            }
            set {
                if (border != value) {
                    border = value;
                    Redraw(false);
                }
            }
        }

        public bool Shining {
            get {
                return shining;
            }
            set {
                if (shining != value) {
                    shining = value;
                    Redraw(false);
                }
            }
        }

        public bool AutoResize {
            get {
                return autoResize;
            }
            set {
                autoResize = value;
            }
        }

        public float CornerRoundness {
            get {
                return cornerRoundness;
            }
            set {
                if (cornerRoundness != value) {
                    cornerRoundness = value;
                    Redraw();
                }
            }
        }

        public float ScaleFactor {
            get {
                return scaleFactor;
            }
            set {
                if (scaleFactor != value) {
                    scaleFactor = value;
                    Redraw();
                }
            }
        }

        private void Redraw(bool autoResize = true)
        {
            if (autoResize && this.autoResize) {
                AutoResizeBadge();
                return; // AutoResize calls redraw
            }
            this.SetNeedsDisplay(); 
        }

        public void AutoResizeBadge()
        {
            var stringSize = new NSString(this.text).StringSize(UIFont.BoldSystemFontOfSize((nfloat)12));
            nfloat flexSpace, rectWidth, rectHeight;
            var frame = (CGRect)this.Frame;
            if (this.text.Length >= 2) {
                flexSpace = this.text.Length;
                rectWidth = 25 + (stringSize.Width + flexSpace);
                rectHeight = 25;
                frame.Width = rectWidth * this.scaleFactor;
                frame.Height = rectHeight * this.scaleFactor;
            } else {
                frame.Width = 25 * this.scaleFactor;
                frame.Height = 25 * this.scaleFactor;
            }
            this.Frame = frame;
            this.Redraw(false);
        }

        public override void Draw(CGRect rect)
        {
            var context = UIGraphics.GetCurrentContext();
            this.DrawRoundedRect(context, (CGRect)rect);
			
            if (this.shining)
                DrawShine(context, (CGRect)rect);
			
            if (this.border)
                DrawBorder(context, (CGRect)rect);
			
            if (this.text.Length > 0) {
                textColor.SetColor();
                var sizeOfFont = 12f * scaleFactor;
                if (this.text.Length < 2) {
                    sizeOfFont += sizeOfFont * 0.20f;
                }
                var font = UIFont.BoldSystemFontOfSize((nfloat)sizeOfFont);
                var text = new NSString(this.text);
                var textSize = text.StringSize(font);
                var textPos = new CGPoint(rect.Width / 2 - textSize.Width / 2, rect.Height / 2 - textSize.Height / 2);
                if (this.text.Length < 2)
                    textPos.X += 0.5f;
                text.DrawString(textPos, font);
            }
        }

        private nfloat MakePath(CGContext context, CGRect rect)
        {
            var radius = rect.Bottom * this.cornerRoundness;
            var puffer = rect.Bottom * 0.12f;
            var maxX = rect.Right - (puffer * 2f);
            var maxY = rect.Bottom - puffer;
            var minX = rect.Left + (puffer * 2f);
            var minY = rect.Top + puffer;
            if (maxX - minX < 20f) {
                maxX = rect.Right - puffer;
                minX = rect.Left + puffer;
            }
			
            context.AddArc((nfloat)maxX - radius, (nfloat)minY + radius, (nfloat)radius, (nfloat)(float)(Math.PI + (Math.PI / 2)), (nfloat)0f, false);
            context.AddArc((nfloat)maxX - radius, (nfloat)maxY - radius, (nfloat)radius, (nfloat)0, (nfloat)(float)(Math.PI / 2), false);
            context.AddArc((nfloat)minX + radius, (nfloat)maxY - radius, (nfloat)radius, (nfloat)(float)(Math.PI / 2), (nfloat)(float)Math.PI, false);
            context.AddArc((nfloat)minX + radius, (nfloat)minY + radius, (nfloat)radius, (nfloat)(float)Math.PI, (nfloat)(float)(Math.PI + Math.PI / 2), false);
			
            return maxY;
        }

        private void DrawRoundedRect(CGContext context, CGRect rect)
        {
            context.SaveState();
			
            context.BeginPath();
            context.SetFillColor(badgeColor.CGColor);
            MakePath(context, rect);
            //context.SetShadow(new CGSize(1.0f, 1.0f), 3, UIColor.Black.CGColor);
            context.FillPath();
			
            context.RestoreState();
        }

        private void DrawShine(CGContext context, CGRect rect)
        {
            context.SaveState();
			
            context.BeginPath();
            var maxY = MakePath(context, rect);
            context.Clip();
			
            /*var locations = new float[] { 0.0f, 0.5f };
			var components = new float[] { 0.92f, 0.92f, 0.92f, 1.0f, 0.82f, 0.82f, 0.82f, 0.4f };*/
			
            var locations = new nfloat[] { 0f, 0.4f, 0.5f, 0.5f, 0.6f, 1.0f };
            var colors = new UIColor[] { 
                UIColor.FromWhiteAlpha((nfloat)1.0f, (nfloat)0.885f),
                UIColor.FromWhiteAlpha((nfloat)1.0f, (nfloat)0.45f),
                UIColor.FromWhiteAlpha((nfloat)1.0f, (nfloat)0.23f),
                UIColor.FromWhiteAlpha((nfloat)1.0f, (nfloat)0.10f),
                UIColor.FromRGBA((nfloat)0f, (nfloat)0f, (nfloat)0f, (nfloat)0.13f),
                UIColor.FromRGBA((nfloat)0f, (nfloat)0f, (nfloat)0f, (nfloat)0.13f)
            };
            var components = GetComponents(colors);
			
            var darkLoc = new nfloat[] { 0.5f, 1.0f };
            var darkComp = new nfloat[] { 0.08f, 0.08f, 0.08f, 0.6f, 0.18f, 0.18f, 0.18f, 0.2f };
			
            using (var cspace = CGColorSpace.CreateDeviceRGB())
            using (var darkGrad = new CGGradient(cspace, darkComp, darkLoc))
            using (var gradient = new CGGradient(cspace, components, locations)) {
		
                CGPoint sPoint = new CGPoint(0f, 0f),
                ePoint = new CGPoint(0f, maxY);
                context.DrawLinearGradient((CGGradient)gradient, (CGPoint)sPoint, (CGPoint)ePoint, (CGGradientDrawingOptions)0);
            }
			
            context.RestoreState();
        }

        private nfloat[] GetComponents(UIColor[] colors)
        {
            var ret = new nfloat[colors.Length * 4];
            for (int i = 0; i < colors.Length; i++) {
                var f = i * 4;
                colors[i].GetRGBA(out ret[f], out ret[f + 1], out ret[f + 2], out ret[f + 3]);
            }
            return ret;
        }

        private void DrawBorder(CGContext context, CGRect rect)
        {
            context.SaveState();
			
            context.BeginPath();
            float lineSize = 2f;
            if (this.scaleFactor > 1) {
                lineSize += this.scaleFactor * .25f;
            }
            context.SetLineWidth((nfloat)lineSize);
            context.SetStrokeColor(this.borderColor.CGColor);
            MakePath(context, rect);
            context.ClosePath();
            context.StrokePath();
			
            context.RestoreState();
        }
    }
}

