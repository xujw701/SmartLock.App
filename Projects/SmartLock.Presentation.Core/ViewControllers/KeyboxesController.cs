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
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        private List<Keybox> _keyboxes;

        public KeyboxesController(IViewService viewService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.OnKeyboxConnected += (keybox) => { View.UpdatePlaceLockButton(CanPlaceLock()); };
            _keyboxService.OnKeyboxDisconnected += () => { View.UpdatePlaceLockButton(CanPlaceLock()); };

            View.KeyboxClicked += (keybox) => Push<KeyboxDetailController>(vc => vc.Keybox = keybox);
            View.PlaceKeyboxClicked += () => Push<KeyboxPlaceController>();
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            DoSafeAsync(LoadData);
        }

        private async Task LoadData()
        {
            _keyboxes = await _keyboxService.GetMyListingKeyboxes();

            View.Show(_keyboxes, CanPlaceLock());
        }

        private bool CanPlaceLock()
        {
            var connectedKeybox = _keyboxService.ConnectedKeybox;
            return connectedKeybox != null
                && !connectedKeybox.PropertyId.HasValue
                && connectedKeybox.UserId.HasValue && connectedKeybox.UserId.Value == _userSession.UserId;
        }
    }
}
