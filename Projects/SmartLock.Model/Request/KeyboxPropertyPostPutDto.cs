using System;
namespace SmartLock.Model.Request
{
    public class KeyboxPropertyPostPutDto
    {
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string KeyboxName { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string Price { get; set; }
        public double? Bedrooms { get; set; }
        public double? Bathrooms { get; set; }
        public double? FloorArea { get; set; }
        public double? LandArea { get; set; }
    }
}
