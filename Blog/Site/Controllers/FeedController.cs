using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Site.Controllers
{
    public class FeedController : Controller
    {
		private IRepository<Post> _postRepository;

		public FeedController(IRepository<Post> postRepository)
		{
			_postRepository = postRepository;
		}

		private SyndicationFeed GenerateFeed()
		{
			var postItems = _postRepository.PublishedPosts().OrderByDescending(p=>p.Posted).Take(25).AsEnumerable()
				.Select(p => new SyndicationItem(p.Title, p.Body, new Uri("http://blog.staticvoid.co.nz/" + p.Path)));

			return new SyndicationFeed("", "", new Uri("http://blog.staticvoid.co.nz"), postItems)
			{
				Language = "en-US"
			};
		}

		public ActionResult Atom()
		{
			return new FeedResult(new Atom10FeedFormatter(GenerateFeed()));
		}
    }
}
