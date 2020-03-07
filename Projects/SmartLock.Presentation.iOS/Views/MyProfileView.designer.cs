// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    [Register ("MyProfileView")]
    partial class MyProfileView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtEmail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtFirstName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtLastName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtPhone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnSubmit != null) {
                BtnSubmit.Dispose ();
                BtnSubmit = null;
            }

            if (EtEmail != null) {
                EtEmail.Dispose ();
                EtEmail = null;
            }

            if (EtFirstName != null) {
                EtFirstName.Dispose ();
                EtFirstName = null;
            }

            if (EtLastName != null) {
                EtLastName.Dispose ();
                EtLastName = null;
            }

            if (EtPhone != null) {
                EtPhone.Dispose ();
                EtPhone = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }
        }
    }
}