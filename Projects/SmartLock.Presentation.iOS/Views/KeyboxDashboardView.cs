using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class KeyboxDashboardView : View<IKeyboxDashboardView>, IKeyboxDashboardView
    {
        public event Action BackClick;

        public KeyboxDashboardView(KeyboxDashboardController controller) : base(controller, "KeyboxDashboardView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvGraph.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));
        }
    }
}

