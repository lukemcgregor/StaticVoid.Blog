using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace StaticVoid.Blog.Site
{
    /// <summary>
    /// Redirect Route Handler
    /// </summary>
    public class RedirectRouteHandler : IRouteHandler
    {
        private string _newUrl;
        private bool _permanent;

        public RedirectRouteHandler(string newUrl, bool permanent = false)
        {
            _newUrl = newUrl;
            _permanent = permanent;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new RedirectHandler(_newUrl,_permanent);
        }
    }

    /// <summary>
    /// <para>Redirecting MVC handler</para>
    /// </summary>
    public class RedirectHandler : IHttpHandler
    {
        private string _newUrl;
        private bool _permanent;

        public RedirectHandler(string newUrl, bool permanent)
        {
            _newUrl = newUrl;
            _permanent = permanent;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            if (_permanent)
            {
                httpContext.Response.Status = "301 Moved Permanently";
                httpContext.Response.StatusCode = 301;
            }
            else
            {
                httpContext.Response.Status = "302 Moved Temporarily";
                httpContext.Response.StatusCode = 302;
            }
            httpContext.Response.AppendHeader("Location", _newUrl);
            return;
        }
    }
}