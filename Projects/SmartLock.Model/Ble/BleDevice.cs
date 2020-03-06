using System;

namespace SmartLock.Model.Ble
{
    public class BleDevice
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rssi { get; set; }
        public object NativeDevice { get; set; }
        public DeviceState State { get; set; } = DeviceState.Disconnected;

        // Id.ToString() == RealId on Android
        // iOS has random Id, so will need this field
        public string RealId => GetRealId(Name);

        public int BatteryLevel { get; set; }
        public string BatteryLevelString => BatteryLevel > 0 ? $"{BatteryLevel}%" : "100%";

        public BleDevice(Guid id, string name, int rssi, object nativeDevice, DeviceState state)
        {
            Id = id;
            Name = name;
            Rssi = rssi;
            NativeDevice = nativeDevice;
            State = state;
        }

        public static string GetRealId(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.ToLower().StartsWith("lock-"))
            {
                var mac = name.ToLower().Replace("lock-", "");
                return $"00000000-0000-0000-0000-{mac}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
