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

        public IEnumerable<Vehicle> GetNewestVehicles()
        {
            var results = db.Vehicles.Where(x => x.Id > 0).OrderBy(x => x.DateCreated).ToList();
            return results;
        }

        public IEnumerable<string> GetAllMakes()
        {
            var makes = db.Vehicles.Select(x => x.Make).ToList().Distinct();
            return makes;
        }

        public IEnumerable<string> GetAllModels()
        {
            var models = db.Vehicles.Select(x => x.Model).ToList().Distinct();
            return models;
        }

        public IEnumerable<string> GetAllModelsFromMake(string make)
        {
            var models = db.Vehicles.Where(x => x.Make == make).Select(x => x.Model).ToList().Distinct();
            return models;
        }

        public IEnumerable<int> GetAllYears()
        {
            var years = db.Vehicles.Select(x => x.Year).ToList().Distinct();
            return years;
        }

        //query database to obtain vehicles that match search results
        public ICollection<Vehicle> SearchInventory(string make = "", string model = "", int year = -1, int minPrice = -1, int maxPrice = -1, int minMileage = -1, int maxMileage = -1, string transmission = "", string style = "", string condition = "", int cylinderNumber = -1, string exteriorColor = "")
        {
        
            //check if price configurations were made in the custom search
            var minPriceSet = minPrice > 0 ? true : false;
            var maxPriceSet = maxPrice > 0 ? true : false;

            //check if mileage configurations were made in the custom search
            var minMileageSet = minMileage > 0 ? true : false;
            var maxMileageSet = maxMileage > 0 ? true : false;

            /*set the price min & max, and the mileage min & max to be used in the query 
            based on whether custom values were submitted*/
            minPrice = minPriceSet == true ? minPrice : 0;
            maxPrice = maxPriceSet == true ? maxPrice : 0;
            minMileage = minMileageSet == true ? minMileage : 0;
            maxMileage = maxMileageSet == true ? maxMileage : 999999;

            /* query database of vehicles using user submitted search criteria - using Entity Framework */
            var results = db.Vehicles.Where(x => make != "" ? x.Make == make : x.Make != "" &&
                             make != "" ? x.Model == model : x.Model != "" &&
                             year > 0 ? x.Year == year : x.Year > 0 &&
                             transmission != "" ? x.Transmission == transmission : x.Transmission != "" &&
                             style != "" ? x.Style == style : x.Style != "" &&
                             cylinderNumber > 0 ? x.CylinderNumber == cylinderNumber : x.CylinderNumber > 0 &&
                             exteriorColor != "" ? x.ExteriorColor == exteriorColor: x.ExteriorColor != "" &&
                             x.Price > minPrice && x.Price < maxPrice &&
                             x.Mileage > minMileage && x.Mileage < maxMileage).ToList();

            //return list of vehicles matching criteria
            return results;
        }

        public Dealership GetDealershipData(int dealershipId)
        {
            var dealership = db.Dealerships.FirstOrDefault(x => x.Id == dealershipId);
            return dealership;
        }
    }
}
