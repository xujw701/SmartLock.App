using SmartLock.Model.Ble;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxDashboardController : ViewController<IKeyboxDashboardView>
    {
        public KeyboxDashboardController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
        }
    }
}
