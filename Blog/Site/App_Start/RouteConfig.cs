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
				url: "feed/{action}",
				defaults: new { controller = "Feed", action = "Atom" }
			);

			routes.MapRoute(
				name: "Post",
				url: "{year}/{month}/{day}/{title}",
				defaults: new { controller = "Post", action = "Display" }
			);

			routes.MapRoute(
				name: "Preview",
				url: "Preview/{id}",
				defaults: new { controller = "Post", action = "Preview" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}