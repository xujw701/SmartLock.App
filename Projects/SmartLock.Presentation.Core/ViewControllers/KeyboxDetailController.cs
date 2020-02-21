using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxDetailController : ViewController<IKeyboxDetailView>
    {
        private readonly IKeyboxService _keyboxService;

        private Property _property;

        public Keybox Keybox;
 
        public KeyboxDetailController(IViewService viewService, IKeyboxService keyboxService) : base(viewService)
        {
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            if (Keybox == null) throw new Exception("Key details should be feteched first.");

            View.BackClick += () => Pop();
            View.LockDashboardClick += () => Push<KeyboxDashboardController>();
            View.LockHistoryClick += () => Push<KeyboxHistoryController>(vc => { vc.Keybox = Keybox; vc.Property = _property; });
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            DoSafeAsync(LoadData);
        }

        private async Task LoadData()
        {
            if (!Keybox.PropertyId.HasValue) throw new Exception("Keybox isn't listed.");

            _property = await _keyboxService.GetKeyboxProperty(Keybox.KeyboxId, Keybox.PropertyId.Value);

            View.Show(Keybox, _property);
        }
    }
}
