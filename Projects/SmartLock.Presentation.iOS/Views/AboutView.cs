using System;
using Foundation;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class AboutView : View<IAboutView>, IAboutView
    {
        public event Action BackClick;
        public event Action FeedbackClick;

        public AboutView(AboutController controller) : base(controller, "AboutView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            LblAppVersion.Text = "App Version: " + NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();

            BtnFeedback.TouchUpInside += (s, e) => FeedbackClick?.Invoke();
        }
    }
}