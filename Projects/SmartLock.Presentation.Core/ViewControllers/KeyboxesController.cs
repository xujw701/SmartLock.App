using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxesController : ViewController<IKeyboxesView>
    {
        public KeyboxesController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.Title = "My Locks";
        }
    }
}
