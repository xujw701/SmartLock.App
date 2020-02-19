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
    [Register ("BleDeviceCell")]
    partial class BleDeviceCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnCancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnConnect { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ContentContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBattery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBattery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblText1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblText2 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnCancel != null) {
                BtnCancel.Dispose ();
                BtnCancel = null;
            }

            if (BtnConnect != null) {
                BtnConnect.Dispose ();
                BtnConnect = null;
            }

            if (ContentContainer != null) {
                ContentContainer.Dispose ();
                ContentContainer = null;
            }

            if (IvBattery != null) {
                IvBattery.Dispose ();
                IvBattery = null;
            }

            if (IvBt != null) {
                IvBt.Dispose ();
                IvBt = null;
            }

            if (LblBattery != null) {
                LblBattery.Dispose ();
                LblBattery = null;
            }

            if (LblText1 != null) {
                LblText1.Dispose ();
                LblText1 = null;
            }

            if (LblText2 != null) {
                LblText2.Dispose ();
                LblText2 = null;
            }
        }
    }
}