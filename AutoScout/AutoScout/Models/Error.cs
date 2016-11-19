using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoScout.Models
{
    public class Error
    {
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime TimeStamp { get; set; }

        public Error()
        {
            TimeStamp = DateTime.Now.ToUniversalTime();
        }
    }
}