﻿    
function VehicleViewModel(id, make, model, year, style, price, mileage, exteriorColor, cylinderCount, transmission, condition, imageBytes) {
    var self = this;
    self.Id = id;
    self.Make = make;
    self.Model = model;
    self.Year = year;
    self.Style = style;
    self.Price = price;
    self.Mileage = mileage;
    self.ExteriorColor = exteriorColor;
    self.CylinderCount = cylinderCount;
    self.Transmission = transmission;
    self.Condition = condition;
    self.ImageBytes = imageBytes;
    self.PreviewImage = "data:image/png;base64," + imageBytes;
   
    /*self.GetDealershipName = function () {
        $.ajax({
            type: "GET",
            url: "Vehicles/GetDealershipName/",
            data: {
                dealershipId: self.DealershipId
            },
            success(data) {
                self.DealershipName(data);
            }

        })
    };
};*/
};

function FilterSearchViewModel() {
    var self = this;

    self.Vehicles = ko.observableArray([]);

    $(document).ready(function () {
        self.LoadData();
    });


    //get model types for user selected make
    self.LoadData = function () {
        $.ajax({
            type: "GET",
            url: "Vehicles/GetInitialVehicleListData/",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    //after retirieving initial results for page, add the items to the Vehicles Array to be rendered to the screen on load
                    self.Vehicles.push(new VehicleViewModel(data[i].Id, data[i].Make, data[i].Model, data[i].Year, data[i].Style, data[i].Price, data[i].Mileage, data[i].ExteriorColor, data[i].CylinderNumber, data[i].Transmission, data[i].Condition, data[i].ImageBytes));
                }
            }
        })
    };

    self.JsonTest = ko.observableArray([]);
    self.MakeTest = ko.observable("");
    self.ModelTest = ko.observable("");

    //options for dropdown menus
    self.MileageChoices = ko.observableArray([{ description: "< 50,000" }, { description: "50,000 - 100,000" }, { description: "100,000 - 150,000" }, { description: "150,000 - 200,000" }, { description: "> 200,000" }]);
    self.StyleChoices = ko.observableArray([{ description: "Coupe" }, { description: "Sedan" }, { description: "Van" }, { description: "Mini Van" }, { description: "SUV" }, { description: "Truck" }]);
    self.ConditionChoices = ko.observableArray([{ description: "Used" }, { description: "New" }]);
    self.CylinderChoices = ko.observableArray([{ description: 2 }, { description: 4 }, { description: 6 }, { description: 8 }]);
    self.TransmissionChoices = ko.observableArray([{ description: "Automatic" }, { description: "Manual" }]);
    

    /*subsribed observable values which are kept track of 
    using knockout, to acquire which fields of the search criteria 
    selections the user wishes to use  */
    self.useMakeField = ko.observable(false);
    self.useModelField = ko.observable(false);
    self.useCylinderField = ko.observable(false);
    self.useStyleField = ko.observable(false);
    self.useInteriorColorField = ko.observable(false);
    self.useExteriorColorField = ko.observable(false);
    self.useConditionField = ko.observable(false);
    self.useYearField = ko.observable(false);
    self.useMileageField = ko.observable(false);
    self.usePriceField = ko.observable(false);
    self.useTransmissionField = ko.observable(false);

    /* observable values uses to keep track of input control 
    values (text / dropdown values )*/
    self.Make = ko.observable("");
    self.Model = ko.observable("");
    self.CylinderCount = ko.observable("");
    self.Style = ko.observable("");
    self.InteriorColor = ko.observable("");
    self.ExteriorColor = ko.observable("");
    self.Condition = ko.observable("");
    self.Year = ko.observable("");
    self.Mileage = ko.observable("");
    self.MinimumPrice = ko.observable("");
    self.MaximumPrice = ko.observable("");
    self.Transmission = ko.observable("");

    self.MinimumMileage = ko.observable("");
    self.MaximumMileage = ko.observable("");

    /*if user chooses to search by a matching field, these values will be 
    filled with user input values to be sent to server to retrieve results*/
    self.MakeChecked = null;
    self.ModelChecked = null;
    self.YearChecked = -1;
    self.StyleChecked = null;
    self.TransmissionChecked = null;
    self.ConditionChecked = null;
    self.CylinderCountChecked = -1;
    self.ExteriorColorChecked = null;
    self.MinimumPriceChecked = -1;
    self.MaximumPriceChecked = -1;


    self.ModelChoices = ko.observableArray([]);
    
        


    //send all search criteria data to server to retreive matching results
    self.SendSearch = function () {

        //take Mileage choices, convert to minimum and maximum choices
        if (self.useMileageField()) {
            if (self.Mileage() === "< 50,000") {
                self.MinimumMileage(0);
                self.MaximumMileage(49999);
            }
            else if (self.Mileage() === "50,000 - 100,000") {
                self.MinimumMileage(50000);
                self.MaximumMileage(100000);
            }
            else if (self.Mileage() === "100,000 - 150,000") {
                self.MinimumMileage(100000);
                self.MaximumMileage(150000);
            }
            else if (self.Mileage() === "150,000 - 200,000") {
                self.MinimumMileage(150000);
                self.MaximumMileage(200000);
            }
            else if (self.Mileage() === "> 200,000") {
                self.MinimumMileage(200000);
                self.MaximumMileage(999999);
            } else {
                self.MinimumMileage(-1);
                self.MaximumMileage(-1);
            }
        } else {
            self.MinimumMileage(-1);
            self.MaximumMileage(-1);
        }


        //convert values to null if the user doesn't submit a choice for that criteria item
        if (self.useMakeField()) {
            self.MakeChecked = self.Make();
        }
        if (self.useModelField()) {
            self.ModelChecked = self.Model();
        }
        if (self.usePriceField()) {
            self.MinimumPriceChecked = self.MinimumPrice();
            self.MaximumPriceChecked = self.MaximumPrice();
        }
        if (self.useConditionField()) {
            self.ConditionChecked = self.Condition();
        }
        if (self.useCylinderField()) {
            self.CylinderCountChecked = self.CylinderCount();
        }
        if (self.useExteriorColorField()) {
            self.ExteriorColorChecked = self.ExteriorColor();
        }
        if (self.useStyleField()) {
            self.StyleChecked = self.Style();
        }
        if (self.useTransmissionField()) {
            self.TransmissionChecked = self.Transmission();
        }
        if (self.useYearField()){
            self.YearChecked = self.Year();
        }
            
        //send all custom search criteria to server to render desired vehicle selections
        $.ajax({
            type: "GET",
            url: /*"@Url.Action("GetSearchCriteriaResults")",*/ "Vehicles/GetSearchResults/",
            data: {
            make: self.MakeChecked,
            model: self.ModelChecked,
            transmission: self.TransmissionChecked,
            style: self.StyleChecked,
            condition: self.ConditionChecked,
            year: self.YearChecked,
            minPrice: self.MinimumPriceChecked,
            maxPrice: self.MaximumPriceChecked,
            minMileage: self.MinimumMileage(),
            maxMileage: self.MaximumMileage(),
            cylinderNumber: self.CylinderCountChecked,
            exteriorColor: self.ExteriorColorChecked

            },
            success: function (data) {
                self.Vehicles.removeAll();
                for (var i = 0; i < data.length; ++i) {
                    //after retrieving matching vehicles from server, replace the current rendered list of vehicles with the list of matching vehicles returned from the search
                    //every instance of a matched vehicle is initialized with a new VehicleViewModel to obtain all vehicle info
                    self.Vehicles.push(new VehicleViewModel(data[i].Id, data[i].Make, data[i].Model, data[i].Year, data[i].Style, data[i].Price, data[i].Mileage, data[i].ExteriorColor, data[i].CylinderNumber, data[i].Transmission, data[i].Condition));
                }
               
        }
    });
}

};

ko.applyBindings(FilterSearchViewModel);