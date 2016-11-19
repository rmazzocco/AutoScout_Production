using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoScout.Models;
using Microsoft.AspNet.Identity;

namespace AutoScout.Services
{
    public class ErrorService
    {
        private AutoScoutDBContext db;

        public ErrorService(AutoScoutDBContext dbContext)
        {
            db = dbContext;
        }

        public void logError(Exception ex)
        {
            try
            {
                var Error = new Error();

                if (ex.GetType() == null) { Error.ExceptionType = ""; } else { Error.ExceptionType = ex.GetType().ToString(); }
                if (ex.InnerException == null) { Error.InnerException = ""; } else { Error.InnerException = ex.InnerException.ToString(); }
                if (ex.StackTrace == null) { Error.StackTrace = ""; } else { Error.StackTrace = ex.StackTrace.ToString(); }

                db.Errors.Add(Error);
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

    }
}