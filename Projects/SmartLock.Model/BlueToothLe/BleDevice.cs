using System;

namespace SmartLock.Model.BlueToothLe
{
    public class BleDevice
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rssi { get; set; }
        public object NativeDevice { get; set; }
        public DeviceState State { get; set; }

        public int BatteryLevel { get; set; }
        public string BatteryLevelString => BatteryLevel > 0 ? $"" : "UNKNOWN";

        public BleDevice(Guid id, string name, int rssi, object nativeDevice, DeviceState state)
        {
            Id = id;
            Name = "28e Oak View Terrace";//name;
            Rssi = rssi;
            NativeDevice = nativeDevice;
            State = state;
        }
    }

    public enum DeviceState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Limited = 3
    }
}
