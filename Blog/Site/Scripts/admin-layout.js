function GlobalNavViewModel() {
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

var MenuItemViewModel = function (options) {
    var self = this;
    self.title = ko.observable(options.data.title);
    self.url = ko.observable(options.data.url);
    self.cssClass = ko.observable(options.data.cssClass);
    self.iconUrl = ko.observable('/images/icons/' + options.data.icon + '.png');
    self.selected = ko.observable(false);
};

var globalNav = new GlobalNavViewModel();
$(function () {
    ko.applyBindings(globalNav, $('#global-nav')[0]);
});