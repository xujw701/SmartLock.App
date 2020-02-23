using System;

namespace SmartLock.Model.Models
{
    public class PropertyFeedback
    {
        public int PropertyFeedbackId { get; set; }
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int? ResPortraitId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }

        public string Name => $"{FirstName} {LastName}";
        public string CreatedOnString => CreatedOn.ToString("dd/MM/yy HH:mm");

        public string KeyboxName { get; set; }
    }
}
