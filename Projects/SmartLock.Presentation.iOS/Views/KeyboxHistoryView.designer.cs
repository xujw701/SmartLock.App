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
        UIKit.UITableView HistoryTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HistoryTableView != null) {
                HistoryTableView.Dispose ();
                HistoryTableView = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }
        }
    }
}