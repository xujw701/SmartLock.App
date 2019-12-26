using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class ListingView : View<IListingView>, IListingView
    {
        public ListingView(ListingController controller) : base(controller, "ListingView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvListing.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;
        }
    }
}

