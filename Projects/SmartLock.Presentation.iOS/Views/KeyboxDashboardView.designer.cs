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
    [Register ("KeyboxDashboardView")]
    partial class KeyboxDashboardView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Btn3Months { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BtnMonth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BtnWeek { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvGraph { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Btn3Months != null) {
                Btn3Months.Dispose ();
                Btn3Months = null;
            }

            if (BtnMonth != null) {
                BtnMonth.Dispose ();
                BtnMonth = null;
            }

            if (BtnWeek != null) {
                BtnWeek.Dispose ();
                BtnWeek = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }

            if (IvGraph != null) {
                IvGraph.Dispose ();
                IvGraph = null;
            }
        }
    }
}