using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using System;
using System.Linq;
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
            View.SubmitClick += (old, new1, new2) => DoSafeAsync(async () => await SubmitData(old, new1, new2));
            View.PinChanged += (success) => DoSafeAsync(async () =>
            {
                if (success)
                {
                    await _keyboxService.UpdateKeyboxPin(_newPin);

                    _messageBoxService.ShowMessage("Success", "Your PIN has been updated successfully.");
                }
                else
                {
                    _messageBoxService.ShowMessage("Failed", "Failed to change PIN, old PIN is incorrect.");
                }
            });
        }

        private async Task SubmitData(string old, string new1, string new2)
        {
            _newPin = null;

            if (string.IsNullOrEmpty(old) || string.IsNullOrEmpty(new1) || string.IsNullOrEmpty(new2))
            {
                _messageBoxService.ShowMessage("Failed", "Please fill all the fields.");

                return;
            }
            if (!new1.Equals(new2))
            {
                _messageBoxService.ShowMessage("Failed", "New PIN doesn't match, please check it again.");

                return;
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
                _messageBoxService.ShowMessage("Failed", "Failed to change PIN.");

                return;
            }
        }

        private bool ValidatePins(string pin)
        {
            var pinByte = StringToByteArray(pin);

            if (pinByte.Length != 6)
            {
                _messageBoxService.ShowMessage("Failed", "PIN must be 6 digits.");

                return false;
            }

            foreach (var p in pinByte)
            {
                if (p != 1 && p != 2 && p != 3 && p != 4)
                {
                    _messageBoxService.ShowMessage("Failed", "PIN must be numbers between 1 and 4.");

                    return false;
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
    }
}