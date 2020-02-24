using SmartLock.Model.Ble;
using System;

namespace SmartLock.Model.Models
{
    public class Keybox
    {
        public int KeyboxId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int? UserId { get; set; }
        public string Uuid { get; set; }
        public int? PropertyId { get; set; }
        public string PropertyAddress { get; set; }
        public string KeyboxName { get; set; }
        public int BatteryLevel { get; set; }

        // Local var
        public DeviceState State { get; set; }

        public string BatteryLevelString => $"{BatteryLevel}%";
    }
}
