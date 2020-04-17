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
    [Register ("EnvironmentView")]
    partial class EnvironmentView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.DropdownField DropdownEnvironment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblAppVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DropdownEnvironment != null) {
                DropdownEnvironment.Dispose ();
                DropdownEnvironment = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }

            if (LblAppVersion != null) {
                LblAppVersion.Dispose ();
                LblAppVersion = null;
            }

            if (LblTitle != null) {
                LblTitle.Dispose ();
                LblTitle = null;
            }
        }
    }
}