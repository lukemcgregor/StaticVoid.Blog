using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Core.Repository;
using StaticVoid.Blog.Site.Gravitar;

namespace StaticVoid.Blog.Site.Controllers
{
	public class PostController : Controller
	{
		private IRepository<Post> _postRepository;
		private readonly VisitLoggerService _visitLogger;

		public PostController(IRepository<Post> postRepository, VisitLoggerService visitLogger)
		{
			_postRepository = postRepository;
			_visitLogger = visitLogger;
		}

		public ActionResult Index()
		{
			var post = _postRepository.LatestPublishedPost();

			return Redirect("/" + post.Path);
		}

		public ActionResult Display(int year, int month, int day, string title)
		{
			_visitLogger.LogCurrentRequest();

			var url =PostHelpers.MakeUrl(year, month, day, title);
			var post = _postRepository.GetBy(p => p.Path == url, p=>p.Author);

			var prevPost = _postRepository.GetPostBefore(post);
			var nextPost = _postRepository.GetPostAfter(post);

			var md = new MarkdownDeep.Markdown();

            var model = new PostModel
            {
                Body = md.Transform(post.Body),
                Title = post.Title,
                Posted = post.Posted,
                CanonicalUrl = post.Canonical,
                Author = new PostAuthor
                {
                    GravatarUrl = post.Author.Email.GravitarUrlFromEmail(),
                    Name = String.Format("{0} {1}", post.Author.FirstName,post.Author.LastName),
                    GooglePlusProfileUrl = post.Author.GooglePlusProfileUrl
                }
            };

			if(prevPost!= null)
			{
                model.PreviousPost = new PartialPostForLinkModel
                {
                    Date = prevPost.Posted,
				    Link = prevPost.Path,
				    Title = prevPost.Title
                };
			}

			if(nextPost!= null)
			{
                model.NextPost = new PartialPostForLinkModel
                {
                    Date = nextPost.Posted,
                    Link = nextPost.Path,
                    Title = nextPost.Title
                };
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
