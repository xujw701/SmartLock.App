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
    [Register ("KeyboxHistoryCell")]
    partial class KeyboxHistoryCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvIn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvOut { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvPortrait { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblDuration { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblIn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblOpener { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblOut { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IvIn != null) {
                IvIn.Dispose ();
                IvIn = null;
            }

            if (IvOut != null) {
                IvOut.Dispose ();
                IvOut = null;
            }

            if (IvPortrait != null) {
                IvPortrait.Dispose ();
                IvPortrait = null;
            }

            if (LblDuration != null) {
                LblDuration.Dispose ();
                LblDuration = null;
            }

            if (LblIn != null) {
                LblIn.Dispose ();
                LblIn = null;
            }

            if (LblOpener != null) {
                LblOpener.Dispose ();
                LblOpener = null;
            }

            if (LblOut != null) {
                LblOut.Dispose ();
                LblOut = null;
            }
        }
    }
}