using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoScout.Models;

namespace AutoScout.Services
{
    public class ImageManagementService
    {
        private AutoScoutDBContext db;

        public ImageManagementService(AutoScoutDBContext dbContext)
        {
            db = dbContext;
        }

        public void AssignImageToVehicle(int vehicleId, HttpPostedFileBase imageFile)
        {
            try
            {
                if (imageFile != null)
                {
                    //create a byte array of appopriate length 
                    var imageBytes = new byte[imageFile.ContentLength];

                    //convert image file to byte array and assign to imageBytes
                    imageFile.InputStream.Read(imageBytes, 0, imageFile.ContentLength);

                    //create new vehicle image, assign VehicleId to Id of Vehicle of which the image belongs
                    var vehicleImage = new VehicleImage
                    {
                        VehicleId = vehicleId,
                        ImageBytes = imageBytes,
                    };

                    //add new vehicle image to database table - VehicleImages, ans save
                    db.VehicleImages.Add(vehicleImage);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Assign uploaded images to dealership profile header and icon
        public void AssignProfileImagesToDealership(int dealershipId, HttpPostedFileBase headerImageFile, HttpPostedFileBase iconImageFile)
        {
            try
            {
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == dealershipId);
                if (dealership != null) { 

                    if (headerImageFile != null)
                    {
                    
                        //create new byte array of appropriate length
                        var imageBytes = new byte[headerImageFile.ContentLength];

                        //read bytes from input file and convert to byte array, store in image bytes (empty byte array)
                        headerImageFile.InputStream.Read(imageBytes, 0, headerImageFile.ContentLength);

                        dealership.ProfileBackgroundImage = imageBytes;

                    }

                    if(iconImageFile != null)
                    {
                        //create new byte array of appropriate length
                        var imageBytes = new byte[iconImageFile.ContentLength];

                        //read bytes from input file and convert to byte array, store in image bytes (empty byte array)
                        iconImageFile.InputStream.Read(imageBytes, 0, iconImageFile.ContentLength);
                       
                        dealership.Icon = imageBytes;
                    }
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

        //Acquire string of base 64 strings, to pull vehicle images from the db to the view to be rendered in the browser
        public IEnumerable<string> GetImagesConvertedToBase64Strings(int id)
        {
            try
            {
                var result = new List<string>();

                //grab vehicle images with an id matching the vehicle id passed to as the function para,eter
                var vehicleImages = db.VehicleImages.Where(x => x.VehicleId == id).ToList();

                //convert each image from a byte array to a base64 string 
                foreach (var item in vehicleImages)
                {
                    var imageBytes = item.ImageBytes;
                    var imageBytesToBase64String = Convert.ToBase64String(imageBytes);
                    result.Add(imageBytesToBase64String);
                }
                
                //return the images in the form of a list of strings
                return result;
            } 
            catch(Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //get the profile icon associated with the dealership, converted to basse 64 string
        public string GetDealershipProfileIconAsBase64String(int id)
        {
            try
            {
                //grab the dealership's icon byte array, convert it to a base 64 string
                var dealershipIconImageBytes = db.Dealerships.FirstOrDefault(x => x.Id == id).Icon;
                var imageBytesToBase64String = Convert.ToBase64String(dealershipIconImageBytes);

                return imageBytesToBase64String;

            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //get the profile background image associated with the dealership, converted to basse 64 string
        public string GetDealershipProfileBackgroundAsBase64String(int id)
        {
            try
            {
                //grab the dealership's background byte array, convert it to a base 64 string
                var dealershipBackgroundImageBytes = db.Dealerships.FirstOrDefault(x => x.Id == id).ProfileBackgroundImage;
                var imageBytesToBase64String = Convert.ToBase64String(dealershipBackgroundImageBytes);

                return imageBytesToBase64String;

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