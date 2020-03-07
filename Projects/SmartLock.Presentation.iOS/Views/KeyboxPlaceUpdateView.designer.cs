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
    [Register ("KeyboxPlaceUpdateView")]
    partial class KeyboxPlaceUpdateView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtArea { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtBathroom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtBedroom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtData { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtPrice { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SmartLock.Presentation.iOS.Controls.CustomField.CustomTextField EtPriceOption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView HeaderView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ImagePickerTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IvBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBattery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView StackView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnSubmit != null) {
                BtnSubmit.Dispose ();
                BtnSubmit = null;
            }

            if (EtAddress != null) {
                EtAddress.Dispose ();
                EtAddress = null;
            }

            if (EtArea != null) {
                EtArea.Dispose ();
                EtArea = null;
            }

            if (EtBathroom != null) {
                EtBathroom.Dispose ();
                EtBathroom = null;
            }

            if (EtBedroom != null) {
                EtBedroom.Dispose ();
                EtBedroom = null;
            }

            if (EtData != null) {
                EtData.Dispose ();
                EtData = null;
            }

            if (EtName != null) {
                EtName.Dispose ();
                EtName = null;
            }

            if (EtPrice != null) {
                EtPrice.Dispose ();
                EtPrice = null;
            }

            if (EtPriceOption != null) {
                EtPriceOption.Dispose ();
                EtPriceOption = null;
            }

            if (HeaderView != null) {
                HeaderView.Dispose ();
                HeaderView = null;
            }

            if (ImagePickerTableView != null) {
                ImagePickerTableView.Dispose ();
                ImagePickerTableView = null;
            }

            if (IvBack != null) {
                IvBack.Dispose ();
                IvBack = null;
            }

            if (LblBattery != null) {
                LblBattery.Dispose ();
                LblBattery = null;
            }

            if (LblName != null) {
                LblName.Dispose ();
                LblName = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (StackView != null) {
                StackView.Dispose ();
                StackView = null;
            }
        }
    }
}