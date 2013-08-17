require(
    [
        'jquery',
        'knockout',
        'admin/global-nav',
        'root-layout-startup',
        'layout-startup',
        'startup',
        'knockout-mapping',
        'bootstrap'
    ],
    function ($, ko, nav, rootLayoutStartup, layoutStartup, startup) {
        $(function () {
            console.debug('admin.js start');
            rootLayoutStartup.onStart();
            layoutStartup.onStart();
            startup.onStart();
            ko.applyBindings(nav, $('#global-nav')[0]);
            if (rootLayoutStartup.displayPage)
                rootLayoutStartup.displayPage();
        });
    });