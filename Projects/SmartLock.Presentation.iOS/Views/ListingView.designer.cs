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
    [Register ("ListingView")]
    partial class ListingView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvListing { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IvListing != null) {
                IvListing.Dispose ();
                IvListing = null;
            }
        }
    }
}