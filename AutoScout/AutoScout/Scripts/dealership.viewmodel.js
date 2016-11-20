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

    //load current dealership details from server, called on page load
    self.LoadCurrentDetails = function () {
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

    /*self.HeaderImageFile = ko.observable(null);
    self.IconImageFile = ko.observable(null);

    self.SaveImages = function () {
        $.ajax({
            type: "POST",
            url: "Dealerships/EditImages/",
            data: {
                headerImageFile: self.HeaderImageFile,
                iconImageFile: self.IconImageFile
            },
            success: function (success) {
                alert("Your changes have been successfully saved;")
            }
        });
    };

    
    self.HeaderImageName = ko.computed(function () {
        return !!self.HeaderImageFile() ? self.HeaderImageFile().name : '-';
    });

    self.IconImageName = ko.computed(function () {
        return !!self.IconImageFile() ? self.IconImageFile().name : '-';
    });

    self.Clear = function () {
        self.HeaderImageFile(null);
        self.IconImageFile(null);
    };

    ko.bindingHandlers.fileUpload = {
        init: function (element, valueAccessor) {
            $(element).change(function () {
                valueAccessor()(element.files[0]);
            });
        },
        update: function (element, valueAccessor) {
            if (ko.unwrap(valueAccessor()) === null) {
                $(element).wrap('<form>').closest('form').get(0).reset();
                $(element).unwrap();
            }
        }
    }*/

};



ko.applyBindings(DealershipViewModel);
