using System;
namespace SmartLock.Model.Response
{
    public class KeyboxHistoryGetResponseDto
    {
        public int KeyboxHistoryId { get; set; }
        public int KeyboxId { get; set; }
        public int UserId { get; set; }
        public int? TmpUserId { get; set; }
        public int PropertyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ResPortraitId { get; set; }
        public DateTimeOffset InOn { get; set; }
        public DateTimeOffset? OutOn { get; set; }
    }
}
