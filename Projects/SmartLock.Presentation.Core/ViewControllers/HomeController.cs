using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class HomeController : ViewController<IHomeView>
    {
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        public HomeController(IViewService viewService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.OnKeyboxDiscovered += OnKeyboxDiscovered;
            _keyboxService.OnKeyboxConnected += OnKeyboxConnected;

            View.StartStop += (isScanning) => DoSafeAsync(async () => await View_StartStop(isScanning));
            View.Connect += (keybox) => DoSafeAsync(async () => await View_Connect(keybox));
            View.Disconnect += (keybox) => DoSafeAsync(async () => await View_Disconnect(keybox));
            View.DisconnectCurrent += () => DoSafeAsync(async () => await View_Disconnect(_keyboxService.ConnectedKeybox));
            View.UnlockClicked += () => DoSafeAsync(View_UnlockClicked);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            if (_keyboxService.ConnectedKeybox != null)
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, _keyboxService.IsOn, false);
                View.Show(_keyboxService.ConnectedKeybox);
            }
            else if (_keyboxService.DiscoveredKeyboxes != null && _keyboxService.DiscoveredKeyboxes.Count > 0)
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, _keyboxService.IsOn, false);
                View.Show(_keyboxService.DiscoveredKeyboxes);
            }
            else
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, _keyboxService.IsOn);
            }
        }

        private void OnKeyboxDiscovered(Keybox keybox)
        {
            View.Show(_keyboxService.DiscoveredKeyboxes);
        }

        private void OnKeyboxConnected(Keybox keybox)
        {
            View.Show(keybox);
        }

        private async Task View_StartStop(bool isScanning)
        {
            if (isScanning)
            {
                await _keyboxService.StartScanningForKeyboxesAsync();
            }
            else
            {
                await _keyboxService.StopScanningForKeyboxesAsync();
            }
        }

        private async Task View_Connect(Keybox keybox)
        {
            await _keyboxService.StopScanningForKeyboxesAsync();

            await _keyboxService.ConnectToKeyboxAsync(keybox);
        }

        private async Task View_Disconnect(Keybox keybox)
        {
            if (keybox != null)
            {
                await _keyboxService.DisconnectKeyboxAsync(keybox);
            }
        }

        private async Task View_UnlockClicked()
        {
            await _keyboxService.StartUnlock();
        }

        private string GenerateGreeting()
        {
            var hourNow = int.Parse(DateTime.Now.ToString("HH"));
            if (hourNow >= 6 && hourNow < 12)
                return "Good morning,";
            else if (hourNow >= 12 && hourNow < 18)
                return "Good afternoon,";
            else if (hourNow >= 18 && hourNow < 22)
                return "Good evening,";
            else
                return "Good night,";
        }

        protected override async Task ShowErrorAsync(Exception exception)
        {
            var messageBoxService = Infrastructure.IoC.Resolve<IMessageBoxService>();

            if (exception is Newtonsoft.Json.JsonException)
            {
                await messageBoxService.ShowMessageAsync("Error", "Error parsing JSON");
            }
            else
            {
                if (exception.Message.Contains("133"))
                {
                    await messageBoxService.ShowMessageAsync("Tips", "The lock is already connected to another user now, please try it later.");
                }
                else
                {
                    //await messageBoxService.ShowMessageAsync("Error", exception.Message);
                }
            }
        }
    }
}