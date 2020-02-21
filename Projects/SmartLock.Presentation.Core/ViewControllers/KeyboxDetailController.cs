using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxDetailController : ViewController<IKeyboxDetailView>
    {
        public Keybox Keybox;

        public KeyboxDetailController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.LockDashboardClick += () => Push<KeyboxDashboardController>();
            View.LockHistoryClick += () => Push<KeyboxHistoryController>();
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(Keybox);
        }
    }
}
