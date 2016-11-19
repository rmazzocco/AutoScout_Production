using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoScout.Models
{
    public class VehicleSearchCriteriaViewModel
    {
        //Model object used to coordinate passing data from client to server when using the custom vehicle search

        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string Transmission { get; set; }
        public string Style { get; set; }
        public string Condition { get; set; }
        public int CylinderNumber { get; set; }
        public string ExteriorColor { get; set; }
        public byte[] ImageBytes { get; set; }

        public VehicleSearchCriteriaViewModel()
        {
            var db = new AutoScoutDBContext();
            var imagebytes = db.VehicleImages.FirstOrDefault(x => x.VehicleId == Id).ImageBytes;
            ImageBytes = imagebytes;
        }

        public VehicleSearchCriteriaViewModel(int id, string make, string model, int year, decimal price, int mileage, string transmission, string style, string condition, int cylinderNumber, string exteriorColor)
        {
            Id = id;
            Make = make;
            Model = model;
            Year = year;
            Price = price;
            Mileage = mileage;
            Transmission = transmission;
            Style = style;
            Condition = condition;
            CylinderNumber = cylinderNumber;
            ExteriorColor = exteriorColor;

            var db = new AutoScoutDBContext();
            var imagebytes = db.VehicleImages.FirstOrDefault(x => x.VehicleId == Id).ImageBytes;
            ImageBytes = imagebytes;

        }

    }
}