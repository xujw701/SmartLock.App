using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class HomeController : ViewController<IHomeView>
    {
        private readonly IBlueToothLeService _blueToothLeService;
        private readonly ITrackedBleService _trackedBleService;

        public HomeController(IViewService viewService, IBlueToothLeService blueToothLeService, ITrackedBleService trackedBleService) : base(viewService)
        {
            _blueToothLeService = blueToothLeService;
            _trackedBleService = trackedBleService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _blueToothLeService.OnDeviceDiscovered += BlueToothLeService_DeviceDiscovered;
            _blueToothLeService.OnDeviceConnected += BlueToothLeService_OnDeviceConnected;

            View.StartStop += (isScanning) => DoSafeAsync(async () => await View_StartStop(isScanning));
            View.Connect += (bleDevice) => DoSafeAsync(async () => await View_Connect(bleDevice));
            View.Disconnect += (bleDevice) => DoSafeAsync(async () => await View_Disconnect(bleDevice));
            View.DisconnectCurrent += () => DoSafeAsync(async () => await View_Disconnect(_blueToothLeService.ConnectedDevice));
            View.UnlockClicked += () => DoSafeAsync(View_UnlockClicked);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            if (_blueToothLeService.ConnectedDevice != null)
            {
                View.Show(GenerateGreeting(), _blueToothLeService.IsOn, false);
                View.Show(_blueToothLeService.ConnectedDevice);
            }
            else if (_blueToothLeService.DiscoveredDevices != null && _blueToothLeService.DiscoveredDevices.Count > 0)
            {
                View.Show(GenerateGreeting(), _blueToothLeService.IsOn, false);
                View.Show(_blueToothLeService.DiscoveredDevices);
            }
            else
            {
                View.Show(GenerateGreeting(), _blueToothLeService.IsOn);
            }
        }

        private void BlueToothLeService_DeviceDiscovered(Model.BlueToothLe.BleDevice bleDevice)
        {
           View.Show(_blueToothLeService.DiscoveredDevices);
        }

        private void BlueToothLeService_OnDeviceConnected(Model.BlueToothLe.BleDevice bleDevice)
        {
            View.Show(bleDevice);
        }

        private async Task View_StartStop(bool isScanning)
        {
            if (isScanning)
            {
                await _blueToothLeService.StartScanningForDevicesAsync();
            }
            else
            {
                await _blueToothLeService.StopScanningForDevicesAsync();
            }
        }

        private async Task View_Connect(Model.BlueToothLe.BleDevice bleDevice)
        {
            await _blueToothLeService.StopScanningForDevicesAsync();

            await _blueToothLeService.ConnectToDeviceAsync(bleDevice);
        }

        private async Task View_Disconnect(Model.BlueToothLe.BleDevice bleDevice)
        {
            if (bleDevice != null)
            {
                await _blueToothLeService.DisconnectDeviceAsync(bleDevice);
            }
        }

        private async Task View_UnlockClicked()
        {
            await _trackedBleService.StartUnlock();
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