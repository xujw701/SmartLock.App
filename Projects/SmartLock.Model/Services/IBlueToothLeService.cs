using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface IBlueToothLeService
    {
        event Action<BleDevice> OnDeviceDiscovered;
        event Action<BleDevice> OnDeviceConnected;

        bool IsOn { get; }
        List<BleDevice> DiscoveredDevices { get; }
        BleDevice ConnectedDevice { get; }
        bool DeviceConnected { get; }

        void StartScanningForDevicesAsync();
        void StopScanningForDevicesAsync();
        void ConnectToDeviceAsync(BleDevice bleDevice);
        void DisconnectDeviceAsync(BleDevice bleDevice);
        void SetLock(bool isLock);
        void GetBatteryLevel();
    }
}
