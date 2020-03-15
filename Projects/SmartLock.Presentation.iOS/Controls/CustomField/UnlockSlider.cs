using System;
using System.Timers;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.CustomField
{
    [Register("UnlockSlider")]
    public class UnlockSlider : UISlider
    {
        private Timer _timer;
        private DateTime lastInvokedTime;

        public event Action Unlocked;

        public UnlockSlider(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public override CGRect TrackRectForBounds(CGRect forBounds)
        {
            var sliderWidth = UIScreen.MainScreen.Bounds.Width - 64 - 48;

            return new CGRect(0, 0, sliderWidth, 45);
        }

        public override CGRect ThumbRectForBounds(CGRect bounds, CGRect trackRect, float value)
        {
            trackRect.X = trackRect.X - 15;

            trackRect.Width = trackRect.Width + 30;

            return RectangleFExtensions.Inset(base.ThumbRectForBounds(bounds, trackRect, value), 15, 10);
        }

        public void Reset()
        {
            SetTrackBackground(true);
            Value = 0;
            _timer.Stop();
        }

        private void Initialize()
        {
            SetTrackBackground(true);

            TouchUpInside += UnlockSlider_TouchUpInside;
            ValueChanged += UnlockSlider_ValueChanged;

            _timer = new Timer();
            _timer.Interval = 5;
            _timer.Enabled = true;
            _timer.Elapsed += (s, e) =>
            {
                InvokeOnMainThread(() =>
                {
                    if (Value > 1)
                    {
                        Value -= 1;
                    }
                    else
                    {
                        _timer.Stop();
                    }
                });
            };
        }

        private void UnlockSlider_TouchUpInside(object sender, EventArgs e)
        {
            if (Value < MaxValue)
            {
                _timer.Start();
            }
        }

        private void UnlockSlider_ValueChanged(object sender, EventArgs e)
        {
            if ((int)Value == 100)
            {
                var diff = (DateTime.Now - lastInvokedTime).TotalMilliseconds;

                if (diff > 500)
                {
                    lastInvokedTime = DateTime.Now;
                    Unlocked?.Invoke();
                    SetTrackBackground(false);
                }
            }
        }

        private void SetTrackBackground(bool locked)
        {
            CGColor[] colors;

            if (locked)
            {
                colors = new CGColor[] { UIColor.FromRGB(62, 83, 195).CGColor, UIColor.FromRGB(82, 156, 250).CGColor };

                SetThumbImage(UIImage.FromBundle("icon_unlock_key"), UIControlState.Normal);
            }
            else
            {
                colors = new CGColor[] { UIColor.FromRGB(46, 207, 98).CGColor, UIColor.FromRGB(60, 255, 224).CGColor };

                SetThumbImage(new UIImage(), UIControlState.Normal);
            }

            var lockedGradient = GetGradientImageWithColors(colors, Bounds.Size);
            var roundedGradient = GetRoundedImage(lockedGradient, Bounds);
            Layer.BackgroundColor = UIColor.Clear.CGColor;
            SetMinTrackImage(roundedGradient, UIControlState.Normal);
            SetMaxTrackImage(roundedGradient, UIControlState.Normal);
        }

        private UIImage GetGradientImageWithColors(CGColor[] colors, CGSize imageSize)
        {
            UIGraphics.BeginImageContextWithOptions(imageSize, true, UIScreen.MainScreen.Scale);

            var gradient = new CGGradient(null, colors);
            var start = new CGPoint(0.0, 0.0);
            var end = new CGPoint(imageSize.Width, imageSize.Height);
            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SaveState();

                context.DrawLinearGradient(
                    gradient: gradient,
                    startPoint: start,
                    endPoint: end,
                    options: CGGradientDrawingOptions.DrawsBeforeStartLocation | CGGradientDrawingOptions.DrawsAfterEndLocation);

                var result = UIGraphics.GetImageFromCurrentImageContext();

                UIGraphics.EndImageContext();

                return result;
            }
        }

        private UIImage GetRoundedImage(UIImage image, CGRect rect, float radius = 12f)
        {
            UIGraphics.BeginImageContextWithOptions(rect.Size, true, UIScreen.MainScreen.Scale);

            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(UIColor.SystemBackgroundColor.CGColor);
                context.FillRect(rect);

                UIBezierPath.FromRoundedRect(rect, radius).AddClip();

                context.DrawImage(rect, image.CGImage);

                var result = UIGraphics.GetImageFromCurrentImageContext();

                UIGraphics.EndImageContext();

                return result;
            }
        }
    }
}
