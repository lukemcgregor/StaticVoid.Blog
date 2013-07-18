function GlobalNavViewModel() {
    var self = {};

    var MenuItemViewModel = function (options) {
        var self = this;
        self.title = ko.observable(options.data.title);
        self.url = ko.observable(options.data.url);
        self.cssClass = ko.observable(options.data.cssClass);
        self.iconUrl = ko.observable('/images/icons/' + options.data.icon + '.png');
        self.selected = ko.observable(false);

        return self;
    };

    self.menuItems = ko.observableArray([
        new MenuItemViewModel({ data: { title: "Dashboard",     url: "/manage",                                     icon: "dashboard"       , cssClass: '' } }),
        new MenuItemViewModel({ data: { title: "Author",        url: "/manage/postauthoring/create",                icon: "format-quote"    , cssClass: '' } }),
        new MenuItemViewModel({ data: { title: "Redirects",     url: "/manage/redirects",                           icon: "shuffle"         , cssClass: '' } }),
        new MenuItemViewModel({ data: { title: "Style",         url: "/manage/templateeditor",                      icon: "quill"           , cssClass: '' } }),
        new MenuItemViewModel({ data: { title: "Permissions",   url: "/manage/permissionseditor/editpermissions",   icon: "key"             , cssClass: 'permissions-editor' } }),
        new MenuItemViewModel({ data: { title: "Recovery",      url: "/manage/recovery",                            icon: "drive-upload"    , cssClass: 'blog-recovery' } }),
        new MenuItemViewModel({ data: { title: "Config",        url: "/manage/blogconfiguration/edit",              icon: "gears"           , cssClass: 'edit-blog-config' } }),
    ]);

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
    return self;
}

var globalNav = new GlobalNavViewModel();
$(function () {
    ko.applyBindings(globalNav, $('#global-nav')[0]);
});