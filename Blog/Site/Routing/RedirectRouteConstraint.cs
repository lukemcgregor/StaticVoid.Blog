using StaticVoid.Blog.Data;
using StaticVoid.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StaticVoid.Blog.Site.Routing
{
    public class RedirectRouteConstraint: IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return DependencyResolver.Current.GetService<IRepository<Redirect>>().GetRedirectFor(httpContext.Request.Url.LocalPath.TrimStart('/')) != null;
        }
    }
}