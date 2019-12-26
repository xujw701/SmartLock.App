using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class SettingView : View<ISettingView>, ISettingView
    {
        public SettingView(SettingController controller) : base(controller, "SettingView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvSetting.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;
        }
    }
}

