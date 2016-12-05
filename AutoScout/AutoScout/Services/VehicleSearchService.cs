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

        public Vehicle GetVehicleFromId(int id)
        {
            try
            {
                var vehicle = db.Vehicles.Find(id);
                return vehicle;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
            
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            try
            {
                var results = db.Vehicles.Where(x => x.Id > 0).OrderBy(x => x.DateCreated).ToList();
                return results;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public IEnumerable<Vehicle> GetNewest3Vehicles()
        {
            try
            {
                var results = db.Vehicles.Where(x => x.Id > 0).OrderBy(x => x.DateCreated).ToArray();
                var top3 = new List<Vehicle>();
                for(var i = 0; i < 3; ++i)
                {
                    top3.Add(results[i]);
                }
                return top3;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public string GetCompanyName(int dealershipId)
        {
            try
            {
                var dealership = db.Dealerships.Find(dealershipId);
                if (dealership != null)
                {
                    var result = dealership.CompanyName;
                    return result;

                }
                return "";
            } catch(Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
           
        }

        public IEnumerable<VehicleImage> GetAllVehicleImages(int id)
        {
            try
            {
                var imagesForVehicle = db.VehicleImages.Where(x => x.VehicleId == id);
                if (imagesForVehicle != null)
                {
                    var result = imagesForVehicle;
                    return imagesForVehicle.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
            
        }

        public VehicleImage GetVehicleImageFromVehicleImageId(int vehicleImageId)
        {
            try
            {
                var vehicleImage = db.VehicleImages.Find(vehicleImageId);
                return vehicleImage;
            }
            catch(Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public IEnumerable<string> GetAllMakes()
        {
            try
            {
                var makes = db.Vehicles.Select(x => x.Make).ToList().Distinct();
                return makes;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public IEnumerable<string> GetAllModels()
        {
            try
            {
                var models = db.Vehicles.Select(x => x.Model).ToList().Distinct();
                return models;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public IEnumerable<string> GetAllModelsFromMake(string make)
        {
            try
            {
                var models = db.Vehicles.Where(x => x.Make == make).Select(x => x.Model).ToList().Distinct();
                return models;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public IEnumerable<int> GetAllYears()
        {
            try
            {
                var years = db.Vehicles.Select(x => x.Year).ToList().Distinct();
                return years;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }


        //query database to obtain vehicles that match search results
        public ICollection<Vehicle> SearchInventory(string make = "", string model = "", int year = -1, int minPrice = -1, int maxPrice = -1, int minMileage = -1, int maxMileage = -1, string transmission = "", string style = "", string condition = "", int cylinderNumber = -1, string exteriorColor = "")
        {
            try
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
                maxPrice = maxPriceSet == true ? maxPrice : 999999;
                minMileage = minMileageSet == true ? minMileage : 0;
                maxMileage = maxMileageSet == true ? maxMileage : 999999;

                /* query database of vehicles using user submitted search criteria - using Entity Framework */
                var results = db.Vehicles.Where(x => (make != "" ? x.Make == make : x.Make != "") &&
                                 (model != "" ? x.Model == model : x.Model != "") &&
                                 (year > 0 ? x.Year == year : x.Year > 0) &&
                                 (transmission != "" ? x.Transmission == transmission : x.Transmission != "") &&
                                 (style != "" ? x.Style == style : x.Style != "") &&
                                 (cylinderNumber > 0 ? x.CylinderNumber == cylinderNumber : x.CylinderNumber > 0) &&
                                 (exteriorColor != "" ? x.ExteriorColor == exteriorColor : x.ExteriorColor != "") &&
                                 (x.Price > minPrice && x.Price < maxPrice) &&
                                 (x.Mileage > minMileage && x.Mileage < maxMileage)).ToList();

                //return list of vehicles matching criteria
                return results;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        

        public Dealership GetDealershipData(int dealershipId)
        {
            try
            {
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == dealershipId);
                return dealership;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }
    }
}
