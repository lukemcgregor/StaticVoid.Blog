using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Repository;
using StaticVoid.Blog.Data;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
	[AuthorAuthorize]
    public class DashboardController : Controller
	{
        private readonly IRepository<Post> _postRepo;

        public DashboardController(IRepository<Post> postRepo)
        {
            _postRepo = postRepo;
        }

        public ActionResult Index()
        {
            return View(new DashboardModel{
                Posts = _postRepo.GetAll().Select(p=> new Tuple<string,int>(p.Title, p.Id)).ToList()
            });
        }

        public JsonResult PostDetail(int id)
        {
            var post = _postRepo.GetBy(p => p.Id == id);
            return Json(new DashboardPostModel
            {
                Id=id,
                Description = post.Description,
                Title =post.Title,
                Url = post.Path
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
