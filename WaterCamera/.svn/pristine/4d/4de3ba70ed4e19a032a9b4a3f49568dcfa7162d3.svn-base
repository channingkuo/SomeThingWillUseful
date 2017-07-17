using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace RekTec.Chat.Views.Common
{
    public class ClickableImageView : UIImageView
    {
        UITapGestureRecognizer grTap;

        event Action onCl;

        public event Action OnClick
        {
            add
            {
                onCl += value;
                UpdateUserInteractionFlag();
            }
            remove
            {
                onCl -= value;
                UpdateUserInteractionFlag();
            }
        }

        void UpdateUserInteractionFlag()
        {
            var enableTap = ((onCl != null) && (onCl.GetInvocationList().Length > 0));
            MultipleTouchEnabled = enableTap;
            UserInteractionEnabled = enableTap;
            if (UserInteractionEnabled)
            {
                if (grTap == null)
                {
                    grTap = new UITapGestureRecognizer(() =>
                        {
                            if (onCl != null)
                            {
                                onCl();
                            }
                        }) { NumberOfTouchesRequired = 1, NumberOfTapsRequired = 11 };
                    grTap.CancelsTouchesInView = true;
                    AddGestureRecognizer(grTap);
                }
            }
            else
            {
                if (grTap != null)
                {
                    RemoveGestureRecognizer(grTap);
                    grTap = null;
                }
            }
        }

        public ClickableImageView()
        {
        }

        public ClickableImageView(CGRect rect)
            : base(rect)
        {
        }

        public ClickableImageView(UIImage image)
            : base(image)
        {
        }
    }
}

