using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Model.Models
{
    public class ResProperty
    {
        public int ResPropertyId { get; set; }
        public int PropertyId { get; set; }
        public string Url { get; set; }

        public Cache Image { get; set; }
        public bool ToDelete { get; set; }
    }
}
