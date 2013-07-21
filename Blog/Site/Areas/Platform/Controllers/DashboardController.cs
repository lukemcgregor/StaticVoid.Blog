using StaticVoid.Blog.Site.Areas.Platform.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Platform.Controllers
{
    [PlatformAdminAuthorize]
    public class DashboardController : Controller
    {
        public readonly IRepository<Data.Blog> _blogRepo;

        public DashboardController(IRepository<Data.Blog> blogRepo)
        {
            _blogRepo = blogRepo;
            ViewBag.IsBlogAdmin = true;
        }

        public ActionResult Index()
        {
            var blogs = _blogRepo.GetAll().Select(b => new DashboardBlogModel
            {
                Id = b.Id,
                Name = b.Name,
                Url = b.AuthoritiveUrl
            }).ToList();

            return View(new PlatformDashboardModel
            {
                Blogs = blogs
            });
        }
    }
}
