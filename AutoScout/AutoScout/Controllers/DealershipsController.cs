using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoScout.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AutoScout.Services;
using Newtonsoft.Json;

//By: Evan Bauer & Ryan Mazzocco
//December 2016
//VehiclesController.cs
namespace AutoScout.Controllers
{
    public class DealershipsController : Controller
    {
        private AutoScoutDBContext db = new AutoScoutDBContext();

        // GET: Dealerships
        public async Task<ActionResult> Index()
        {
            try
            {
                var dealershipService = new DealershipAccountService(db);
                var dealershipId = dealershipService.GetCurrentUserDealershipIdFromIdentity();
                Dealership dealership = await db.Dealerships.FindAsync(dealershipId);
                return View(dealership);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // GET: Dealerships/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Dealership dealership = await db.Dealerships.FindAsync(id);
                if (dealership == null)
                {
                    return HttpNotFound();
                }
                //use the image management service to get the background image converted to a base 64 string
                var imageService = new ImageManagementService(db);

                var iconString = "";
                var backgroundString = "";

                if (dealership.Icon != null)
                {
                    var dealershipIconImage = imageService.GetDealershipProfileIconAsBase64String(dealership.Id);
                    iconString = "data:image/png;base64," + dealershipIconImage;
                }

                if(dealership.ProfileBackgroundImage != null)
                {
                    var dealershipBackgroundImage = imageService.GetDealershipProfileBackgroundAsBase64String(dealership.Id);
                    backgroundString = "data:image/png;base64," + dealershipBackgroundImage;
                }

                //pass image base 64 strings to view using ViewBag
                ViewBag.Icon = iconString;
                ViewBag.Background = backgroundString;

                //acquire full vehicle inventory
                var dealershipService = new DealershipAccountService(db);
                var vehicles = dealershipService.GetAllVehiclesInInventory(dealership.Id);

                ViewBag.Vehicles = vehicles.ToList();

                return View(dealership);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        // GET: Dealerships/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Dealership dealership = await db.Dealerships.FindAsync(id);
                ViewBag.EditManager = new DealershipManager
                {
                    Id = dealership.Id,
                    CompanyName = dealership.CompanyName,
                    Email = dealership.Email,
                    City = dealership.City,
                    State = dealership.State,
                    ZipCode = dealership.ZipCode,
                    Notes = dealership.Notes,
                    PhoneNumber = dealership.PhoneNumber,
                    FaxNumber = dealership.FaxNumber
                };
                if (dealership == null)
                {
                    return HttpNotFound();
                }
                ViewBag.AutoScoutIdentityUserId = new SelectList(db.Dealerships, "Id", "Email", dealership.AutoScoutIdentityUserId);
                return View(dealership);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }
        
        // POST: Dealerships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CompanyName,Email,City,State,ZipCode,FaxNumber,Notes,PhoneNumber")] Dealership dealership)
        {
            try
            {
                var service = new DealershipAccountService(db);
                string identityId = service.GetCurrentUserIdentity();
                dealership.AutoScoutIdentityUserId = identityId;

                if (ModelState.IsValid)
                {
                    db.Entry(dealership).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.ResponseMessage = "Your changes have been saved.";
                }
                //ViewBag.AutoScoutIdentityUserId = new SelectList(db.Dealerships, "Id", "Email", dealership.AutoScoutIdentityUserId);
                return View("ManageProfile");
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //GET - manage profile page and vehicles active and viewable from public search
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageProfile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Dealership dealership = await db.Dealerships.FindAsync(id);
                return View(dealership);
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Post - set images for dealership profile's header image and icon
        [Authorize]
        [HttpPost]
        public ActionResult SetProfileImages(HttpPostedFileBase headerImageFile, HttpPostedFileBase iconImageFile)
        {
            try
            {
                //get the dealer id from dealership account service
                var dealershipService = new DealershipAccountService(db);
                var imageService = new ImageManagementService(db);
                var currentDealerId = dealershipService.GetCurrentUserDealershipIdFromIdentity();

                //add images to Dealership table using image service
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == currentDealerId);
                if (dealership != null)
                {
                    imageService.AssignProfileImagesToDealership(currentDealerId, headerImageFile, iconImageFile);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        [Authorize]
        [HttpPost]
        public void EditImages(HttpPostedFileBase headerImageFile, HttpPostedFileBase iconImageFile)
        {
            try
            {
                //get the dealer id from dealership account service
                var dealershipService = new DealershipAccountService(db);
                var currentDealerId = dealershipService.GetCurrentUserDealershipIdFromIdentity();

                //add images to Dealership table using image service
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == currentDealerId);
                if (dealership != null)
                {
                    var imageService = new ImageManagementService(db);
                    imageService.AssignProfileImagesToDealership(currentDealerId, headerImageFile, iconImageFile);
                }
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Get/Edit/ - return JSON for knockout.viewmodel
        [Authorize]
        public JsonResult GetCurrentDealershipInfo()
        {
            try
            {
                var service = new DealershipAccountService(db);
                var currentId = service.GetCurrentUserDealershipIdFromIdentity();
                var dealership = db.Dealerships.FirstOrDefault(x => x.Id == currentId);
                var dealershipEditManager = new DealershipManager
                {
                    Id = dealership.Id,
                    CompanyName = dealership.CompanyName,
                    Email = dealership.Email,
                    City = dealership.City,
                    State = dealership.State,
                    ZipCode = dealership.ZipCode,
                    Notes = dealership.Notes,
                    PhoneNumber = dealership.PhoneNumber,
                    FaxNumber = dealership.FaxNumber
                };

                return Json(dealershipEditManager, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        
        public JsonResult GetDealershipInfo(int id)
        {
            try
            {
                var service = new DealershipAccountService(db);
                var dealership = service.GetDealership(id);
                var dealershipEditManager = new DealershipManager
                {
                    Id = dealership.Id,
                    CompanyName = dealership.CompanyName,
                    Email = dealership.Email,
                    City = dealership.City,
                    State = dealership.State,
                    ZipCode = dealership.ZipCode,
                    Notes = dealership.Notes,
                    PhoneNumber = dealership.PhoneNumber,
                    FaxNumber = dealership.FaxNumber
                };

                return Json(dealershipEditManager, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Get the base64 string version of the current dealership's background image
        [Authorize]
        [HttpGet]
        public JsonResult GetProfileBackgroundImage(int id)
        {
            try
            {
                //use the image management service to get the background image converted to a base 64 string
                var imageService = new ImageManagementService(db);
                var dealershipBackgroundImage = imageService.GetDealershipProfileBackgroundAsBase64String(id);

                //create a new object easily converted to json to be acquired in the view
                var background = new ImageRenderViewModel(dealershipBackgroundImage);

                return Json(background, JsonRequestBehavior.AllowGet);

            } 
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Get the base64 string version of the current dealership's icon image
        [Authorize]
        [HttpGet]
        public string GetProfileIconImage(int id)
        {
            try
            {
                //use the image management service to get the background image converted to a base 64 string
                var imageService = new ImageManagementService(db);
                var dealershipIconImage = imageService.GetDealershipProfileIconAsBase64String(id);

                //create a new object easily converted to json to be acquired in the view
                //var icon = new ImageRenderViewModel(dealershipIconImage);

                return dealershipIconImage;

            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public JsonResult GetAllVehicles(int id)
        {
            try
            {
                var dealershipService = new DealershipAccountService(db);
                var vehicles = dealershipService.GetAllVehiclesInInventory(id).ToList();
                var vehicleViewModels = new List<VehicleSearchCriteriaViewModel>();

                foreach (var item in vehicles)
                {
                    var companyName = db.Dealerships.FirstOrDefault(x => x.Id == id).CompanyName;
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

        //Save changes to Dealership info
        [Authorize]
        [HttpPost]
        public void EditDetails(int id, string companyName, string email, string city, string state, string zipCode, string phoneNumber, string faxNumber, string notes)
        {
            try
            {
                var dealershipService = new DealershipAccountService(db);
                dealershipService.SaveDealershipDetails(id, companyName, email, city, state, zipCode, phoneNumber, faxNumber, notes);
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
