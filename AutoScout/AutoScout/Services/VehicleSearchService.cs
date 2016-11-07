using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoScout.Models;

namespace AutoScout.Services
{
    class VehicleSearchService
    {
        private AutoScoutDBContext db;

        public VehicleSearchService(AutoScoutDBContext dbContext)
        {
            db = dbContext;
        }

        public ICollection<Vehicle> SearchInventory(string make = null, string model = null, int year = -1, int minPrice = -1, int maxPrice = -1, int minMileage = -1, int maxMileage = -1, string transmission = null, string style = null, string condition = null, int cylinderNumber = -1, string exteriorColor = null)
        {
        
            var minPriceSet = minPrice > 0 ? true : false;
            var maxPriceSet = maxPrice > 0 ? true : false;
            var minMileageSet = minMileage > 0 ? true : false;
            var maxMileageSet = maxMileage > 0 ? true : false;

            minPrice = minPriceSet == true ? minPrice : 0;
            maxPrice = maxPriceSet == true ? maxPrice : 0;
            minMileage = minMileageSet == true ? minMileage : 0;
            maxMileage = maxMileageSet == true ? maxMileage : 0;

            var results = db.Vehicles.Where(x => make != null ? x.Make == make : x.Make != null &&
                             make != null ? x.Model == model : x.Model != null &&
                             year > 0 ? x.Year == year : x.Year > 0 &&
                             transmission != null ? x.Transmission == transmission : x.Transmission != null &&
                             style != null ? x.Style == style : x.Style != null &&
                             cylinderNumber > 0 ? x.CylinderNumber == cylinderNumber : x.CylinderNumber > 0 &&
                             exteriorColor != null ? x.ExteriorColor == exteriorColor: x.ExteriorColor != null &&
                             x.Price > minPrice && x.Price < maxPrice &&
                             x.Mileage > minMileage && x.Mileage < maxMileage).ToList();

            return results;
        }
    }
}
