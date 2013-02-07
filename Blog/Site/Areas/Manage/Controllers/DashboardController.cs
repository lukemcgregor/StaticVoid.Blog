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
            var posts = _postRepo.GetAll().OrderByDescending(p => p.Posted).ToArray();

            return View(new DashboardModel{
                Posts = posts.Select(p => new Tuple<string, int>(p.GetDraftTitle(), p.Id)).ToList(),
                SelectedPost = new DashboardPostModel
                {
                    Id = posts.First().Id,
                    Description = posts.First().GetDraftDescription(),
                    Title = posts.First().GetDraftTitle(),
                    Url = posts.First().Path,
                    HasDraftContent = posts.First().HasDraftContent(),
                    Status = posts.First().Status.ToString()
                }
            });
        }

        public JsonResult PostDetail(int id)
        {
            var post = _postRepo.GetBy(p => p.Id == id);
            return Json(new DashboardPostModel
            {
                Id=id,
                Description = post.GetDraftDescription(),
                Title = post.GetDraftTitle(),
                Url = post.Path,
                HasDraftContent = post.HasDraftContent(),
                Status = post.Status.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
