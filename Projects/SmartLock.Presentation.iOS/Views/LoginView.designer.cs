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
        UIKit.UITextField etUsername { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivLogo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (etUsername != null) {
                etUsername.Dispose ();
                etUsername = null;
            }

            if (ivLogo != null) {
                ivLogo.Dispose ();
                ivLogo = null;
            }
        }
    }
}