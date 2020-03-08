using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Server;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class HomeController : ViewController<IHomeView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IUserService _userService;
        private readonly IKeyboxService _keyboxService;
        private readonly IPlatformServices _platformServices;

        private static int Timeout = 0;

        private Keybox _lastKeybox;

        public HomeController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IUserService userService, IKeyboxService keyboxService, IPlatformServices platformServices) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _userService = userService;
            _keyboxService = keyboxService;
            _platformServices = platformServices;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.OnBleStateChanged += OnBleStateChanged;
            _keyboxService.OnKeyboxDiscovered += OnKeyboxDiscovered;
            _keyboxService.OnKeyboxConnected += OnKeyboxConnected;
            _keyboxService.OnKeyboxDisconnected += OnKeyboxDisconnected;

            _keyboxService.OnUnlocked += () => { View.SetLockUI(false); };
            _keyboxService.OnLocked += () => { View.SetLockUI(true); };

            View.MessageClick += () => Push<PropertyFeedbackController>(vc => { vc.Mine = true; });
            View.StartStop += (isScanning) => DoSafeAsync(async () => await View_StartStop(isScanning));
            View.Connect += (keybox) => DoSafeAsync(async () => await View_Connect(keybox));
            View.Disconnect += (keybox) => DoSafeAsync(async () => await View_Disconnect(keybox));
            View.DisconnectCurrent += () => DoSafeAsync(async () => await View_Disconnect(_keyboxService.ConnectedKeybox));
            View.UnlockClicked += () => DoSafeAsync(View_UnlockClicked);
            View.BtClicked += () => _platformServices.Bt();
            View.Timeout += () => DoSafeAsync(Disconnect);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            if (_keyboxService.ConnectedKeybox != null)
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, false);
                View.Show(_keyboxService.ConnectedKeybox);
            }
            else if (_keyboxService.DiscoveredKeyboxes != null && _keyboxService.DiscoveredKeyboxes.Count > 0)
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, false);
                View.Show(_keyboxService.DiscoveredKeyboxes);
            }
            else
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, _keyboxService.IsOn);
            }
        }

        private void OnBleStateChanged(bool isOn)
        {
            View.SetBleIndicator(isOn);
        }

        private void OnKeyboxDiscovered(Keybox keybox)
        {
            View.Show(_keyboxService.DiscoveredKeyboxes);
        }

        private void OnKeyboxConnected(Keybox keybox)
        {
            View.Show(keybox);

            View.StartCountDown(30);
        }

        private void OnKeyboxDisconnected()
        {
            View.Show(GenerateGreeting(), _userSession.FirstName, _keyboxService.IsOn);
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
            _lastKeybox = keybox;

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
            var unlocked = await _keyboxService.StartUnlock();

            if (!unlocked)
            {
                await _messageBoxService.ShowMessageAsync("Unlock Failed", "The keybox isn't listed or you don't have permission to unlock it.");
            }
        }

        private async Task Disconnect()
        {
            if (_keyboxService.ConnectedKeybox != null)
            {
                await _keyboxService.DisconnectKeyboxAsync(_keyboxService.ConnectedKeybox);
            }
        }

        private string GenerateGreeting()
        {
            var hourNow = int.Parse(DateTimeOffset.Now.ToString("HH"));
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
            var webServiceClientException = exception as WebServiceClientException;

            if (webServiceClientException != null)
            {
                if (webServiceClientException.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await _messageBoxService.ShowMessageAsync("Tips", "Your credentials could not be authenticated. Please log in again.");
                    await LogOut();
                }
                else
                {
                    await _messageBoxService.ShowMessageAsync("Error", exception.Message);
                }
            }
            else if (exception.Message.Contains("133"))
            {
                if (_lastKeybox != null)
                {
                    await _keyboxService.DisconnectKeyboxAsync(_lastKeybox);
                    await _keyboxService.ConnectToKeyboxAsync(_lastKeybox);

                    return;
                }

                _messageBoxService.ShowMessage("Tips", "The lock is already connected to another user now, please try it later.");
            }
            else
            {
                //await _messageBoxService.ShowMessageAsync("Error", exception.Message);
            }
        }

        private async Task LogOut()
        {
            await _userService.LogOut();

            PopToRoot();
        }
    }
}