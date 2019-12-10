using System;

using Foundation;
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

        public void Configure(string name, bool isChecked)
        {
            LblTitle.Text = name;
            IvCheck.Hidden = !isChecked;
        }
    }
}
