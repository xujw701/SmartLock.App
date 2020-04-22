using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.CustomField
{
    [Register("CustomTextField")]
    public class CustomTextField : UITextField
    {
        private string _title;

        public CustomTextField(IntPtr handle) : base(handle)
        {
            Initalize();
        }

        public void SetTitle(string title)
        {
            _title = title;

            if (!string.IsNullOrEmpty(_title))
            {
                var lblTitle = new UILabel(new CGRect(10, -1, 70, 64));
                lblTitle.Text = _title;
                lblTitle.Font = lblTitle.Font.WithSize(14);
                lblTitle.TextColor = UIColor.DarkGray;

                AddSubview(lblTitle);

                LeftView = new UIView(new CGRect(0, 0, 85, 0));
            }
        }

        private void Initalize()
        {
            LeftView = new UIView(new CGRect(0, 0, 16, 0));

            LeftViewMode = UITextFieldViewMode.Always;
        }
    }
}
