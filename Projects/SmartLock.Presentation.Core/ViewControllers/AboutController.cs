using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class AboutController : ViewController<IAboutView>
    {

        public AboutController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.FeedbackClick += () => { Push<FeedbackController>(); };
        }
    }
}
