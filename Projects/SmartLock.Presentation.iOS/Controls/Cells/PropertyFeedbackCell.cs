using System;
using Foundation;
using SmartLock.Infrastructure;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.iOS.Support;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public partial class PropertyFeedbackCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PropertyFeedbackCell");
        public static readonly UINib Nib;

        static PropertyFeedbackCell()
        {
            Nib = UINib.FromName("PropertyFeedbackCell", NSBundle.MainBundle);
        }

        protected PropertyFeedbackCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal static PropertyFeedbackCell Create()
        {
            return (PropertyFeedbackCell)Nib.Instantiate(null, null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ShadowHelper.AddShadow(FeedbackContainer);
        }

        public void SetData(PropertyFeedback propertyFeedback)
        {
            LblKeyboxName.Text = propertyFeedback.KeyboxName;
            LblName.Text = propertyFeedback.Name;
            LblDateTime.Text = propertyFeedback.CreatedOnString;
            LblNotes.Text = propertyFeedback.Content;

            BtnPhone.TouchUpInside += (s, e) =>
            {
                IoC.Resolve<IPlatformServices>().Call(propertyFeedback.Phone);
            };

            BtnSms.TouchUpInside += (s, e) =>
            {
                IoC.Resolve<IPlatformServices>().Sms(propertyFeedback.Phone);
            };

            ConfigurePortait(propertyFeedback);
        }

        private void ConfigurePortait(PropertyFeedback propertyFeedback)
        {
            if (propertyFeedback.ResPortraitId.HasValue)
            {
                ImageHelper.SetImageView(IvPortrait, propertyFeedback.Portrait);
            }
        }
    }
}
