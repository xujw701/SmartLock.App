using Android.Support.V7.App;
using Android.Widget;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Droid.Platform
{
    public class BlueToothLeService : IBlueToothLeService
    {
        private const string MainServiceId = "0000ffd5";
        private const string NotifyServiceId = "0000ffd0";
        private const string BatteryServiceId = "0000180f";

        private const string MainCharacteristicId = "0000ffd9";
        private const string NotifyCharacteristicId = "0000ffd4";
        private const string BatteryCharacteristicId = "00002a19";

        private const string ResponseLockActionHeader = "62";
        private const string ResponseLockActionLcoked = "F0";
        private const string ResponseLockActionUnlcoked = "0F";

        private AppCompatActivity Context => ViewBase.CurrentActivity;

        private IBluetoothLE _ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter _adapter;
        private IDevice _connectedDevice;

        // Raw devices
        private List<IDevice> _discoveredDevices;
        private List<BleDevice> _discoveredBleDevices;

        private ICharacteristic _mainCharacteristic;
        private ICharacteristic _notifyCharacteristic;
        private ICharacteristic _batteryCharacteristic;

        public event Action<BleDevice> OnDeviceDiscovered;
        public event Action<BleDevice> OnDeviceConnected;
        public event Action OnLocked;
        public event Action OnUnlocked;

        public bool IsOn => _ble.IsOn;
        public List<BleDevice> DiscoveredDevices => _discoveredBleDevices ?? new List<BleDevice>();
        public BleDevice ConnectedDevice => _connectedDevice != null ? new BleDevice(_connectedDevice.Id, _connectedDevice.Name, _connectedDevice.Rssi, _connectedDevice.NativeDevice, (DeviceState)_connectedDevice.State) : null;
        public bool DeviceConnected => _connectedDevice != null;

        public BlueToothLeService()
        {
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _adapter.DeviceDiscovered += Adapter_OnDeviceDiscovered;
            _adapter.DeviceConnected += Adapter_OnDeviceConnected;
        }

        public async void StartScanningForDevicesAsync()
        {
            // Clear the previous results
            _discoveredDevices = new List<IDevice>();
            _discoveredBleDevices = new List<BleDevice>();

            // Stop it first anyway
            await _adapter.StopScanningForDevicesAsync();

            await _adapter.StartScanningForDevicesAsync();
        }

        public async void StopScanningForDevicesAsync()
        {
            await _adapter.StopScanningForDevicesAsync();
        }

        public async void ConnectToDeviceAsync(BleDevice bleDevice)
        {
            var device = _discoveredDevices.FirstOrDefault(d => d.Id == bleDevice.Id);

            if (device == null) throw new Exception("Invalid device");

            await _adapter.ConnectToDeviceAsync(device);
        }

        public async void DisconnectDeviceAsync(BleDevice bleDevice)
        {
            var device = _discoveredDevices.FirstOrDefault(d => d.Id == bleDevice.Id);

            if (device == null) throw new Exception("Invalid device");

            await _adapter.DisconnectDeviceAsync(device);
        }

        public async void StartSetLock(bool isLock)
        {
            if (_mainCharacteristic == null) throw new Exception("Connect to a device first");

            byte[] command = null;

            if (isLock)
            {
                var closeCommand = StringToByteArray("FE434C4F5345000000FD");
                command = closeCommand;
            }
            else
            {
                var openCommand = StringToByteArray("FE4F50454E00000000FD");
                command = openCommand;
            }

            await _mainCharacteristic.WriteAsync(command);
        }

        public async void GetBatteryLevel()
        {
            if (_batteryCharacteristic == null) throw new Exception("Connect to a device first");

            var result = await _batteryCharacteristic.ReadAsync();

            Context.RunOnUiThread(() =>
            {
                var toast = Toast.MakeText(Context, "Battery " + result[0].ToString(), ToastLength.Short);
                toast.Show();
            });
        }

        private void Adapter_OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            var device = args.Device;

            if (string.IsNullOrEmpty(device.Name)) return;
            if (!string.IsNullOrEmpty(device.Name) && !device.Name.ToLower().StartsWith("lock")) return;

            var bleDevice = new BleDevice(device.Id, device.Name, device.Rssi, device.NativeDevice, (DeviceState)device.State);

            if (!_discoveredDevices.Contains(device))
            {
                _discoveredDevices.Add(device);
            }

            if (!_discoveredBleDevices.Contains(bleDevice))
            {
                _discoveredBleDevices.Add(bleDevice);
            }

            OnDeviceDiscovered?.Invoke(bleDevice);

//#if DEBUG
//            if (!string.IsNullOrEmpty(device.Name) && device.Name.ToLower().Contains("lock"))
//            {
//                ConnectToDeviceAsync(bleDevice);
//            }
//#endif
        }

        private async void Adapter_OnDeviceConnected(object sender, DeviceEventArgs args)
        {
            _connectedDevice = args.Device;

            var bleDevice = new BleDevice(_connectedDevice.Id, _connectedDevice.Name, _connectedDevice.Rssi, _connectedDevice.NativeDevice, (DeviceState)_connectedDevice.State);

            _mainCharacteristic = await FindCharacteristic(MainServiceId, MainCharacteristicId);
            _notifyCharacteristic = await FindCharacteristic(NotifyServiceId, NotifyCharacteristicId);
            _batteryCharacteristic = await FindCharacteristic(BatteryServiceId, BatteryCharacteristicId);

            Auth();

            if (_notifyCharacteristic != null)
            {
                _notifyCharacteristic.ValueUpdated += NotifyCharValueUpdated;
                await _notifyCharacteristic.StartUpdatesAsync();
            }

            Context.RunOnUiThread(() =>
            {
                OnDeviceConnected?.Invoke(bleDevice);
            });
        }

        private void NotifyCharValueUpdated(object sender, CharacteristicUpdatedEventArgs args)
        {
            var bytes = args.Characteristic.Value;

            if (bytes != null && bytes.Count() > 0)
            {
                var header = bytes[0].ToString("X2");

                if (!string.IsNullOrEmpty(header))
                {
                    // Response sent after Lock/Unlock
                    if (header.Equals(ResponseLockActionHeader))
                    {
                        var stat = bytes[1].ToString("X2");

                        if (string.IsNullOrEmpty(stat)) return;

                        if (stat.Equals(ResponseLockActionLcoked))
                            OnLocked?.Invoke();
                        else if (stat.Equals(ResponseLockActionUnlcoked))
                            OnUnlocked?.Invoke();
                    }
                }
            }

            // TODO: Validate the response
            //Context.RunOnUiThread(() =>
            //{
            //    var toast = Toast.MakeText(Context, $"response: {bytes[0].ToString("X2")} {bytes[1].ToString("X2")} {bytes[2].ToString("X2")} {bytes[3].ToString("X2")}", ToastLength.Short);
            //    toast.Show();
            //});
        }

        private async void Auth()
        {
            if (_mainCharacteristic == null) throw new Exception("Connect to a device first");

            var authCommand = StringToByteArray("2901020304010228");
            await _mainCharacteristic.WriteAsync(authCommand);

            await Task.Delay(50);
        }

        private async Task<ICharacteristic> FindCharacteristic(string serviceId, string characteristicId)
        {
            if (!DeviceConnected) throw new Exception("Connect to a device first");

            // Let it take a 50ms break first
            await Task.Delay(50);

            var services = await _connectedDevice.GetServicesAsync();

            var service = services.FirstOrDefault(srv => srv.Id != null && srv.Id.ToString().ToLower().StartsWith(serviceId));
            if (service != null)
            {
                var characteristics = await service.GetCharacteristicsAsync();
                if (characteristics != null)
                {
                    var characteristic = characteristics.FirstOrDefault(c => !string.IsNullOrEmpty(c.Uuid) && c.Uuid.ToLower().StartsWith(characteristicId));
                     
                    return characteristic;
                }
            }
            return null;
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static string ByteArrayToString(byte[] bytes)
        {
            var result = string.Empty;

            foreach(var b in bytes)
            {
                result = result + b.ToString("X2");
            }
            return result;
        }
    }
}
