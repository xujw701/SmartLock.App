using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface IBlueToothLeService
    {
        List<BleDevice> DiscoveredDevices { get; }
        bool DeviceConnected { get; }

        void StartScanningForDevicesAsync();
        void StopScanningForDevicesAsync();
        void ConnectToDeviceAsync(BleDevice bleDevice);
        void SetLock(bool isLock);
        void GetBatteryLevel();
    }
}
