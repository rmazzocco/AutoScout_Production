function VehicleItemViewModel(id, make, model, year, style, price, mileage, exteriorColor, cylinderCount, transmission, condition, imageBytes, dealershipId, dealershipName) {
    var self = this;
    self.Id = id;
    self.Make = make;
    self.Model = model;
    self.Year = year + " ";
    self.Style = style;
    self.Price = "$" + price + ".00";
    self.Mileage = mileage;
    self.ExteriorColor = exteriorColor;
    self.CylinderCount = cylinderCount;
    self.Transmission = transmission;
    self.Condition = condition;
    self.ImageBytes = imageBytes;
    self.PreviewImage = "data:image/png;base64," + imageBytes;
    self.DealershipName = " " + dealershipName;
    self.DealershipId = dealershipId;

    self.ViewInfoPage = function () {
        window.location.href = $("#viewFullVehicleInfo").data("url") + "/" + self.Id;
    }

    self.EditVehicleInfo = function () {
        window.location.href = $("#editVehicleInfo").data("url") + "/" + self.Id;
    }

    self.DeleteVehicle = function () {
        var result = confirm("Are you sure you want to delete this vehicle from AutoScout?");
        if (result) {
            $.ajax({
                type: "GET",
                url: $("#deleteVehicle").data("url") + "/" + self.Id,
                success: function (data) {
                    
                }
            });
            location.reload();
        }
    };

}

function DealershipViewModel() {

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
        
    };

    self.LoadVehicles = function(){
        $.ajax({
            type: "GET",
            url: self.LoadVehicleUrl,
            data: {
                id: self.Id
            },
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    //after retirieving initial results for page, add the items to the Vehicles Array to be rendered to the screen on load
                    self.Vehicles.push(new VehicleItemViewModel(data[i].Id, data[i].Make, data[i].Model, data[i].Year, data[i].Style, data[i].Price, data[i].Mileage, data[i].ExteriorColor, data[i].CylinderNumber, data[i].Transmission, data[i].Condition, data[i].ImageBytes, data[i].DealershipIdNumber, data[i].DealershipName));
                }
            }
        });
    }

    self.AddVehicle = function() {
        window.location.href = $("#addVehicle").data("url");
    }

};



ko.applyBindings(DealershipViewModel);
