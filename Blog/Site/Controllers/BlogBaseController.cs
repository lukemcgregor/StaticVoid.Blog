using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;

namespace StaticVoid.Blog.Site.Controllers
{
    public abstract class BlogBaseController : Controller
    {
        public BlogBaseController(IRepository<Data.Blog> blogRepo)
        {
            var blog = blogRepo.CurrentBlog();
            
            ViewBag.BlogName = blog.Name;
            ViewBag.BlogDescription = blog.Description;
            ViewBag.Analytics = blog.AnalyticsKey;
            ViewBag.Twitter = blog.Twitter;
            ViewBag.Disqus = blog.DisqusShortname;
            ViewBag.BlogStyleId = blog.StyleId;
        }
    }
}
