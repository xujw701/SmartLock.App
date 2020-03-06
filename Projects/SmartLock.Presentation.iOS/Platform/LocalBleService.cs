using SmartLock.Model.Services;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;
using SmartLock.Model.Ble;
using Plugin.BLE;
using System.Linq;
using System;
using Plugin.BLE.Abstractions.EventArgs;
using System.Threading.Tasks;

namespace SmartLock.Presentation.iOS.Platform
{
    public class LocalBleService : ILocalBleService
    {
        private const string MainServiceId = "0000ffd5";
        private const string NotifyServiceId = "0000ffd0";
        private const string BatteryServiceId = "0000180f";

        private const string MainCharacteristicId = "ffd9";
        private const string NotifyCharacteristicId = "ffd4";
        private const string BatteryCharacteristicId = "2a19";

        private const string ResponseLockActionHeader = "62";
        private const string ResponseLockActionLcoked = "F0";
        private const string ResponseLockActionUnlcoked = "0F";

        private IBluetoothLE _ble;
        private IAdapter _adapter;
        private IDevice _connectedDevice;

        // Raw devices
        private List<IDevice> _discoveredDevices;
        private List<BleDevice> _discoveredBleDevices;

        private ICharacteristic _mainCharacteristic;
        private ICharacteristic _notifyCharacteristic;
        private ICharacteristic _batteryCharacteristic;

        public event Action<bool> OnBleStateChanged;
        public event Action<BleDevice> OnDeviceDiscovered;
        public event Action<BleDevice> OnDeviceConnected;
        public event Action OnDeviceDisconnected;
        public event Action OnLocked;
        public event Action OnUnlocked;

        public bool IsOn => _ble.IsOn;
        public List<BleDevice> DiscoveredDevices => _discoveredBleDevices;
        public BleDevice ConnectedDevice => _connectedDevice != null ? new BleDevice(_connectedDevice.Id, _connectedDevice.Name, _connectedDevice.Rssi, _connectedDevice.NativeDevice, (DeviceState)_connectedDevice.State) : null;
        public bool DeviceConnected => _connectedDevice != null;

        public LocalBleService()
        {
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _discoveredDevices = new List<IDevice>();
            _discoveredBleDevices = new List<BleDevice>();

            _ble.StateChanged += Ble_StateChanged;

            _adapter.DeviceDiscovered += Adapter_OnDeviceDiscovered;
            _adapter.DeviceConnected += Adapter_OnDeviceConnected;
            _adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
        }

        public async Task StartScanningForDevicesAsync()
        {
            Clear();

            // Stop it first anyway
            await _adapter.StopScanningForDevicesAsync();

            // Set the scan mode fast only
            _adapter.ScanMode = ScanMode.LowLatency;

            await _adapter.StartScanningForDevicesAsync();
        }

        public async Task StopScanningForDevicesAsync()
        {
            await _adapter.StopScanningForDevicesAsync();
        }

        public async Task ConnectToDeviceAsync(string uuid)
        {
            var device = _discoveredDevices.FirstOrDefault(d => BleDevice.GetRealId(d.Name).Equals(uuid));

            if (device == null) return; //throw new Exception("Invalid device");

            await _adapter.ConnectToDeviceAsync(device);
        }

        public async Task DisconnectDeviceAsync(string uuid)
        {
            var device = _discoveredDevices.FirstOrDefault(d => BleDevice.GetRealId(d.Name).Equals(uuid));

            if (device == null) return; //throw new Exception("Invalid device");

            await _adapter.DisconnectDeviceAsync(device);

            Clear();
        }

        public async Task StartSetLock(bool isLock)
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

        public async Task<int> GetBatteryLevel()
        {
            if (_batteryCharacteristic == null) throw new Exception("Connect to a device first");

            var result = await _batteryCharacteristic.ReadAsync();

            return int.Parse(result[0].ToString());
            //Context.RunOnUiThread(() =>
            //{
            //    var toast = Toast.MakeText(Context, "Battery " + result[0].ToString(), ToastLength.Short);
            //    toast.Show();
            //});
        }

        public void Clear()
        {
            // Clear the previous results
            _discoveredDevices = new List<IDevice>();
            _discoveredBleDevices = new List<BleDevice>();

            _connectedDevice = null;
        }

        private void Ble_StateChanged(object sender, BluetoothStateChangedArgs e)
        {
            OnBleStateChanged?.Invoke(e.NewState == BluetoothState.On);
        }

        private void Adapter_OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            var device = args.Device;

            if (string.IsNullOrEmpty(device.Name)) return;
            if (!string.IsNullOrEmpty(device.Name) && !device.Name.ToLower().StartsWith("lock")) return;

            var bleDevice = new BleDevice(device.Id, device.Name, device.Rssi, device.NativeDevice, (DeviceState)device.State);

            if (!_discoveredDevices.Exists(d => d.Id.ToString().Equals(device.Id.ToString())))
            {
                _discoveredDevices.Add(device);
            }

            if (!_discoveredBleDevices.Exists(d => d.Id.Equals(device.Id.ToString())))
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
            try
            {
                _connectedDevice = args.Device;

                var bleDevice = new BleDevice(_connectedDevice.Id, _connectedDevice.Name, _connectedDevice.Rssi, _connectedDevice.NativeDevice, (DeviceState)_connectedDevice.State);

                _mainCharacteristic = await FindCharacteristic(MainServiceId, MainCharacteristicId);
                _notifyCharacteristic = await FindCharacteristic(NotifyServiceId, NotifyCharacteristicId);
                _batteryCharacteristic = await FindCharacteristic(BatteryServiceId, BatteryCharacteristicId);

                await Auth();

                if (_notifyCharacteristic != null)
                {
                    _notifyCharacteristic.ValueUpdated += NotifyCharValueUpdated;
                    await _notifyCharacteristic.StartUpdatesAsync();
                }

                bleDevice.BatteryLevel = await GetBatteryLevel();

                OnDeviceConnected?.Invoke(bleDevice);
            }
            catch (Exception e)
            {
            }
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            OnDeviceDisconnected?.Invoke();
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

        private async Task Auth()
        {
            if (_mainCharacteristic == null) throw new Exception("Connect to a device first");

            var authCommand = StringToByteArray("2901020304010228");
            await _mainCharacteristic.WriteAsync(authCommand);

            await Task.Delay(50);

            //Context.RunOnUiThread(() =>
            //{
            //    var toast = Toast.MakeText(Context, "Connected", ToastLength.Short);
            //    toast.Show();
            //});
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

            foreach (var b in bytes)
            {
                result = result + b.ToString("X2");
            }
            return result;
        }
    }
}