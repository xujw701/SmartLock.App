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
    }
}
