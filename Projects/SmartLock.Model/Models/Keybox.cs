﻿using SmartLock.Model.Ble;
using System;

namespace SmartLock.Model.Models
{
    public class Keybox
    {
        public int KeyboxId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string Uuid { get; set; }
        public int? PropertyId { get; set; }
        public string KeyboxName { get; set; }
        public int BatteryLevel { get; set; }

        // Local var
        public DeviceState State { get; set; }

        // To be cleared
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Rooms { get; set; }
        public int Toilets { get; set; }
        public int Area { get; set; }
        public int Price { get; set; }

        public string BatteryLevelString => $"{BatteryLevel}%";
        public string AreaString => $"{Area}m2";
        public string PriceString => $"${String.Format("{0:n0}", Price)}";
    }
}
