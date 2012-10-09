using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Site.Controllers
{
	public class PostController : Controller
	{
		private IRepository<Post> _postRepository;

		public PostController(IRepository<Post> postRepository)
		{
			_postRepository = postRepository;
		}

		public ActionResult Index()
		{
			var post = _postRepository.LatestPublishedPost();

			return Redirect("/" + post.Path);
		}

		public ActionResult Display(int year, int month, int day, string title)
		{
			var url =PostHelpers.MakeUrl(year, month, day, title);
			var post = _postRepository.GetBy(p => p.Path == url);

			var prevPost = _postRepository.GetPostBefore(post);
			var nextPost = _postRepository.GetPostAfter(post);

			var md = new MarkdownDeep.Markdown();

			var model = new PostModel
			{
				Body = md.Transform(post.Body),
				Title = post.Title,
				HasNextPost = nextPost!=null,
				HasPreviousPost = prevPost!= null,
				Posted = post.Posted,
				CanonicalUrl = post.Canonical
			};

			if(prevPost!= null)
			{
				model.PreviousPostDate = prevPost.Posted;
				model.PreviousPostLink =prevPost.Path;
				model.PreviousPostTitle = prevPost.Title;
			}

			if(nextPost!= null)
			{
				model.NextPostDate = nextPost.Posted;
				model.NextPostLink =nextPost.Path;
				model.NextPostTitle = nextPost.Title;
			}

			return View("Post", model);
		}

		public ActionResult Preview(int id)
		{
			var post = _postRepository.GetBy(p => p.Id == id);

			var md = new MarkdownDeep.Markdown();

			return View("Post", new PostModel
			{
				Body = md.Transform(post.DraftBody),
				Title = post.DraftTitle,
				Posted = DateTime.Now,
				CanonicalUrl = post.Canonical
			});
		}
	}
}
