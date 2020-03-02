using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxesController : ViewController<IKeyboxesView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        private List<Keybox> _keyboxes;

        private bool _mine = true;

        private Keybox ConnectedKeybox => _keyboxService.ConnectedKeybox;

        private bool CanPlaceLock =>  ConnectedKeybox != null
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
                    Push<KeyboxPlaceController>(vc => vc.Keybox = ConnectedKeybox);
                }
                else
                {
                    if (ConnectedKeybox == null)
                    {
                        _messageBoxService.ShowMessage("Cannot place keybox", "Please connect to a keybox first.");
                    }
                    else
                    {
                        _messageBoxService.ShowMessage("Cannot place keybox", "You are not the owner of this keybox.");
                    }
                }
            };
            View.TabClicked += (mine) =>
            {
                _mine = mine;

                LoadData();
            };
            View.Refresh += () => View.UpdatePlaceLockButton(CanPlaceLock);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.UpdatePlaceLockButton(CanPlaceLock);

            LoadData();
        }

        private void LoadData()
        {
            if (_mine)
            {
                DoSafeAsync(LoadMineData);
            }
            else
            {
                DoSafeAsync(LoadOthersData);
            }
        }

        private async Task LoadMineData()
        {
            _keyboxes = await _keyboxService.GetMyListingKeyboxes();

            View.Show(_keyboxes, CanPlaceLock);
        }

        private async Task LoadOthersData()
        {
            _keyboxes = await _keyboxService.GetKeyboxesIUnlocked();

            View.Show(_keyboxes, CanPlaceLock);
        }
    }
}
