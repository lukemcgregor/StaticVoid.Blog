using StaticVoid.Blog.Site.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [AuthorAuthorize]
    public class BlogConfigurationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

    }
}
