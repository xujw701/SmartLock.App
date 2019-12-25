using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.CustomField
{
    [Register("CustomTextField")]
    public class CustomTextField : UITextField
    {
        public CustomTextField(IntPtr handle) : base(handle)
        {
            Initalize();
        }

        private void Initalize()
        {
            LeftView = new UIView(new CGRect(0, 0, 16, 0));

            LeftViewMode = UITextFieldViewMode.Always;
        }
    }
}
