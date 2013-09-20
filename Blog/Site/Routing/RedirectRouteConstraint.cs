using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StaticVoid.Blog.Site.Routing
{
	public class RedirectRouteConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			var blogRepo = DependencyResolver.Current.GetService<IRepository<Data.Blog>>();
			var httpContextService = DependencyResolver.Current.GetService<IHttpContextService>();

			return DependencyResolver.Current.GetService<IRepository<Redirect>>().GetRedirectFor(httpContext.Request.Url.LocalPath.TrimStart('/'), blogRepo.GetCurrentBlog(httpContextService).Id) != null;
		}
	}
}