﻿// WARNING
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
    [Register ("PropertyFeedbackView")]
    partial class PropertyFeedbackView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView EtFeedback { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FeedbackTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnSubmit != null) {
                BtnSubmit.Dispose ();
                BtnSubmit = null;
            }

            if (EtFeedback != null) {
                EtFeedback.Dispose ();
                EtFeedback = null;
            }

            if (FeedbackTableView != null) {
                FeedbackTableView.Dispose ();
                FeedbackTableView = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }
        }
    }
}