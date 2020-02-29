using System;
using System.Collections.Generic;

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
        public string PriceString => GetPriceString();

        private string GetPriceString()
        {
            if (double.TryParse(Price, out double result))
            {
                return $"$ {String.Format("{0:n0}", result)}";
            }

            return Price;
        }

        public List<ResProperty> PropertyResource { get; set; } = new List<ResProperty>();
        public List<Cache> ToUploadResource { get; set; } = new List<Cache>();
    }
}
