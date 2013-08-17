define(["knockout"], function (ko) {
    return function (options) {
        var self = this;
        self.title = ko.observable(options.data.title);
        self.url = ko.observable(options.data.url);
        self.cssClass = ko.observable(options.data.cssClass);
        self.iconUrl = ko.observable('/images/icons/' + options.data.icon + '.png');
        self.selected = ko.observable(false);
    };
});