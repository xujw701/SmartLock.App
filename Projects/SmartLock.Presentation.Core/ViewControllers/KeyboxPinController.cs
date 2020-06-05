using SmartLock.Infrastructure;
using SmartLock.Model.Models;
using SmartLock.Model.Server;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxPinController : ViewController<IKeyboxPinView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IKeyboxService _keyboxService;

        private string _newPin;

        public Keybox Keybox { get; set; }
        public event Action<bool> PinChanged;

        public KeyboxPinController(IViewService viewService, IMessageBoxService messageBoxService, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _keyboxService.PinChanged += (success) => View.OnPinChanged(success);

            View.BackClick += () => Pop();
            View.SubmitClick += (old, new1, new2) => DoSafeAsync(async () => await SubmitData(old, new1, new2), successAcction: () => View.IsBusy = true); ;
            View.PinChanged += (success) => DoSafeAsync(async () =>
            {
                if (success)
                {
                    await _keyboxService.UpdateKeyboxPin(_newPin);

                    _messageBoxService.ShowMessage("Success", "Your PIN has been updated successfully.");

                    Pop();
                }
                else
                {
                    _messageBoxService.ShowMessage("Failed", "Failed to change PIN, old PIN is incorrect.");
                }

                View.IsBusy = false;
            });
        }

        private async Task SubmitData(string old, string new1, string new2)
        {
            _newPin = null;

            if (string.IsNullOrEmpty(old) || string.IsNullOrEmpty(new1) || string.IsNullOrEmpty(new2))
            {
                throw new Exception("Please fill all the fields.");
            }
            if (!new1.Equals(new2))
            {
                throw new Exception("New PIN doesn't match, please check it again.");
            }

            if (!ValidatePins(old) || !ValidatePins(new2))
            {
                return;
            }

            try
            {
                _newPin = new1;

                await _keyboxService.StartChangePin(ConvertPin(old), ConvertPin(new1));
            }
            catch (Exception)
            {
                throw new Exception("Failed to change PIN.");
            }
        }

        private bool ValidatePins(string pin)
        {
            var pinByte = StringToByteArray(pin);

            if (pinByte.Length != 6)
            {
                throw new Exception("PIN must be 6 digits.");
            }

            foreach (var p in pinByte)
            {
                if (p != 1 && p != 2 && p != 3 && p != 4)
                {
                    throw new Exception("PIN must be numbers between 1 and 4.");
                }
            }

            return true;
        }

        private string ConvertPin(string pin)
        {
            var pinByte = StringToByteArray(pin);

            var result = string.Empty;

            foreach (var p in pinByte)
            {
                result = result + p.ToString("X2");
            }

            return result;
        }

        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             //.Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 1), 16))
                             .ToArray();
        }

        protected override async Task ShowErrorAsync(Exception exception)
        {
            var messageBoxService = IoC.Resolve<IMessageBoxService>();

            if (exception is WebServiceClientException webServiceClientException)
            {
                if (webServiceClientException.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await messageBoxService.ShowMessageAsync("Tips", "Your credentials could not be authenticated. Please log in again.");
                    await IoC.Resolve<IUserService>().LogOut();
                    ViewService.PopToRoot();
                }
                else
                {
                    await messageBoxService.ShowMessageAsync("Failed", exception.Message);
                }
            }
            else if (exception is TaskCanceledException)
            {
                // Do nothing
            }
            else
            {
                await messageBoxService.ShowMessageAsync("Failed", exception.Message);
            }
        }
    }
}