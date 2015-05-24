using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Controllers
{
	public abstract class BlogBaseController : Controller
	{
		private readonly IRepository<Data.Blog> _blogRepo;
		private readonly IHttpContextService _httpContext;

		public Data.Blog CurrentBlog
		{
			get
			{
				return _blogRepo.GetCurrentBlog(_httpContext);
			}
		}

		public BlogBaseController(IRepository<Data.Blog> blogRepo, IHttpContextService httpContext)
		{
			_blogRepo = blogRepo;
			_httpContext = httpContext;

			var blog = CurrentBlog;

			if (blog != null)
			{
				ViewBag.BlogName = blog.Name;
				ViewBag.BlogDescription = blog.Description;
				ViewBag.Analytics = blog.AnalyticsKey;
				ViewBag.Twitter = blog.Twitter;
				ViewBag.Disqus = blog.DisqusShortname;
				ViewBag.BlogTemplateId = blog.BlogTemplateId;
			}
		}
	}
}
