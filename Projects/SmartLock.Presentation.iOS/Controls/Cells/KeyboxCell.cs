using System;
using CoreGraphics;
using Foundation;
using SmartLock.Model.BlueToothLe;
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

        public void SetData(Keybox keybox)
        {
            ConfigureShadow();

            LblText1.Text = keybox.Name;
            LblText2.Text = keybox.Address;
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

        private void ConfigureShadow()
        {
            //Layer.ShadowRadius = 1f;
            //Layer.ShadowColor = UIColor.FromRGB(176, 199, 226).CGColor;
            //Layer.ShadowOffset = new CGSize(0, 0);
            //Layer.ShadowOpacity = 0.5f;
            //Layer.MasksToBounds = false;

            //var shadowInsets = new UIEdgeInsets(-1.5f, -1.5f, -1.5f, -1.5f);
            //var shadowPath = UIBezierPath.FromRect(shadowInsets.InsetRect(Bounds));
            //Layer.ShadowPath = shadowPath.CGPath;
            //Layer.ShouldRasterize = true;
            //Layer.RasterizationScale = UIScreen.MainScreen.Scale;
        }
    }
}
