using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxPlaceController : ViewController<IKeyboxPlaceView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        private Property _property;

        private Keybox ConnectedKeybox => _keyboxService.ConnectedKeybox ?? throw new Exception("Please connect to a keybox first.");
        private bool IsListed => ConnectedKeybox != null && ConnectedKeybox.PropertyId.HasValue;

        public KeyboxPlaceController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.SubmitClick += (property) => DoSafeAsync(SubmitData);

            if (_property == null)
            {
                _property = new Property();
            }
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(_keyboxService.ConnectedKeybox, _property);

            if (IsListed)
            {
                _messageBoxService.ShowMessage("Please notice", "This keybox has been placed already. Place this lock will end your last listing.");
            }
        }

        private async Task SubmitData()
        {
            if (IsListed)
            {
                await _keyboxService.EndKeyboxProperty(ConnectedKeybox.KeyboxId, ConnectedKeybox.PropertyId.Value);
            }

            var result = await _keyboxService.PlaceLock(ConnectedKeybox, _property);

            if (result)
            {
                await _messageBoxService.ShowMessageAsync("Success", "Your keybox has been placed successfully.");
                Pop();
            }
            else
            {
                await _messageBoxService.ShowMessageAsync("Failed", "Failed to place the keybox.");
            }
        }
    }
}
