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
    [Register ("KeyboxHistoryView")]
    partial class KeyboxHistoryView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Btn30Days { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Btn7Days { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BtnAll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Dot30Days { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Dot7Days { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView DotAll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView HistoryTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Lbl30Days { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Lbl7Days { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblAll { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Btn30Days != null) {
                Btn30Days.Dispose ();
                Btn30Days = null;
            }

            if (Btn7Days != null) {
                Btn7Days.Dispose ();
                Btn7Days = null;
            }

            if (BtnAll != null) {
                BtnAll.Dispose ();
                BtnAll = null;
            }

            if (Dot30Days != null) {
                Dot30Days.Dispose ();
                Dot30Days = null;
            }

            if (Dot7Days != null) {
                Dot7Days.Dispose ();
                Dot7Days = null;
            }

            if (DotAll != null) {
                DotAll.Dispose ();
                DotAll = null;
            }

            if (HistoryTableView != null) {
                HistoryTableView.Dispose ();
                HistoryTableView = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }

            if (Lbl30Days != null) {
                Lbl30Days.Dispose ();
                Lbl30Days = null;
            }

            if (Lbl7Days != null) {
                Lbl7Days.Dispose ();
                Lbl7Days = null;
            }

            if (LblAll != null) {
                LblAll.Dispose ();
                LblAll = null;
            }
        }
    }
}