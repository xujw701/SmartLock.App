using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class PairingController : ViewController<IPairingView>
    {
        private readonly IBlueToothLeService _blueToothLeService;
        private readonly IMessageBoxService _messageBoxService;

        public PairingController(IViewService viewService, IBlueToothLeService blueToothLeService, IMessageBoxService messageBoxService) : base(viewService)
        {
            _blueToothLeService = blueToothLeService;
            _messageBoxService = messageBoxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _blueToothLeService.OnDeviceDiscovered += BlueToothLeService_DeviceDiscovered;
            _blueToothLeService.OnDeviceConnected += BlueToothLeService_OnDeviceConnected;

            View.Title = "Pairing";
            View.StartStop += View_StartStop;
            View.Connect += View_Connect;
            View.Show(_blueToothLeService.DiscoveredDevices);
        }

        private void BlueToothLeService_DeviceDiscovered(Model.BlueToothLe.BleDevice bleDevice)
        {
            View.Show(_blueToothLeService.DiscoveredDevices);
        }

        private void BlueToothLeService_OnDeviceConnected()
        {
            _messageBoxService.ShowMessage("", "Lock connected.");
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
    }
}