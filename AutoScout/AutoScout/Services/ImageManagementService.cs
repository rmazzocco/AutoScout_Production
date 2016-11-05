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
            catch(Exception ex)
            {
                throw ex;
                
            }
        }

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
                throw ex;

            }
        }
    }
}