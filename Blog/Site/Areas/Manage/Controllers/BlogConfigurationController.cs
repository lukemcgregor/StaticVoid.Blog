using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [AuthorAuthorize]
    public class BlogConfigurationController : Controller
    {
        private readonly IRepository<Data.Blog> _blogRepo;

        public BlogConfigurationController(IRepository<Data.Blog> blogRepo)
        {
            _blogRepo = blogRepo;
        }

        public ActionResult Edit()
        {
            var blog = _blogRepo.CurrentBlog();
            return PartialView("EditModal", new BlogConfigModel
                {
                    AnalyticsKey = blog.AnalyticsKey,
                    AuthoritiveUrl = blog.AuthoritiveUrl,
                    Description = blog.Description,
                    DisqusShortname = blog.DisqusShortname,
                    Name = blog.Name,
                    Twitter = blog.Twitter,
                    BlogStyleId = blog.StyleId
                });
        }

        [HttpPost]
        public ActionResult Edit(BlogConfigModel model)
        {
            if (ModelState.IsValid)
            {
                var blog = _blogRepo.CurrentBlog();
                blog.AnalyticsKey = model.AnalyticsKey;
                blog.AuthoritiveUrl = model.AuthoritiveUrl;
                blog.Description = model.Description;
                blog.DisqusShortname = model.DisqusShortname;
                blog.Name = model.Name;
                blog.Twitter = model.Twitter;

                _blogRepo.Update(blog);

                return Json(new { success = true });
            }

            return PartialView("EditModal", model);
        }

    }
}
