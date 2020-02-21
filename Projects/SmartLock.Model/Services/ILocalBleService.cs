using SmartLock.Model.Ble;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface ILocalBleService
    {
        event Action<BleDevice> OnDeviceDiscovered;
        event Action<BleDevice> OnDeviceConnected;
        event Action OnDeviceDisconnected;
        event Action OnLocked;
        event Action OnUnlocked;

        bool IsOn { get; }
        List<BleDevice> DiscoveredDevices { get; }
        BleDevice ConnectedDevice { get; }
        bool DeviceConnected { get; }

        Task StartScanningForDevicesAsync();
        Task StopScanningForDevicesAsync();
        Task ConnectToDeviceAsync(string uuid);
        Task DisconnectDeviceAsync(string uuid);
        Task StartSetLock(bool isLock);
        Task<int> GetBatteryLevel();
    }
}
