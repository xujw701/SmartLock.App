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
        private readonly UILabel _button;

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
                Frame = new CGRect(0, 0, frame.Width, frame.Width),
                ContentMode = UIViewContentMode.ScaleAspectFit
            };

            _button = new UILabel
            {
                ClipsToBounds = true,
                Frame = new CGRect(0, frame.Width + ImagePickerCell.ButtonMarginTop, frame.Width, ImagePickerCell.ButtonHeight),
                TextAlignment = UITextAlignment.Center,
                UserInteractionEnabled = true
            };
            _button.Font = _button.Font.WithSize(14);

            ContentView.AddSubview(_imageView);
            ContentView.AddSubview(_button);
        }

        public void ConfigureImage(Cache attachment, Action<Cache> itemDeleted)
        {
            var imagePath = attachment.NativePath;
            _imageView.Image = UIImage.FromFile(imagePath) ?? null;

            _button.Text = "Delete";
            _button.TextColor = UIColor.Red;

            _button.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                itemDeleted?.Invoke(attachment);
            }));
        }

        public void ConfigureAddButton()
        {
            _imageView.Image = UIImage.FromBundle("icon_add_photo");
            _imageView.ContentMode = UIViewContentMode.Center;
            _imageView.TintColor = UIColor.FromRGB(96, 149, 255);

            _button.Text = "Add Photos";
            _button.TextColor = UIColor.Black;

        }
    }
}
