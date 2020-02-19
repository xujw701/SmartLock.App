using System;
namespace SmartLock.Model.Response
{
    public class PropertyFeedbackGetResponseDto
    {
        public int PropertyFeedbackId { get; set; }
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
