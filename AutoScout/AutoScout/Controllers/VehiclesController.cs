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

namespace AutoScout.Controllers
{
    public class VehiclesController : Controller
    {
        private AutoScoutDBContext db = new AutoScoutDBContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            using (var db = new AutoScoutDBContext())
            {
                var service = new DealershipAccountService(db);
                int currentUserId = service.GetCurrentUserDealershipIdFromIdentity();
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == currentUserId);
                /*if (currentUserId != 0 && currentUserId != 1)
                {
                    var vehicles = db.Vehicles.Where(x => x.DealershipId == currentUserId);
                    ViewBag.CompanyName = dealership.CompanyName;
                    return View(vehicles.ToList());
                }
                else
                {
                    var vehicles = db.Vehicles.Where(x => x.Make != null).ToList();
                    return View(vehicles);
                }*/
                var allMakes = db.Vehicles.Select(x => x.Make).ToList().Distinct();
                var allModels = db.Vehicles.Select(x => x.Model).ToList().Distinct();
                var allExteriorColors = db.Vehicles.Select(x => x.ExteriorColor).ToList().Distinct();
                ViewBag.AllModels = allModels;
                ViewBag.AllMakes = allMakes;
                ViewBag.AllExteriorColors = allExteriorColors;
                
                var vehicles = db.Vehicles.Where(x => x.Make != null).ToList();
                if(vehicles.Count < 1 || vehicles == null)
                {
                    ViewBag.ErrorMessage = "Sorry, your search returned no results. Please change your search criteria and try again.";
                }
                return View(vehicles);
            }
        }
        

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);

            if(vehicle != null)
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

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            ViewBag.DealershipId = new SelectList(db.Dealerships, "Id", "CompanyName");
            return View();
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

            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("AddImage", new { id = vehicle.Id });
            }

            ViewBag.DealershipId = new SelectList(db.Dealerships, "Id", "CompanyName", vehicle.DealershipId);
            return PartialView(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VIN,Mileage,ExteriorColor,InteriorColor,Make,Model,Year,Price,Transmission,Style,Condition,CylinderNumber,TransmissionType")] Vehicle vehicle)
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

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            ViewBag.VehicleMake = vehicle.Make;
            ViewBag.VehicleModel = vehicle.Model;
            ViewBag.Year = vehicle.Year;
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET
        public ActionResult AddImage(int? id)
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

        //GET - retrieve vehicles that meet custom search criteria
        public void SearchInventory()
        {
            var service = new VehicleSearchService(db);
            service.SearchInventory(null, null, 2005, -1, -1, -1, -1, null, null, null, -1, null);
            service.SearchInventory(null, null, -1, -1, -1, -1, -1, null, null, null, -1, "Silver");
            
        }

        [HttpGet]
        public JsonResult GetSearchCriteriaResults(VehicleSearchCriteriaViewModel model)
        {
            var pleaseWork = model;
            var make = model.Make;
            var message = "hopefully it worked";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImage(int id, VehicleImage model, HttpPostedFileBase imageFile)
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

        public ActionResult ListItem(Vehicle vehicle)
        {
            return PartialView(vehicle);
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
