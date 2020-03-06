using System;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class SettingView : View<ISettingView>, ISettingView
    {
        public event Action<byte[]> PortraitChanged;
        public event Action ProfileClick;
        public event Action PasswordClick;
        public event Action FeedbackClick;
        public event Action LogoutClick;
        public event Action Refresh;

        public SettingView(SettingController controller) : base(controller, "SettingView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvSetting.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;
        }

        public void Show(string name, Cache portrait)
        {
        }
    }
}

