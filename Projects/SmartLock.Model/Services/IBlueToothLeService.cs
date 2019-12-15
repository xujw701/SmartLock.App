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
        event Action OnLocked;
        event Action OnUnlocked;

        bool IsOn { get; }
        List<BleDevice> DiscoveredDevices { get; }
        BleDevice ConnectedDevice { get; }
        bool DeviceConnected { get; }

        Task StartScanningForDevicesAsync();
        Task StopScanningForDevicesAsync();
        Task ConnectToDeviceAsync(BleDevice bleDevice);
        Task DisconnectDeviceAsync(BleDevice bleDevice);
        Task StartSetLock(bool isLock);
        Task<int> GetBatteryLevel();
    }
}
