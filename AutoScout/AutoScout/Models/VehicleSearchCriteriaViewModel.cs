using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoScout.Models
{
    public class VehicleSearchCriteriaViewModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int MinMileage { get; set; }
        public int MaxMileage { get; set; }
        public string Transmission { get; set; }
        public string Style { get; set; }
        public string Condition { get; set; }
        public int CylinderNumber { get; set; }
        public string ExteriorColor { get; set; }
    }
}