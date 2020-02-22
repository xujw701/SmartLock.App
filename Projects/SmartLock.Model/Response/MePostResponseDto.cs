using System;
namespace SmartLock.Model.Response
{
    public class MePostResponseDto
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Individual { get; set; }
        public int UserRoleId { get; set; }
        public int? ResPortraitId { get; set; }
    }
}
