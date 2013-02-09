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
        private readonly IRepository<PostModification> _postModRepo;

        public DashboardController(IRepository<Post> postRepo, IRepository<PostModification> postModRepo)
        {
            _postRepo = postRepo;
            _postModRepo = postModRepo;
        }

        public ActionResult Index()
        {
            var posts = _postRepo.GetAll().OrderByDescending(p => p.Posted).ToArray();

            return View(new DashboardModel{
                Posts = posts.Select(p => new Tuple<string, int>(p.GetDraftTitle(), p.Id)).ToList(),
                SelectedPost = GetDashboardPostModel(posts.First().Id)
            });
        }

        private DashboardPostModel GetDashboardPostModel(int id)
        {
            var post = _postRepo.GetBy(p => p.Id == id);
            return new DashboardPostModel
            {
                Id = id,
                Description = post.GetDraftDescription(),
                Title = post.GetDraftTitle(),
                Url = post.Path,
                HasDraftContent = post.HasDraftContent(),
                Status = post.Status.ToString(),
                PublishedDate = post.Status >= PostStatus.Published ? post.Posted : (DateTime?)null,
                LastModified = _postModRepo.PostLastModificationDate(post.Id) ?? post.Posted
            };
        }

        public JsonResult PostDetail(int id)
        {
            return Json(GetDashboardPostModel(id), JsonRequestBehavior.AllowGet);
        }

    }
}
