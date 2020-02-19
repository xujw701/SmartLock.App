using System;
namespace SmartLock.Model.Response
{
    public class KeyboxGetResponseDto
    {
        public int KeyboxId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string Uuid { get; set; }
        public int? PropertyId { get; set; }
        public string KeyboxName { get; set; }
        public int BatteryLevel { get; set; }
    }
}
