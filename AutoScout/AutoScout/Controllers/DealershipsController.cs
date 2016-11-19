﻿using System;
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
                var dealerships = db.Dealerships.Include(d => d.AutoScoutIdentityUser);
                return View(await dealerships.ToListAsync());
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
        [HttpPost]
        public async Task<ActionResult> SetProfileImages(HttpPostedFileBase headerImageFile, HttpPostedFileBase iconImageFile)
        {
            try
            {
                //get the dealer id from dealership account service
                var dealershipService = new DealershipAccountService(db);
                var currentDealerId = dealershipService.GetCurrentUserDealershipIdFromIdentity();

                //add images to Dealership table using image service
                var dealership = await db.Dealerships.FirstOrDefaultAsync(x => x.Id == currentDealerId);
                if (dealership != null)
                {
                    var imageService = new ImageManagementService(db);
                    imageService.AssignProfileImagesToDealership(currentDealerId, headerImageFile, iconImageFile);
                }

                return View("ManageProfile", dealership);
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
                    PhoneNumber = dealership.PhoneNumber
                };

                /*
                var companyName = dealership.CompanyName;
                var email = dealership.Email;
                var city = dealership.City;
                var state = dealership.State;
                var zipcode = dealership.ZipCode;
                var notes = dealership.Notes;
                var phone = dealership.PhoneNumber;
                */

                return Json(dealershipEditManager, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        //Edit dealership info - using knockout
        /*[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(int id, string companyName, string email, string city, string state, string zipCode, string notes, string phoneNumber, string faxNumber)
        {
            try
            {
                var currentDealership = db.Dealerships.FirstOrDefault(x => x.Id == id);
                if (currentDealership != null)
                {
                    currentDealership.CompanyName = companyName;
                    currentDealership.Email = email;
                    currentDealership.City = city;
                    currentDealership.State = state;
                    currentDealership.Notes = notes;
                    currentDealership.PhoneNumber = phoneNumber;
                    currentDealership.ZipCode = zipCode;
                    currentDealership.FaxNumber = faxNumber;

                    db.SaveChanges();
                }
                return View(currentDealership);
            }
            catch(Exception exception)
            {
                throw (exception);
            }
            
        }
        */
        
        /*[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Dealership model)
        {
            if(model != null)
            {
                var currentDealership = db.Dealerships.FirstOrDefault(x => x.Id == model.Id);
                if (currentDealership != null)
                {
                    currentDealership.CompanyName = model.CompanyName;
                    currentDealership.Email = model.Email;
                    currentDealership.City = model.City;
                    currentDealership.State = model.State;
                    currentDealership.Notes = model.Notes;
                    currentDealership.PhoneNumber = model.PhoneNumber;
                    currentDealership.ZipCode = model.ZipCode;
                    currentDealership.FaxNumber = model.FaxNumber;

                    db.SaveChanges();
                }

                return Json("Edits were successfully changed.");
            }
            else
            {
                return Json("An error has occurred.");
            }
            
        }

    */
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
