using System;

namespace SmartLock.Model.BlueToothLe
{
    public class Keybox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int BatteryLevel { get; set; }
        public int Rooms { get; set; }
        public int Toilets { get; set; }
        public int Area { get; set; }
        public int Price { get; set; }

        public string BatteryLevelString => $"{BatteryLevel}%";
        public string AreaString => $"{Area}m2";
        public string PriceString => $"${String.Format("{0:n0}", Price)}";
    }
}
