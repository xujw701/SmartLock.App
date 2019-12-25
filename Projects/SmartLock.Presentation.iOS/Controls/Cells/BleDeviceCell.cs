using System;
using CoreGraphics;
using Foundation;
using SmartLock.Model.BlueToothLe;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public partial class BleDeviceCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("BleDeviceCell");
        public static readonly UINib Nib;

        static BleDeviceCell()
        {
            Nib = UINib.FromName("BleDeviceCell", NSBundle.MainBundle);
        }

        protected BleDeviceCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal static BleDeviceCell Create()
        {
            return (BleDeviceCell)Nib.Instantiate(null, null)[0];
        }

        public void SetData(BleDevice bleDevice, Action<BleDevice> connect, Action<BleDevice> disconnect)
        {
            LblText1.Text = bleDevice.Name;
            LblBattery.Text = bleDevice.BatteryLevelString;

            BtnConnect.TouchUpInside += (s, e) =>
            {
                UpdateUI(true);
                connect?.Invoke(bleDevice);
            };

            BtnCancel.TouchUpInside += (s, e) =>
            {
                UpdateUI(false);
                connect?.Invoke(bleDevice);
            };

            UpdateUI(bleDevice.State == DeviceState.Connecting);
        }

        private void UpdateUI(bool connecting)
        {
            BtnConnect.SetTitle(connecting ? "Connecting..." : "Connect", UIControlState.Normal);
            BtnCancel.Hidden = !connecting;
        }
    }
}
