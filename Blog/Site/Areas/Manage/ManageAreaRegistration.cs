using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage
{
	public class AuthorsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Manage";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Manage_default",
				"Manage/{controller}/{action}/{id}",
				new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
