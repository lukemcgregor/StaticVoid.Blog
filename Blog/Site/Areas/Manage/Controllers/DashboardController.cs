using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Repository;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
	[CurrentBlogAuthorAuthorize]
    public class DashboardController : ManageBaseController
	{
        private readonly IRepository<Post> _postRepo;
        private readonly IRepository<PostModification> _postModRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Securable> _securableRepo;
        private readonly IRepository<Data.Blog> _blogRepo;
        private readonly ISecurityHelper _securityHelper;

        public DashboardController(
            IRepository<Post> postRepo,
            IRepository<PostModification> postModRepo,
            IRepository<User> userRepo,
            IRepository<Securable> securableRepo,
            IRepository<Data.Blog> blogRepo,
            ISecurityHelper securityHelper,
            IHttpContextService httpContext)
            : base(blogRepo, httpContext, securityHelper, userRepo, securableRepo)
        {
            _postRepo = postRepo;
            _postModRepo = postModRepo;
            _userRepo = userRepo;
            _blogRepo = blogRepo;
            _securableRepo = securableRepo;
            _securityHelper = securityHelper;
        }

        public ActionResult Index()
        {
            var currentUser = _userRepo.GetCurrentUser(_securityHelper);
            var currentBlog = CurrentBlog;

            var posts = _postRepo.PostsForBlog(currentBlog.Id).OrderByDescending(p => p.Posted).ToArray();

            return View(new DashboardModel{
                Posts = posts.Select(p => new { Title = p.GetDraftTitle(), PostId = p.Id}).Cast<object>().ToList(),
                SelectedPost = posts.Any() ? GetDashboardPostModel( posts.First().Id): null,
                IsAdmin = currentUser.IsAdminOfBlog(currentBlog, _securableRepo)
            });
        }

        private DashboardPostModel GetDashboardPostModel(int id)
        {
            var post = _postRepo.PostsForBlog(CurrentBlog.Id).Single(p => p.Id == id);
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
