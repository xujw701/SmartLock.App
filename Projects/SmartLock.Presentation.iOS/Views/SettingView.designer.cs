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
    [Register ("SettingView")]
    partial class SettingView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnAbout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnLogout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnProfile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvPortrait { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnAbout != null) {
                BtnAbout.Dispose ();
                BtnAbout = null;
            }

            if (BtnLogout != null) {
                BtnLogout.Dispose ();
                BtnLogout = null;
            }

            if (BtnPassword != null) {
                BtnPassword.Dispose ();
                BtnPassword = null;
            }

            if (BtnProfile != null) {
                BtnProfile.Dispose ();
                BtnProfile = null;
            }

            if (IvPortrait != null) {
                IvPortrait.Dispose ();
                IvPortrait = null;
            }

            if (LblName != null) {
                LblName.Dispose ();
                LblName = null;
            }
        }
    }
}