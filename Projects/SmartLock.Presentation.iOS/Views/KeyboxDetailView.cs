using System;
using System.Collections.Generic;
using System.Linq;
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
        public event Action<Cache> ImageClick;

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

            LblFeedback.Text = mine ? "Feedback History" : "Feedback";

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

            IVImage1.HeightAnchor.ConstraintEqualTo(230).Active = true;
            IVImage2.HeightAnchor.ConstraintEqualTo(230).Active = true;
            IVImage3.HeightAnchor.ConstraintEqualTo(230).Active = true;
            IVImage4.HeightAnchor.ConstraintEqualTo(230).Active = true;

            var imageViewList = new List<UIImageView>();
            imageViewList.Add(IVImage1);
            imageViewList.Add(IVImage2);
            imageViewList.Add(IVImage3);
            imageViewList.Add(IVImage4);

            var propertyImages = property.PropertyResource.Select(p => p.Image).ToList();

            var dotList = new List<UIImageView>();

            // Clear PageIndicatorContainer subviews
            foreach (var view in PageIndicatorContainer.Subviews)
            {
                view.RemoveFromSuperview();
            }

            foreach (var imageView in imageViewList)
            {
                imageView.Hidden = true;
                imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
                imageView.UserInteractionEnabled = true;

                imageView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    ImageClick?.Invoke(propertyImages[(int)imageView.Tag]);
                }));
            }

            for (var i = 0; i < propertyImages.Count(); i++)
            {
                imageViewList[i].Image = UIImage.FromFile(propertyImages[i].NativePath);
                imageViewList[i].Hidden = false;
                imageViewList[i].Tag = i;

                dotList.Add(CreateDot(i == 0));
            }

            if (propertyImages.Count == 0)
            {
                imageViewList[0].Image = UIImage.FromBundle("Image");
                imageViewList[0].Hidden = false;

                IvLeft.Hidden = true;
                IvRight.Hidden = true;
            }
            else
            {
                IvLeft.Hidden = false;
                IvRight.Hidden = false;
            }

            SlideShow.Scrolled += (s, a) =>
            {
                var pageWidth = SlideShow.Frame.Size.Width;
                var page = Math.Floor((SlideShow.ContentOffset.X - pageWidth / 2) / pageWidth) + 1;

                for (var pos = 0; pos < dotList.Count; pos ++)
                {
                    dotList[pos].Image = DotImage(pos == page);
                }
            };
        }

        private UIImageView CreateDot(bool enable)
        {
            var dot = new UIImageView(DotImage(enable));
            dot.HeightAnchor.ConstraintEqualTo(10).Active = true;
            dot.WidthAnchor.ConstraintEqualTo(10).Active = true;

            PageIndicatorContainer.AddArrangedSubview(dot);

            return dot;
        }

        private UIImage DotImage(bool enable)
        {
            return UIImage.FromBundle(enable ? "icon_dot_enable" : "icon_dot_disable");
        }
    }
}