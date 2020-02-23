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
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        private List<Keybox> _keyboxes;

        private Keybox ConnectedKeybox => _keyboxService.ConnectedKeybox;

        private bool CanPlaceLock => ConnectedKeybox != null
                && ConnectedKeybox.UserId.HasValue
                && ConnectedKeybox.UserId.Value == _userSession.UserId;

        public KeyboxesController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.OnKeyboxConnected += (keybox) => { View.UpdatePlaceLockButton(CanPlaceLock); };
            _keyboxService.OnKeyboxDisconnected += () => { View.UpdatePlaceLockButton(CanPlaceLock); };

            View.KeyboxClicked += (keybox) => Push<KeyboxDetailController>(vc => vc.Keybox = keybox);
            View.PlaceKeyboxClicked += () =>
            {
                if (CanPlaceLock)
                {
                    Push<KeyboxPlaceController>();
                }
                else
                {
                    _messageBoxService.ShowMessage("Cannot place keybox", "Please connect to a keybox first.");
                }
            };
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.UpdatePlaceLockButton(CanPlaceLock);

            DoSafeAsync(LoadData);
        }

        private async Task LoadData()
        {
            _keyboxes = await _keyboxService.GetMyListingKeyboxes();

            View.Show(_keyboxes, CanPlaceLock);
        }
    }
}
