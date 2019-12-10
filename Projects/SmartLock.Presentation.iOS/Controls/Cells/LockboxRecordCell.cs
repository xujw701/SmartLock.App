using System;

using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public partial class LockboxRecordCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LockboxRecordCell");
        public static readonly UINib Nib;

        static LockboxRecordCell()
        {
            Nib = UINib.FromName("LockboxRecordCell", NSBundle.MainBundle);
        }

        protected LockboxRecordCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal static LockboxRecordCell Create()
        {
            return (LockboxRecordCell)Nib.Instantiate(null, null)[0];
        }

        public void Configure(string text1, string text2)
        {
            LblText1.Text = text1;
            LblText2.Text = text2;
        }
    }
}
