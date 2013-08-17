define(["admin/global-nav", 'admin/global-nav-menu-item', 'admin-permissions'], function (nav, menuItemViewModel, adminPermissions) {
    return {
        setup: function () {
            console.debug('global-nav.setup');

            var menu = nav.menuItems;
            menu.push(new menuItemViewModel({ data: { title: "Dashboard", url: "/manage", icon: "dashboard", cssClass: '' } }));
            menu.push(new menuItemViewModel({ data: { title: "Author", url: "/manage/postauthoring/create", icon: "format-quote", cssClass: '' } }));
            menu.push(new menuItemViewModel({ data: { title: "Redirects", url: "/manage/redirects", icon: "shuffle", cssClass: '' } }));
            menu.push(new menuItemViewModel({ data: { title: "Style", url: "/manage/templateeditor", icon: "quill", cssClass: '' } }));
            if (adminPermissions.isBlogAdmin) {
                menu.push(new menuItemViewModel({ data: { title: "Permissions", url: "/manage/permissionseditor/editpermissions", icon: "key", cssClass: 'permissions-editor' } }));
            }
            menu.push(new menuItemViewModel({ data: { title: "Recovery", url: "/manage/recovery", icon: "drive-upload", cssClass: 'blog-recovery' } }));
            menu.push(new menuItemViewModel({ data: { title: "Config", url: "/manage/blogconfiguration/edit", icon: "gears", cssClass: 'edit-blog-config' } }));
        }
    };
});