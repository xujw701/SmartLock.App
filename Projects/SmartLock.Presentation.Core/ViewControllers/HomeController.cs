using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;

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

            View.StartStop += (isScanning) => DoSafe(() => View_StartStop(isScanning));
            View.Connect += (bleDevice) => DoSafe(() => View_Connect(bleDevice));
            View.Disconnect += (bleDevice) => DoSafe(() => View_Disconnect(bleDevice));
            View.UnlockClicked += () => DoSafe(View_UnlockClicked);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(GenerateGreeting(), _blueToothLeService.IsOn);
        }

        private void BlueToothLeService_DeviceDiscovered(Model.BlueToothLe.BleDevice bleDevice)
        {
           View.Show(_blueToothLeService.DiscoveredDevices);
        }

        private void BlueToothLeService_OnDeviceConnected(Model.BlueToothLe.BleDevice bleDevice)
        {
            View.Show(bleDevice);
        }

        private void View_StartStop(bool isScanning)
        {
            if (isScanning)
            {
                _blueToothLeService.StartScanningForDevicesAsync();
            }
            else
            {
                _blueToothLeService.StopScanningForDevicesAsync();
            }
        }

        private void View_Connect(Model.BlueToothLe.BleDevice bleDevice)
        {
            _blueToothLeService.StopScanningForDevicesAsync();

            _blueToothLeService.ConnectToDeviceAsync(bleDevice);
        }

        private void View_Disconnect(Model.BlueToothLe.BleDevice bleDevice)
        {
            _blueToothLeService.DisconnectDeviceAsync(bleDevice);
        }

        private void View_UnlockClicked()
        {
            _trackedBleService.StartUnlock();
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
    }
}