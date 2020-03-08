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
    [Register ("PropertyFeedbackCell")]
    partial class PropertyFeedbackCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnPhone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnSms { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FeedbackContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvPhone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvPortrait { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvSms { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblDateTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblKeyboxName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblNotes { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnPhone != null) {
                BtnPhone.Dispose ();
                BtnPhone = null;
            }

            if (BtnSms != null) {
                BtnSms.Dispose ();
                BtnSms = null;
            }

            if (FeedbackContainer != null) {
                FeedbackContainer.Dispose ();
                FeedbackContainer = null;
            }

            if (IvPhone != null) {
                IvPhone.Dispose ();
                IvPhone = null;
            }

            if (IvPortrait != null) {
                IvPortrait.Dispose ();
                IvPortrait = null;
            }

            if (IvSms != null) {
                IvSms.Dispose ();
                IvSms = null;
            }

            if (LblDateTime != null) {
                LblDateTime.Dispose ();
                LblDateTime = null;
            }

            if (LblKeyboxName != null) {
                LblKeyboxName.Dispose ();
                LblKeyboxName = null;
            }

            if (LblName != null) {
                LblName.Dispose ();
                LblName = null;
            }

            if (LblNotes != null) {
                LblNotes.Dispose ();
                LblNotes = null;
            }
        }
    }
}