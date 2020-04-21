﻿using SmartLock.Model.Models;
using SmartLock.Model.Server;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using System;
using System.Linq;
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

        private Keybox _lastKeybox;
        private Property _property;

        private bool _startConnecting = false;

        private Keybox ConnectedKeybox => _keyboxService.ConnectedKeybox;

        private bool CanPlaceLock => ConnectedKeybox != null
                && ConnectedKeybox.UserId.HasValue
                && ConnectedKeybox.UserId.Value == _userSession.UserId;

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
            _keyboxService.OnKeyboxConnected += (keybox) => DoSafeAsync(async () =>  await OnKeyboxConnected(keybox));
            _keyboxService.OnKeyboxDisconnected += OnKeyboxDisconnected;

            _keyboxService.OnUnlocked += () => { View.SetLockUI(false); };
            _keyboxService.OnLocked += () => { View.SetLockUI(true); };

            View.MessageClick += () => Push<PropertyFeedbackController>(vc => { vc.Mine = true; });
            View.PlaceKeyboxClicked += () =>
            {
                if (CanPlaceLock)
                {
                    Push<KeyboxPlaceController>(vc => vc.Keybox = ConnectedKeybox);
                }
            };

            View.StartStop += (isScanning) => DoSafeAsync(async () => await StartStopKeybox(isScanning));
            View.Connect += (keybox) => DoSafeAsync(async () => await Connect(keybox));
            View.Cancel += (keybox) => DoSafeAsync(async () => await Cancel(keybox));
            View.Dismiss += (keybox) => Dismiss(keybox);
            View.Close += () => DoSafeAsync(Close);
            View.UnlockClicked += () => DoSafeAsync(Unlock);

            View.BtClicked += () => _platformServices.Bt();
            View.Timeout += () => DoSafeAsync(Close);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_keyboxService.ConnectedKeybox != null)
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, false);
                View.Show(_keyboxService.ConnectedKeybox, CanPlaceLock);
            }
            else if (_keyboxService.DiscoveredKeyboxes != null && _keyboxService.DiscoveredKeyboxes.Where(k => !k.Dismissed).Count() > 0)
            {
                View.Show(GenerateGreeting(), _userSession.FirstName, false);
                View.Show(_keyboxService.DiscoveredKeyboxes.Where(k => !k.Dismissed).ToList());
            }
            else
            {
                View.Show(GenerateGreeting(), _userSession.FirstName);
            }
        }

        private void OnBleStateChanged(bool isOn)
        {
            View.SetBleIndicator(isOn);
        }

        private void OnKeyboxDiscovered(Keybox keybox)
        {
            UpdateUI();
        }

        private async Task OnKeyboxConnected(Keybox keybox)
        {
            UpdateUI();

            if (!_startConnecting) return;

            _startConnecting = false;

            if (keybox.KeyboxId > 0 && keybox.PropertyId.HasValue)
            {
                _property = await _keyboxService.GetBriefKeyboxProperty(keybox.KeyboxId, keybox.PropertyId.Value);

                await _messageBoxService.ShowMessageAsync("Data at door", _property.Notes);
            }

            View.StopCountDown();
            View.StartCountDown(15);
        }

        private void OnKeyboxDisconnected()
        {
            UpdateUI();
        }

        private async Task StartStopKeybox(bool isScanning)
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

        private async Task Connect(Keybox keybox)
        {
            _startConnecting = true;

            _lastKeybox = keybox;

            await _keyboxService.StopScanningForKeyboxesAsync();

            await _keyboxService.ConnectToKeyboxAsync(keybox);
        }

        private async Task Cancel(Keybox keybox)
        {
            _startConnecting = false;

            if (keybox != null)
            {
                await _keyboxService.DisconnectKeyboxAsync(keybox);
            }
        }

        private void Dismiss(Keybox keybox)
        {
            _startConnecting = false;

            if (keybox != null)
            {
                _keyboxService.DismssKeybox(keybox);

                UpdateUI();
            }
        }

        private async Task Close()
        {
            _startConnecting = false;

            View.StopCountDown();

            if (_keyboxService.ConnectedKeybox != null)
            {
                _lastKeybox = null;
                await _keyboxService.DisconnectKeyboxAsync(_keyboxService.ConnectedKeybox);
            }

            _keyboxService.Clear();

            UpdateUI();
        }

        private async Task Unlock()
        {
            var unlocked = await _keyboxService.StartUnlock();

            if (!unlocked)
            {
                await _messageBoxService.ShowMessageAsync("Unlock Failed", "The keybox isn't listed or you don't have permission to unlock it.");
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
            if (exception is WebServiceClientException webServiceClientException)
            {
                if (webServiceClientException.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await _messageBoxService.ShowMessageAsync("Tips", "Your credentials could not be authenticated. Please log in again.");
                    await LogOut();
                }
                else
                {
#if DEBUG
                await _messageBoxService.ShowMessageAsync("Error", exception.Message);
#endif
                }
            }
            else if (exception.Message.Contains("133"))
            {
                if (_lastKeybox != null)
                {
                    await Cancel(_lastKeybox);
                }

                _messageBoxService.ShowMessage("Tips", "The lock is already connected to another user now, please try it later.");
            }
            else if (exception is TaskCanceledException)
            {
                // Do nothing
            }
            else
            {
#if DEBUG
                await _messageBoxService.ShowMessageAsync("Error", exception.Message);
#endif
            }
        }

        private async Task LogOut()
        {
            await _userService.LogOut();

            PopToRoot();
        }
    }
}