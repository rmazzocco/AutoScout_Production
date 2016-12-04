using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoScout.Models;
using Microsoft.AspNet.Identity;

namespace AutoScout.Services
{
    public class DealershipAccountService
    {
        private AutoScoutDBContext db; 

        public DealershipAccountService(AutoScoutDBContext dbContext)
        {
            db = dbContext;
        }

        public void RegisterDealershipAccount(string companyName, string autoScoutIdentityUserId, string email, string city, string state, string zipCode, string phoneNumber, string faxNumber, string notes)
        {
            try
            {
                var db = new AutoScoutDBContext();
                var dealership = new Dealership { CompanyName = companyName, AutoScoutIdentityUserId = autoScoutIdentityUserId, Email = email, City = city, State = state, ZipCode = zipCode, PhoneNumber = phoneNumber, FaxNumber = faxNumber, Notes = notes };
                db.Dealerships.Add(dealership);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public int GetCurrentUserDealershipIdFromIdentity()
        {
            try
            {
                var identityId = HttpContext.Current.User.Identity.GetUserId();
                if (identityId != null)
                {
                    var dealershipId = db.Dealerships.FirstOrDefault(x => x.AutoScoutIdentityUserId == identityId).Id;
                    return dealershipId;
                }

                return -1;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public string GetCurrentUserIdentity()
        {
            try
            {
                var identityId = HttpContext.Current.User.Identity.GetUserId();
                return identityId;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public string GetCurrentUserNameFromDealershipId(int dealershipId)
        {
            try
            {
                var companyName = db.Dealerships.FirstOrDefault(x => x.Id == dealershipId).CompanyName;
                return companyName;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public void SaveDealershipDetails(int id, string companyName, string email, string city, string state, string zipCode, string phoneNumber, string faxNumber, string notes)
        {
            try
            {
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == id);
                dealership.CompanyName = companyName;
                dealership.Email = email;
                dealership.City = city;
                dealership.State = state;
                dealership.ZipCode = zipCode;
                dealership.PhoneNumber = phoneNumber;
                dealership.FaxNumber = faxNumber;
                dealership.Notes = notes;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public IEnumerable<Vehicle> GetAllVehiclesInInventory(int id)
        {
            try
            {
                var inventoryVehicles = db.Vehicles.Where(x => x.DealershipId == id).ToList();
                return inventoryVehicles;
            } 
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
           
        }

        public void DeleteVehicleFromInventory(int vehicleId, int dealershipId)
        {
            try
            {
                var ownerOfVehicleId = db.Vehicles.FirstOrDefault(x => x.Id == vehicleId).DealershipId;
                var currentUserId = GetCurrentUserDealershipIdFromIdentity();
                if(ownerOfVehicleId == currentUserId)
                {
                    var vehicle = db.Vehicles.FirstOrDefault(x => x.Id == vehicleId);
                    db.Vehicles.Remove(vehicle);
                    db.SaveChanges();
                    
                }
                return;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //create a new vehicle with parameters given, along with vehicle images associated with the vehicle, and add them to the database
        public void CreateInventoryVehicle(int id, string vin, int mileage, string exteriorColor, string interiorColor, string make, string model, int year, decimal price, string condition, int cylinderNumber, string transmissionType, HttpPostedFileBase[] imageFiles)
        {

            try
            {
                var currentDealershipId = GetCurrentUserDealershipIdFromIdentity();
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == currentDealershipId);
                var newVehicle = new Vehicle
                {
                    VIN = vin,
                    Mileage = mileage,
                    ExteriorColor = exteriorColor,
                    InteriorColor = interiorColor,
                    Make = make,
                    Model = model,
                    Year = year,
                    Price = price,
                    Condition = condition,
                    CylinderNumber = cylinderNumber,
                    Transmission = transmissionType,
                    DealershipId = currentDealershipId
                };


                var vehicleImageList = new List<VehicleImage>();
                for (int i = 0; i < imageFiles.Count(); i++)
                {
                    //create new byte array of appropriate length
                    var imageBytes = new byte[imageFiles[i].ContentLength];

                    //read bytes from input file and convert to byte array, store in image bytes (empty byte array)
                    imageFiles[i].InputStream.Read(imageBytes, 0, imageFiles[i].ContentLength);


                    var vehicleImage = new VehicleImage
                    {
                        ImageBytes = imageBytes,
                        VehicleId = id
                    };

                    vehicleImageList.Add(vehicleImage);
                }

                //Add vehicle to Vehicles table 
                db.Vehicles.Add(newVehicle);

                //Add vehicle images to VehicleImages table
                foreach (var item in vehicleImageList)
                {
                    db.VehicleImages.Add(item);
                }

                db.SaveChanges();
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