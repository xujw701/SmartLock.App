using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class AttachmentController : ViewController<IAttachmentView>
    {
        public Cache Cache { get; set; }

        public AttachmentController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(Cache);
        }
    }
}