using SmartLock.Model.BlueToothLe;
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
            View.LockHistoryClick += () => { };
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(Keybox);
        }
    }
}
