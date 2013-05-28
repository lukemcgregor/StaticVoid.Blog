using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Platform
{
    public class PlatformAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Platform";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Platform_default",
                "Platform/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
