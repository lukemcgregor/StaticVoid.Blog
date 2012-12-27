using StaticVoid.Blog.Site.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ThrowNotFound()
        {
            throw new HttpException((int)HttpStatusCode.NotFound, "The requested URL was not found");
        }

        public ActionResult NotFound(string aspxerrorpath)
        {
            Response.TrySkipIisCustomErrors = true;
            ViewData["error_path"] = aspxerrorpath;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFound");
        }

        public ActionResult Error()
        {
            Response.TrySkipIisCustomErrors = true;
            return View("Error");
        }

        [AuthorAuthorize]
        public ActionResult Test(int statusCode)
        {
            throw new HttpException(statusCode,"");
        }
    }
}
