using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoScout.Models;
using AutoScout.Services;
using Newtonsoft.Json;

namespace AutoScout.Controllers
{
    public class VehiclesController : Controller
    {
        private AutoScoutDBContext db = new AutoScoutDBContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            try
            {
                using (var db = new AutoScoutDBContext())
                {
                    var service = new DealershipAccountService(db);
                    int currentUserId = service.GetCurrentUserDealershipIdFromIdentity();
                    var dealership = db.Dealerships.FirstOrDefault(x => x.Id == currentUserId);

                    var allMakes = db.Vehicles.Select(x => x.Make).ToList().Distinct();
                    var allModels = db.Vehicles.Select(x => x.Model).ToList().Distinct();
                    var allExteriorColors = db.Vehicles.Select(x => x.ExteriorColor).ToList().Distinct();
                    var allYears = db.Vehicles.Select(x => x.Year).ToList().Distinct();

                    ViewBag.AllModels = allModels;
                    ViewBag.AllMakes = allMakes;
                    ViewBag.AllExteriorColors = allExteriorColors;
                    ViewBag.AllYears = allYears;

                    var vehicles = db.Vehicles.Where(x => x.Make != null).ToList();
                    if (vehicles.Count < 1 || vehicles == null)
                    {
                        ViewBag.ErrorMessage = "Sorry, your search returned no results. Please change your search criteria and try again.";
                    }
                    ViewBag.AllVehicles = vehicles.ToList();

                    return View(vehicles);
                }
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }
        

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Vehicle vehicle = db.Vehicles.Find(id);

                if (vehicle != null)
                {
                    string make = vehicle.Make;
                    string model = vehicle.Model;
                    string year = vehicle.Year.ToString();
                    string item = year + " " + make + " " + model;
                    ViewBag.stringDescription = item;
                }
                if (vehicle == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Images = db.VehicleImages.Where(x => x.VehicleId == id).ToList();

                return View(vehicle);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.DealershipId = new SelectList(db.Dealerships, "Id", "CompanyName");
                return View();
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VIN,Mileage,ExteriorColor,InteriorColor,Make,Model,Year,Price,Transmission,Style,Condition,CylinderNumber,TransmissionType")] Vehicle vehicle)
        {
            /*
            var dealershipService = new DealershipAccountService(db);
            var dealershipId = dealershipService.GetCurrentUserDealershipIdFromIdentity();
            vehicle.DealershipId = dealershipId;
            vehicle.DateCreated = DateTime.Now.ToUniversalTime();
            */
            try
            {
                if (ModelState.IsValid)
                {
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    return RedirectToAction("AddImage", new { id = vehicle.Id });
                }

                ViewBag.DealershipId = new SelectList(db.Dealerships, "Id", "CompanyName", vehicle.DealershipId);
                return View(vehicle);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Vehicle vehicle = db.Vehicles.Find(id);
                if (vehicle != null)
                {
                    string make = vehicle.Make;
                    string model = vehicle.Model;
                    string year = vehicle.Year.ToString();
                    string item = year + " " + make + " " + model;
                    ViewBag.StringDescription = item;
                }
                if (vehicle == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DealershipId = new SelectList(db.Dealerships, "Id", "CompanyName", vehicle.DealershipId);
                return PartialView(vehicle);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VIN,Mileage,ExteriorColor,InteriorColor,Make,Model,Year,Price,Transmission,Style,Condition,CylinderNumber,TransmissionType")] Vehicle vehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(vehicle).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //ViewBag.DealershipId = new SelectList(db.Dealerships, "Id", "CompanyName", vehicle.DealershipId);
                return View(vehicle);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Vehicle vehicle = db.Vehicles.Find(id);
                if (vehicle != null)
                {
                    string make = vehicle.Make;
                    string model = vehicle.Model;
                    string year = vehicle.Year.ToString();
                    string item = year + " " + make + " " + model;
                    ViewBag.stringDescription = item;
                }
                if (vehicle == null)
                {
                    return HttpNotFound();
                }
                return View(vehicle);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Vehicle vehicle = db.Vehicles.Find(id);
                ViewBag.VehicleMake = vehicle.Make;
                ViewBag.VehicleModel = vehicle.Model;
                ViewBag.Year = vehicle.Year;
                db.Vehicles.Remove(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //GET
        public ActionResult AddImage(int? id)
        {
            try
            {
                var vehicle = db.Vehicles.FirstOrDefault(x => x.Id == id);

                if (vehicle != null)
                {
                    string make = vehicle.Make;
                    string vModel = vehicle.Model;
                    string year = vehicle.Year.ToString();
                    string item = year + " " + make + " " + vModel;
                    ViewBag.stringDescription = item;
                }

                VehicleImage image = new VehicleImage();
                return View(image);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //GET - retrieve vehicles that meet custom search criteria
        public void SearchInventory()
        {
            try
            {
                var service = new VehicleSearchService(db);
                service.SearchInventory(null, null, 2005, -1, -1, -1, -1, null, null, null, -1, null);
                service.SearchInventory(null, null, -1, -1, -1, -1, -1, null, null, null, -1, "Silver");
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Get - retrieve vehicle search results
        public JsonResult GetSearchResults(string make, string model, string transmission, string style, string condition, int year, int minPrice, int maxPrice, int minMileage, int maxMileage, int cylinderNumber, string exteriorColor)
        {
            try
            {
                using (var db = new AutoScoutDBContext())
                {
                    //create instance of Vehicle Search Service
                    var vehicleSearchService = new VehicleSearchService(db);

                    //send search result criteria from parameters to SearchInventory method which will return matching vehicles
                    var searchResults = vehicleSearchService.SearchInventory(make, model, year, minPrice, maxPrice, minMileage, maxMileage, transmission, style, condition, cylinderNumber, exteriorColor);
                    var vehicleViewModels = new List<VehicleSearchCriteriaViewModel>();

                    foreach (var item in searchResults)
                    {
                        vehicleViewModels.Add(new VehicleSearchCriteriaViewModel(item.Id, item.Make, item.Model, item.Year, item.Price, item.Mileage, item.Transmission, item.Style, item.Condition, item.CylinderNumber, item.ExteriorColor));
                    }
                    return Json(vehicleViewModels, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        [HttpGet]
        public string GetVehicleDealershipName(int dealershipId)
        {
            try
            {
                var vehicleSearchService = new VehicleSearchService(db);
                var dealershipOfVehicle = vehicleSearchService.GetDealershipData(dealershipId);
                var name = dealershipOfVehicle.CompanyName;
                return name;
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

      
        [HttpGet]
        public JsonResult GetModelsFromMake(string make)
        {
            try
            {
                var service = new VehicleSearchService(db);
                var models = service.GetAllModelsFromMake(make);

                return Json(models, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

       
        //Obtain initial list of vehicles when loading filter search page
        [HttpGet]
        public JsonResult GetInitialVehicleListData()
        {
            try
            {
                var service = new VehicleSearchService(db);
                var vehicles = service.GetNewestVehicles();
                var vehicleViewModels = new List<VehicleSearchCriteriaViewModel>();

                foreach (var item in vehicles)
                {
                    vehicleViewModels.Add(new VehicleSearchCriteriaViewModel(item.Id, item.Make, item.Model, item.Year, item.Price, item.Mileage, item.Transmission, item.Style, item.Condition, item.CylinderNumber, item.ExteriorColor));
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

        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImage(int id, VehicleImage model, HttpPostedFileBase imageFile)
        {
            try
            {
                var db = new AutoScoutDBContext();
                //var vehicle = db.Vehicles.FirstOrDefault(x => x.Id == vehicleId);
                Vehicle vehicle = db.Vehicles.Find(id);

                if (vehicle != null)
                {
                    string make = vehicle.Make;
                    string vModel = vehicle.Model;
                    string year = vehicle.Year.ToString();
                    string item = year + " " + make + " " + vModel;
                    ViewBag.stringDescription = item;
                }

                if (imageFile != null)
                {
                    var service = new ImageManagementService(db);
                    service.AssignImageToVehicle(id, imageFile);

                }

                return View(model);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public ActionResult ListItem(Vehicle vehicle)
        {
            try
            {
                return PartialView(vehicle);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
