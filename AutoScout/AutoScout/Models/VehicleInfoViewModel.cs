using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoScout.Models
{
    public class VehicleInfoViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string VIN { get; set; }
        public int Mileage { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Transmission { get; set; }
        public string Style { get; set; }
        public string Condition { get; set; }

        public VehicleInfoViewModel()
        {

        }
    }
}