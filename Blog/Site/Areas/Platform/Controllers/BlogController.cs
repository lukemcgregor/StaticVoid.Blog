using StaticVoid.Blog.Site.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Platform.Controllers
{
    [PlatformAdminAuthorize]
    public class BlogController : Controller
    {
        public ActionResult Create()
        {
            return PartialView("CreateModal");
        }

    }
}
