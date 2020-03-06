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
    }
}
