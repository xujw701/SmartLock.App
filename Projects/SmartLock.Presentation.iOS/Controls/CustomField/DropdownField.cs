using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.CustomField
{
    [Register("DropdownField")]
    public class DropdownField : UITextField
    {
        public DropdownField(IntPtr handle) : base(handle)
        {
            Initalize();
        }

        private void Initalize()
        {
            LeftView = new UIView(new CGRect(0, 0, 16, 0));

            LeftViewMode = UITextFieldViewMode.Always;

            var imageView = new UIImageView(new CGRect(0, 0, 24, 24))
            {
                Image = UIImage.FromBundle("icon_dropdown"),
                ClipsToBounds = true,
            };
            imageView.TintColor = UIColor.FromRGB(0, 57, 255);

            RightView = imageView;
            RightViewMode = UITextFieldViewMode.Always;
        }
    }
}
