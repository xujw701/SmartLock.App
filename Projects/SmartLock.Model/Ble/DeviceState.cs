using System;

namespace SmartLock.Model.Ble
{
    public enum DeviceState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Limited = 3
    }
}
