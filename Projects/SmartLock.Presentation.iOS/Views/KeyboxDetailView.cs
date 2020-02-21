using System;
using SmartLock.Model.Ble;
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
        public event Action LockDashboardClick;

        public KeyboxDetailView(KeyboxDetailController controller) : base(controller, "KeyboxDetailView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvImage.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width - 40).Active = true;

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            BtnHistory.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                LockHistoryClick?.Invoke();
            }));

            BtnDashboard.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                LockDashboardClick?.Invoke();
            }));
        }

        public void Show(Keybox keybox)
        {
        }
    }
}

