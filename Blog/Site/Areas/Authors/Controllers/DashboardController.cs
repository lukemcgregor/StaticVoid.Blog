using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Security;

namespace StaticVoid.Blog.Site.Areas.Authors.Controllers
{
	[AuthorAuthorize]
    public class DashboardController : Controller
	{
        public ActionResult Index()
        {
            return View();
        }

    }
}
