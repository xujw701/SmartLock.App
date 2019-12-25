using System;
using CoreGraphics;
using UIKit;

namespace SmartLock.Presentation.iOS.Support
{
    public static class ShadowHelper
    {
        public static void AddShadow(UIView view)
        {
            view.Layer.ShadowRadius = 8f;
            view.Layer.ShadowColor = UIColor.FromRGB(197, 219, 241).CGColor;
            view.Layer.ShadowOffset = new CGSize(0, 0);
            view.Layer.ShadowOpacity = 0.3f;
            view.Layer.MasksToBounds = false;

            var shadowPath = UIBezierPath.FromRoundedRect(view.Bounds, 8f);
            view.Layer.ShadowPath = shadowPath.CGPath;
            view.Layer.ShouldRasterize = true;
            view.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
        }
    }
}
