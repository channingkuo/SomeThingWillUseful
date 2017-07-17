using System;
using UIKit;

namespace RekTec.Chat.Views.Common
{
    public class FullScreenImageView : UIView
    {
        UIImage iImage;
        ScrollImageView sviMain;

        public bool UseAnimation = true;
        public float AnimationDuration = 0.3f;

        public FullScreenImageView()
        {
            BackgroundColor = UIColor.Black;

            sviMain = new ScrollImageView();
            AddSubview(sviMain);
        }

        public void SetImage(UIImage image)
        {
            iImage = image;
        }

        public void Show()
        {
            var window = UIApplication.SharedApplication.Windows[0];
            Frame = window.Frame;
            sviMain.Frame = window.Frame;
            sviMain.SetImage(iImage);
            sviMain.OnSingleTap += () =>
            {
                Hide();
            };

            window.AddSubview(this);

            Alpha = 0f;
            UIView.Animate(AnimationDuration, () =>
                {
                    Alpha = 1f;
                });
        }

        public void Hide()
        {
            if (Superview != null)
            {
                if (!UseAnimation)
                {
                    RemoveFromSuperview();
                }
                else
                {
                    Alpha = 1f;
                    UIView.Animate(AnimationDuration, () =>
                        {
                            Alpha = 0f;
                        }, () =>
                        {
                            RemoveFromSuperview();
                        });
                }
            }
        }
    }
}

