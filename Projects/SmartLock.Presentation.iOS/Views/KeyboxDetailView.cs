using System;
using System.Collections.Generic;
using System.Linq;
using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class KeyboxDetailView : View<IKeyboxDetailView>, IKeyboxDetailView
    {
        public event Action BackClick;
        public event Action LockHistoryClick;
        public event Action LockEditClick;
        public event Action LockDashboardClick;
        public event Action LockDataClick;
        public event Action FeedbackClick;
        public event Action Refresh;

        public KeyboxDetailView(KeyboxDetailController controller) : base(controller, "KeyboxDetailView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            BtnHistory.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                LockHistoryClick?.Invoke();
            }));

            BtnEdit.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                LockEditClick?.Invoke();
            }));

            BtnData.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                LockDataClick?.Invoke();
            }));

            BtnFeedback.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                FeedbackClick?.Invoke();
            }));

            BtnDashboard.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                LockDashboardClick?.Invoke();
            }));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Refresh?.Invoke();
        }

        public void Show(Keybox keybox, Property property, bool mine)
        {
            LblText1.Text = property.Address;
            LblText2.Text = property.PropertyName;
            LblBed.Text = property.Bedrooms.HasValue ? property.Bedrooms.Value.ToString() : "N/A";
            LblBathroom.Text = property.Bathrooms.HasValue ? property.Bathrooms.Value.ToString() : "N/A";
            LblArea.Text = property.FloorAreaString;
            LblPrice.Text = property.PriceString;

            LblFeedback.Text = mine ? "Feedback history" : "Leave a feedback";

            BtnHistory.Hidden = !mine;
            BtnEdit.Hidden = !mine;

            SetupViewPager(property);
        }

        private void SetupViewPager(Property property)
        {
            SlideShow.PagingEnabled = true;
            SlideShow.ShowsHorizontalScrollIndicator = false;
            SlideShow.ShowsVerticalScrollIndicator = false;

            var slideShowWidth = UIScreen.MainScreen.Bounds.Width - 40;

            SlideShow.WidthAnchor.ConstraintEqualTo(slideShowWidth).Active = true;
            IVImage1.WidthAnchor.ConstraintEqualTo(slideShowWidth).Active = true;
            IVImage2.WidthAnchor.ConstraintEqualTo(slideShowWidth).Active = true;
            IVImage3.WidthAnchor.ConstraintEqualTo(slideShowWidth).Active = true;
            IVImage4.WidthAnchor.ConstraintEqualTo(slideShowWidth).Active = true;

            IVImage1.HeightAnchor.ConstraintEqualTo(150).Active = true;
            IVImage2.HeightAnchor.ConstraintEqualTo(150).Active = true;
            IVImage3.HeightAnchor.ConstraintEqualTo(150).Active = true;
            IVImage4.HeightAnchor.ConstraintEqualTo(150).Active = true;

            var imageViewList = new List<UIImageView>();
            imageViewList.Add(IVImage1);
            imageViewList.Add(IVImage2);
            imageViewList.Add(IVImage3);
            imageViewList.Add(IVImage4);

            foreach (var imageView in imageViewList)
            {
                imageView.Hidden = true;
                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            }

            var propertyImages = property.PropertyResource.Select(p => p.Image).ToList();

            for (var i = 0; i < propertyImages.Count(); i++)
            {
                imageViewList[i].Image = UIImage.FromFile(propertyImages[i].NativePath);
                imageViewList[i].Hidden = false;
            }

            if (propertyImages.Count == 0)
            {
                imageViewList[0].Image = UIImage.FromBundle("Image");
                imageViewList[0].Hidden = false;
            }
        }
    }
}

