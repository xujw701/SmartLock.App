using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
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

        private bool _mine = true;

        private Keybox ConnectedKeybox => _keyboxService.ConnectedKeybox;

        private bool CanPlaceLock =>  ConnectedKeybox != null
                && ConnectedKeybox.UserId.HasValue
                && ConnectedKeybox.UserId.Value == _userSession.UserId;
        private string PlaceLockTitle => ConnectedKeybox != null && ConnectedKeybox.PropertyId.HasValue ? "Replace Lock" : "Place Lock";

        public KeyboxesController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.OnKeyboxConnected += (keybox) => UpdatePlaceLockButton();
            _keyboxService.OnKeyboxDisconnected += () => UpdatePlaceLockButton();

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
            View.Refresh += () => UpdatePlaceLockButton();
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            UpdatePlaceLockButton();

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

            View.Show(_keyboxes);

            UpdatePlaceLockButton();
        }

        private async Task LoadOthersData()
        {
            _keyboxes = await _keyboxService.GetKeyboxesIUnlocked();

            View.Show(_keyboxes);

            UpdatePlaceLockButton();
        }

        private void UpdatePlaceLockButton()
        {
            View.UpdatePlaceLockButton(PlaceLockTitle, CanPlaceLock);
        }
    }
}