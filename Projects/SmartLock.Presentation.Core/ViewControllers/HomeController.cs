using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

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

            View.Title = "Pairing";
            View.StartStop += View_StartStop;
            View.Connect += View_Connect;
            View.UnlockClicked += View_UnlockClicked;
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

        private void View_UnlockClicked()
        {
            _trackedBleService.Unlock();
        }
    }
}