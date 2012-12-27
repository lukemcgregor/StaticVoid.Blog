using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Http404()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Error(int? statusCode)
        {
            if(statusCode.HasValue)
                Response.StatusCode = statusCode.Value;
            return View();
        }
    }
}
