using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult From(string path)
        {
            return View();
        }

    }
}
