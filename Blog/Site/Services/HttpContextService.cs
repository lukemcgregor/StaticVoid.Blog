using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Services
{
    public interface IHttpContextService
    {
        Uri RequestUrl { get; }
    }

    public class HttpContextService : IHttpContextService
    {
        public Uri RequestUrl
        {
            get { return System.Web.HttpContext.Current.Request.Url; }
        }
    }
}