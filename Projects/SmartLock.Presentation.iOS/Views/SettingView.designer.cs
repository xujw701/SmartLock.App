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
    [Register ("SettingView")]
    partial class SettingView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvSetting { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IvSetting != null) {
                IvSetting.Dispose ();
                IvSetting = null;
            }
        }
    }
}