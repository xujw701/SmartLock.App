using System;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Support;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public partial class KeyboxCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("KeyboxCell");
        public static readonly UINib Nib;

        static KeyboxCell()
        {
            Nib = UINib.FromName("KeyboxCell", NSBundle.MainBundle);
        }

        protected KeyboxCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal static KeyboxCell Create()
        {
            return (KeyboxCell)Nib.Instantiate(null, null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ShadowHelper.AddShadow(ContentContainer);
        }

        public void SetData(Keybox keybox)
        {
            LblText1.Text = keybox.PropertyAddress;
            LblText2.Text = keybox.KeyboxName;
            LblBattery.Text = keybox.BatteryLevelString;

            SetBatteryColor(keybox);
        }

        private void SetBatteryColor(Keybox keybox)
        {
            if (keybox.BatteryLevel < 20)
            {
                LblBattery.TextColor = UIColor.FromRGB(255, 28, 28);
            }
            else if (keybox.BatteryLevel < 40)
            {
                LblBattery.TextColor = UIColor.FromRGB(244, 115, 0);
            }
            else
            {
                LblBattery.TextColor = UIColor.FromRGB(0, 194, 63);
            }
        }
    }
}
