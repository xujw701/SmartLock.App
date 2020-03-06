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
    [Register ("LoginView")]
    partial class LoginView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnForgotPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnLogin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnSignUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtUsername { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvCheckbox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivLogo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnForgotPassword != null) {
                BtnForgotPassword.Dispose ();
                BtnForgotPassword = null;
            }

            if (BtnLogin != null) {
                BtnLogin.Dispose ();
                BtnLogin = null;
            }

            if (BtnSignUp != null) {
                BtnSignUp.Dispose ();
                BtnSignUp = null;
            }

            if (EtPassword != null) {
                EtPassword.Dispose ();
                EtPassword = null;
            }

            if (EtUsername != null) {
                EtUsername.Dispose ();
                EtUsername = null;
            }

            if (IvCheckbox != null) {
                IvCheckbox.Dispose ();
                IvCheckbox = null;
            }

            if (ivLogo != null) {
                ivLogo.Dispose ();
                ivLogo = null;
            }
        }
    }
}