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
    [Register ("AttachmentView")]
    partial class AttachmentView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvContent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }

            if (IvContent != null) {
                IvContent.Dispose ();
                IvContent = null;
            }
        }
    }
}