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
    [Register ("HomeView")]
    partial class HomeView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView BleDeviceTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnUnlock { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBattery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvLockIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvScanButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBattery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBtStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblGreeting { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblScanButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblText1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblText2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LockContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BleDeviceTableView != null) {
                BleDeviceTableView.Dispose ();
                BleDeviceTableView = null;
            }

            if (BtnUnlock != null) {
                BtnUnlock.Dispose ();
                BtnUnlock = null;
            }

            if (IvBattery != null) {
                IvBattery.Dispose ();
                IvBattery = null;
            }

            if (IvBt != null) {
                IvBt.Dispose ();
                IvBt = null;
            }

            if (IvClose != null) {
                IvClose.Dispose ();
                IvClose = null;
            }

            if (IvLockIcon != null) {
                IvLockIcon.Dispose ();
                IvLockIcon = null;
            }

            if (IvMessage != null) {
                IvMessage.Dispose ();
                IvMessage = null;
            }

            if (IvScanButton != null) {
                IvScanButton.Dispose ();
                IvScanButton = null;
            }

            if (LblBattery != null) {
                LblBattery.Dispose ();
                LblBattery = null;
            }

            if (LblBt != null) {
                LblBt.Dispose ();
                LblBt = null;
            }

            if (LblBtStatus != null) {
                LblBtStatus.Dispose ();
                LblBtStatus = null;
            }

            if (LblGreeting != null) {
                LblGreeting.Dispose ();
                LblGreeting = null;
            }

            if (LblName != null) {
                LblName.Dispose ();
                LblName = null;
            }

            if (LblScanButton != null) {
                LblScanButton.Dispose ();
                LblScanButton = null;
            }

            if (LblText1 != null) {
                LblText1.Dispose ();
                LblText1 = null;
            }

            if (LblText2 != null) {
                LblText2.Dispose ();
                LblText2 = null;
            }

            if (LockContainer != null) {
                LockContainer.Dispose ();
                LockContainer = null;
            }
        }
    }
}