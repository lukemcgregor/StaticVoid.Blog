using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StaticVoid.Blog.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Feed",
                url: "feed.{action}",
                defaults: new { controller = "Feed", action = "Atom" }
            );

            routes.MapRoute(
                name: "Preview",
                url: "Preview/{id}",
                defaults: new { controller = "Post", action = "Preview" }
            );

            routes.MapRoute(
                name: "404",
                url: "404",
                defaults: new { controller = "Error", action = "NotFound" }
            );

            routes.MapRoute(
                name: "403",
                url: "403",
                defaults: new { controller = "Error", action = "PermissionDenied" }
            );

            routes.MapRoute(
                name: "Generic Error",
                url: "Error",
                defaults: new { controller = "Error", action = "Error" }
            );

            routes.MapRoute(
                name: "Test Error",
                url: "Error/Test/{statusCode}",
                defaults: new { controller = "Error", action = "Test" }
            );

            var redirectConstraint = new RedirectRouteConstraint();
            routes.MapRoute(
                name: "Redirect",
                url: "{*Path}",
                defaults: new { controller = "Redirect", action = "ProcessRedirect" },
                constraints: new { redirectConstraint }
            );

            var postConstraint = new PostRouteConstraint();
            routes.MapRoute(
                name: "Post",
                url: "{*Path}",
                defaults: new { controller = "Post", action = "Display" },
                constraints: new { postConstraint }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute("Error", "{*url}",
                new { controller = "Error", action = "NotFound" }
            );
        }
    }
}