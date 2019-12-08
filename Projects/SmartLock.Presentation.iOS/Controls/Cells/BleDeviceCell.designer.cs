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
    [Register ("BleDeviceCell")]
    partial class BleDeviceCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvCheck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IvCheck != null) {
                IvCheck.Dispose ();
                IvCheck = null;
            }

            if (LblTitle != null) {
                LblTitle.Dispose ();
                LblTitle = null;
            }
        }
    }
}