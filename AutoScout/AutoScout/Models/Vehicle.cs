using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoScout.Services;
using AutoScout.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoScout.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        [Display(Name = "VIN Number")]
        public string VIN { get; set; }

        public int Mileage { get; set; }

        [Display(Name = "Exterior Color")]
        public string ExteriorColor { get; set; }

        [Display(Name = "Interior Color")]
        public string InteriorColor { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        //dropdown options
        public string Transmission { get; set; }
        public string Style { get; set; }
        public string Condition { get; set; }
        public bool Visible { get; set; }
        public bool Active { get; set; }

        [Display(Name = "Number of Cylinders")]
        public int CylinderNumber { get; set; }

        public ICollection<VehicleImage> Images { get; set; }
        public VehicleImage DefaultImage { get; set; }
        public virtual Dealership Dealership { get; set; }
        public int DealershipId { get; set; }

        public Vehicle()
        {

            var context = new AutoScoutDBContext();
            var service = new DealershipAccountService(context);
            var dealershipId = service.GetCurrentUserDealershipIdFromIdentity();
            DealershipId = dealershipId;
            DateCreated = DateTime.Now.ToUniversalTime();

            Visible = true;
            Active = true;
        }

    }

    
}