// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    [Register("KeyboxPinView")]
    partial class KeyboxPinView
    {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton BtnSubmit { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtNewPassword1 { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtNewPassword2 { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtOldPassword { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (BtnSubmit != null)
            {
                BtnSubmit.Dispose();
                BtnSubmit = null;
            }

            if (EtNewPassword1 != null)
            {
                EtNewPassword1.Dispose();
                EtNewPassword1 = null;
            }

            if (EtNewPassword2 != null)
            {
                EtNewPassword2.Dispose();
                EtNewPassword2 = null;
            }

            if (EtOldPassword != null)
            {
                EtOldPassword.Dispose();
                EtOldPassword = null;
            }

            if (IvBack != null)
            {
                IvBack.Dispose();
                IvBack = null;
            }
        }
    }
}
