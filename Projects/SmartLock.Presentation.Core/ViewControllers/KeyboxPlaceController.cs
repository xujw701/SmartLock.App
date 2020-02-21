using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxPlaceController : ViewController<IKeyboxPlaceView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IKeyboxService _keyboxService;

        private Property _property;

        public KeyboxPlaceController(IViewService viewService, IMessageBoxService messageBoxService, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
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
        }

        private async Task SubmitData()
        {
            var result = await _keyboxService.PlaceLock(_keyboxService.ConnectedKeybox, _property);

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
