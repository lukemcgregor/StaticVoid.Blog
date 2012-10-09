using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Authors
{
	public class AuthorsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Authors";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Authors_default",
				"Authors/{controller}/{action}/{id}",
				new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
