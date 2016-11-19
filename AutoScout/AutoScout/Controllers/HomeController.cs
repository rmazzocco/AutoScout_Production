using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoScout.Services;
using AutoScout.Models;

namespace AutoScout.Controllers
{
    public class HomeController : Controller
    {
        private AutoScoutDBContext db = new AutoScoutDBContext();

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public ActionResult About()
        {
            try
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }
            catch (Exception ex)
            {
                var errorService = new ErrorService(db);
                errorService.logError(ex);

                throw (ex);
            }
        }

        public ActionResult Contact()
        {
            try
            {
                var service = new DealershipAccountService(db);
                var dealershipId = service.GetCurrentUserDealershipIdFromIdentity();
                ViewBag.Name = service.GetCurrentUserNameFromDealershipId(dealershipId);

                return View();
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