using System;
using Foundation;
using SmartLock.Model.BlueToothLe;
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
            LblOpener.Text = keyboxHistory.Opener;
            LblDuration.Text = keyboxHistory.Duration;
            LblIn.Text = keyboxHistory.InTimeString;
            LblOut.Text = keyboxHistory.OutTimeString;

            ConfigureDemoPortait(keyboxHistory);
        }

        private void ConfigureDemoPortait(KeyboxHistory keyboxHistory)
        {
            if (keyboxHistory.Opener.StartsWith("Della"))
            {
                IvPortrait.Image = UIImage.FromBundle("portait1");
            }
            else if (keyboxHistory.Opener.StartsWith("Mol"))
            {
                IvPortrait.Image = UIImage.FromBundle("portait3");
            }
            else if (keyboxHistory.Opener.StartsWith("Win"))
            {
                IvPortrait.Image = UIImage.FromBundle("portait5");
            }
            else if (keyboxHistory.Opener.StartsWith("Har"))
            {
                IvPortrait.Image = UIImage.FromBundle("portait2");
            }
            else
            {
                IvPortrait.Image = UIImage.FromBundle("portait4");
            }
        }
    }
}
