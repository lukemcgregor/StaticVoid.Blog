define(["jquery", "knockout"], function ($, ko) {
    var GlobalNavViewModel = function () {
        var self = this;

        self.menuItems = ko.observableArray([]);

        self.select = function (url) {
            $.each(self.menuItems(), function (index, value) {
                if (url === value.url()) {
                    value.selected(true);
                }
                else {
                    value.selected(false);
                }
            });
        };
    }
    var vm = new GlobalNavViewModel();

    return vm;
});