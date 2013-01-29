using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Areas.Manage.Models;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
	[AuthorAuthorize]
    public class DashboardController : Controller
	{
        public ActionResult Index()
        {
            return View(new DashboardModel{
                Posts = new List<Tuple<string,int>>
                { 
                    new Tuple<string,int>("aaaa",1),
                    new Tuple<string,int>("bbbb",2)
                }
            });
        }

        public JsonResult PostDetail(int id)
        {
            return Json(new DashboardPostModel
            {
                Id=id,
                Description ="blurb " + id,
                Title ="aaaa" + id,
                Url = "fsdfsdfds/"+id
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
