using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxesController : ViewController<IKeyboxesView>
    {
        private readonly IKeyboxService _keyboxService;

        private List<Keybox> _keyboxes;

        public KeyboxesController(IViewService viewService, IKeyboxService keyboxService) : base(viewService)
        {
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.OnKeyboxConnected += (keybox) => { View.UpdatePlaceLockButton(true); };
            _keyboxService.OnKeyboxDisconnected += () => { View.UpdatePlaceLockButton(true); };

            View.KeyboxClicked += (keybox) => Push<KeyboxDetailController>(vc => vc.Keybox = keybox);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            DoSafeAsync(LoadData);
        }

        private async Task LoadData()
        {
            _keyboxes = await _keyboxService.GetMyListingKeyboxes();

            View.Show(_keyboxes, _keyboxService.ConnectedKeybox != null);
        }
    }
}
