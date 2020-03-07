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
    [Register ("KeyboxesView")]
    partial class KeyboxesView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnAddLock { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BtnMine { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BtnOthers { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvAdd { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvMine { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvOther { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView KeyboxesTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnAddLock != null) {
                BtnAddLock.Dispose ();
                BtnAddLock = null;
            }

            if (BtnMine != null) {
                BtnMine.Dispose ();
                BtnMine = null;
            }

            if (BtnOthers != null) {
                BtnOthers.Dispose ();
                BtnOthers = null;
            }

            if (IvAdd != null) {
                IvAdd.Dispose ();
                IvAdd = null;
            }

            if (IvMine != null) {
                IvMine.Dispose ();
                IvMine = null;
            }

            if (IvOther != null) {
                IvOther.Dispose ();
                IvOther = null;
            }

            if (KeyboxesTableView != null) {
                KeyboxesTableView.Dispose ();
                KeyboxesTableView = null;
            }

            if (LblTitle != null) {
                LblTitle.Dispose ();
                LblTitle = null;
            }
        }
    }
}