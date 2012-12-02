using StaticVoid.Blog.Data;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StaticVoid.Blog.Site.Routing
{
    public class PostRouteConstraint: IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return DependencyResolver.Current.GetService<IRepository<Post>>().IsUrlAPost(httpContext.Request.Url.LocalPath.TrimStart('/'));
        }
    }
}