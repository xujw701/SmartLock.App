// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    [Register ("LockboxRecordCell")]
    partial class LockboxRecordCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblText1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblText2 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LblText1 != null) {
                LblText1.Dispose ();
                LblText1 = null;
            }

            if (LblText2 != null) {
                LblText2.Dispose ();
                LblText2 = null;
            }
        }
    }
}