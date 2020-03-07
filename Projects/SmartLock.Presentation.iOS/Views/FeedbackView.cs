using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class FeedbackView : View<IFeedbackView>, IFeedbackView
    {
        public event Action BackClick;
        public event Action<string> SubmitClick;

        public FeedbackView(FeedbackController controller) : base(controller, "FeedbackView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ConfigureUI();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            BtnSubmit.TouchUpInside += (s, e) => SubmitClick?.Invoke(EtFeedback.Text);
        }

        private void ConfigureUI()
        {
            var contentWidth = UIScreen.MainScreen.Bounds.Width - 40;

            EtFeedback.WidthAnchor.ConstraintEqualTo(contentWidth).Active = true;

            ShadowHelper.AddShadow(EtFeedback);
        }
    }
}

