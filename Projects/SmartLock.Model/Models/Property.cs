using System;

namespace SmartLock.Model.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string Price { get; set; }
        public double? Bedrooms { get; set; }
        public double? Bathrooms { get; set; }
        public double? FloorArea { get; set; }
        public double? LandArea { get; set; }

        public string FloorAreaString => FloorArea.HasValue ? $"{FloorArea.Value}m2" : "N/A";
        public string PriceString => $"${String.Format("{0:n0}", Price)}";
    }
}
