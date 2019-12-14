using System;

namespace SmartLock.Model.BlueToothLe
{
    public class Keybox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int BatteryLevel { get; set; }

        public string BatteryLevelString => $"{BatteryLevel}%";
    }
}
