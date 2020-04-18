using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class EnvironmentView : View<IEnvironmentView>, IEnvironmentView
    {
        public event Action BackClick;
        public event Action<string> EnvironemntChanged;

        public EnvironmentView(EnvironmentController controller) : base(controller, "EnvironmentView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            ShadowHelper.AddShadow(DropdownEnvironment);
        }

        public void Show(List<string> environemnts, string selectedEnvironment)
        {
#if DEBUG
            var buildType = " (DEV)";
#else
            var buildType = " (PRD)";
#endif
            LblAppVersion.Text = "App Version: " + NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString() + buildType;

            UIHelper.SetupPicker(DropdownEnvironment, environemnts.ToArray(), EnvironemntChanged);

            DropdownEnvironment.Text = selectedEnvironment;
        }
    }
}