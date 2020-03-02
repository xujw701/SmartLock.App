using System;
namespace SmartLock.Model.Response
{
    public class KeyboxGetResponseDto
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

        // Extra
        public int AcessUserId { get; set; }
        public DateTime InOn { get; set; }
    }
}
