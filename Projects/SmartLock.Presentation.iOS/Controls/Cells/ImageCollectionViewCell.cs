using System;
using CoreGraphics;
using Foundation;
using SmartLock.Model.Models;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public class ImageCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ImageCollectionViewCell");

        private readonly UIImageView _imageView;

        private nfloat _width;
        private nfloat _height;

        [Export("initWithFrame:")]
        public ImageCollectionViewCell(CGRect frame) : base(frame)
        {
            BackgroundView = new UIView { BackgroundColor = UIColor.White };

            SelectedBackgroundView = new UIView { BackgroundColor = UIColor.White };

            ContentView.Layer.BorderWidth = 0;
            ContentView.BackgroundColor = UIColor.White;

            _imageView = new UIImageView
            {
                ClipsToBounds = true,
                Frame = new CGRect(0, 0, frame.Width, frame.Height),
                ContentMode = UIViewContentMode.ScaleAspectFit
            };

            _width = frame.Width;
            _height = frame.Height;

            ContentView.AddSubview(_imageView);
        }

        public void ConfigureImage(Cache attachment)
        {
            var imagePath = attachment.NativePath;
            _imageView.Image = UIImage.FromFile(imagePath) ?? null;
        }

        public void ConfigureAddButton()
        {
            _imageView.Image = UIImage.FromBundle("icon_add_photo");
            _imageView.ContentMode = UIViewContentMode.Center;
            _imageView.TintColor = UIColor.FromRGB(96, 149, 255);
        }
    }
}
