function VehicleInfoViewModel(){
    var self = this;
    self.Make = ko.observable("");
    self.Model = ko.observable("");
    self.Year = ko.observable("");
    self.VIN = ko.observable("");
    self.Mileage = ko.observable("");
    self.CompanyName = ko.observable("");
    self.ExteriorColor = ko.observable("");
    self.InteriorColor = ko.observable("");
    self.Price = ko.observable("");
    self.Transmission = ko.observable("");
    self.Style = ko.observable("");
    self.Condition = ko.observable("");
    self.Cylinders = ko.observable("");

    self.LoadCurrentDetails = function () {
        self.LoadVehicles();
        $.ajax({
            type: "GET",
            url: "Vehicles/GetCurrentStateDetails/",
            success: function (data) {
                self.Id(data.Id);
                self.Make(data.Make);
                self.Model(data.Model);
                self.Year(data.Year);
                self.VIN(data.VIN);
                self.Mileage(data.Mileage);
                self.CompanyName(data.CompanyName);
                self.ExteriorColor(data.PhoneNumber);
                self.InteriorColor(data.InteriorColor);
                self.Price(data.Price);
                self.Transmission(data.Transmission);
                self.Style(data.Style);
                self.Condition(data.Condition);

            }
        });

    };

    /*
    self.SaveChanges = function () {
        $.ajax({
            type: "POST",
            url: ""
        })
    }*/

    /*
    var self = this;

    $(document).ready(function () {
        self.LoadCurrentDetails();
    });

    self.Id = ko.observable("");
    self.CompanyName = ko.observable("");
    self.Email = ko.observable("");
    self.City = ko.observable("");
    self.State = ko.observable("");
    self.ZipCode = ko.observable("");
    self.PhoneNumber = ko.observable("");
    self.FaxNumber = ko.observable("");
    self.Notes = ko.observable("");

    self.Vehicles = ko.observableArray([]);
    self.LoadVehicleUrl = $("#loadVehicles").data("url");

    //load current dealership details from server, called on page load
    self.LoadCurrentDetails = function () {
        self.LoadVehicles();
        $.ajax({
            type: "GET",
            url: "Dealerships/GetCurrentDealershipInfo/",
            success: function (data) {
                self.Id(data.Id);
                self.CompanyName(data.CompanyName);
                self.Email(data.Email);
                self.City(data.City);
                self.State(data.State);
                self.ZipCode(data.ZipCode);
                self.PhoneNumber(data.PhoneNumber);
                self.FaxNumber(data.FaxNumber);
                self.Notes(data.Notes);
            }
        });

    };

    self.SaveDetailChanges = function () {
        $.ajax({
            type: "POST",
            url: "Dealerships/EditDetails/",
            data: {
                id: self.Id,
                companyName: self.CompanyName,
                email: self.Email,
                city: self.City,
                state: self.State,
                zipCode: self.ZipCode,
                phoneNumber: self.PhoneNumber,
                faxNumber: self.FaxNumber,
                notes: self.Notes
            },
            success: function (data) {
                alert("Your changes have been successfully saved.");
            }
        });

    };*/
}

ko.applyBindings(VehicleInfoViewModel);