using System;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class AttachmentView : View<IAttachmentView>, IAttachmentView
    {
        public event Action BackClick;

        public AttachmentView(AttachmentController controller) : base(controller, "AttachmentView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));
        }

        public void Show(Cache cache)
        {
            ImageHelper.SetImageView(IvContent, cache);
        }
    }
}