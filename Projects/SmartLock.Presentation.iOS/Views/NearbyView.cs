using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class NearbyView : View<INearbyView>, INearbyView
    {
        public NearbyView(NearbyController controller) : base(controller, "NearbyView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvNearBy.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;
        }
    }
}

