using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
            filters.Add(new ElmahHandleErrorAttribute());
		}
	}
}