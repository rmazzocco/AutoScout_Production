using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoScout.Services;
using AutoScout.Models;

//By: Evan Bauer & Ryan Mazzocco
//December 2016
//HomeController.cs
namespace AutoScout.Controllers
{
    public class HomeController : Controller
    {
        private AutoScoutDBContext db = new AutoScoutDBContext();

        public ActionResult Index()
        {
            try
            {
                var vehicleService = new VehicleSearchService(db);
                var imageService = new ImageManagementService(db);

                var newestVehicles = vehicleService.GetNewest3Vehicles();
                //var newestVehicleImages = imageService.GetImagesConvertedToBase64Strings();
                return View(newestVehicles);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public JsonResult GetNewestVehicles()
        {
            try
            {
                var vehicleService = new VehicleSearchService(db);
                var vehicles = vehicleService.GetNewest3Vehicles();
                var vehicleViewModels = new List<VehicleSearchCriteriaViewModel>();

                foreach (var item in vehicles)
                {
                    var companyName = db.Dealerships.FirstOrDefault(x => x.Id == item.DealershipId).CompanyName;
                    vehicleViewModels.Add(new VehicleSearchCriteriaViewModel(item.Id, item.Make, item.Model, item.Year, item.Price, item.Mileage, item.Transmission, item.Style, item.Condition, item.CylinderNumber, item.ExteriorColor, item.DealershipId, companyName));
                }
                return Json(vehicleViewModels, JsonRequestBehavior.AllowGet);
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