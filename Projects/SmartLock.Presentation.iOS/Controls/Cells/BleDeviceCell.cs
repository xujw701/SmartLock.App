using System;
using Foundation;
using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Support;
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

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ShadowHelper.AddShadow(ContentContainer);
        }

        public void SetData(Keybox keybox, Action<Keybox> connect, Action<Keybox> disconnect, Action cancel)
        {
            LblText1.Text = keybox.PropertyAddress;
            LblText2.Text = keybox.KeyboxName;
            LblBattery.Text = keybox.BatteryLevelString;

            BtnConnect.TouchUpInside += (s, e) =>
            {
                UpdateUI(true);
                connect?.Invoke(keybox);
            };

            BtnCancel.TouchUpInside += (s, e) =>
            {
                UpdateUI(false);
                disconnect?.Invoke(keybox);
            };

            IvClose.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                cancel?.Invoke();
            }));

            UpdateUI(keybox.State == DeviceState.Connecting);
        }

        private void UpdateUI(bool connecting)
        {
            BtnConnect.SetTitle(connecting ? "Connecting..." : "Connect", UIControlState.Normal);
            BtnCancel.Hidden = !connecting;
        }
    }
}
