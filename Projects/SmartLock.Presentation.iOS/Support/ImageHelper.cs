using System;
using SmartLock.Model.Models;
using UIKit;

namespace SmartLock.Presentation.iOS.Support
{
    public static class ImageHelper
    {
        public static void SetImageView(UIImageView imageView, Cache cache)
        {
            imageView.Image = UIImage.FromFile(cache.NativePath);
        }

        public static UIImage MaxResizeImage(this UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CoreGraphics.CGSize(width, height));
            sourceImage.Draw(new CoreGraphics.CGRect(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
    }
}
