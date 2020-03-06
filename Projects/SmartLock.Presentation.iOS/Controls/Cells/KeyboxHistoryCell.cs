using System;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Support;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public partial class KeyboxHistoryCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("KeyboxHistoryCell");
        public static readonly UINib Nib;

        static KeyboxHistoryCell()
        {
            Nib = UINib.FromName("KeyboxHistoryCell", NSBundle.MainBundle);
        }

        protected KeyboxHistoryCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal static KeyboxHistoryCell Create()
        {
            return (KeyboxHistoryCell)Nib.Instantiate(null, null)[0];
        }

        public void SetData(KeyboxHistory keyboxHistory)
        {
            LblName.Text = keyboxHistory.Name;
            LblDuration.Text = keyboxHistory.Duration;
            LblIn.Text = keyboxHistory.InOnString;
            LblOut.Text = keyboxHistory.OutOnString;

            ConfigurePortait(keyboxHistory);
        }

        private void ConfigurePortait(KeyboxHistory keyboxHistory)
        {
            if (keyboxHistory.ResPortraitId.HasValue)
            {
                ImageHelper.SetImageView(IvPortrait, keyboxHistory.Portrait);
            }
            else
            {
                IvPortrait.Image = UIImage.FromBundle("portait4");
            }
        }
    }
}
