using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Repository;
using StaticVoid.Blog.Site.Gravitar;
using System.Net;

namespace StaticVoid.Blog.Site.Controllers
{
	public class PostController : BlogBaseController
	{
		private IRepository<Post> _postRepository;
        private readonly IRepository<Data.Blog> _blogRepo;
		private readonly VisitLoggerService _visitLogger;

        public PostController(IRepository<Post> postRepository, VisitLoggerService visitLogger, IRepository<Data.Blog> blogRepo)
            : base(blogRepo)
		{
			_postRepository = postRepository;
			_visitLogger = visitLogger;
		}

		public ActionResult Index()
		{
			var post = _postRepository.LatestPublishedPost();

            if(post== null)
                throw new HttpException((int)HttpStatusCode.NotFound, "No posts have been published");

			return Redirect("/" + post.Path);
		}

		public ActionResult Display(string path)
		{
            _visitLogger.LogCurrentRequest();

			var post = _postRepository.GetPostAtUrl(path, p=>p.Author);

			var prevPost = _postRepository.GetPostBefore(post);
			var nextPost = _postRepository.GetPostAfter(post);

			var md = new MarkdownDeep.Markdown();

            var model = new PostModel
            {
                Body = md.Transform(post.Body),
                Description = post.Description,
                Title = post.Title,
                Posted = post.Posted,
                CanonicalUrl = post.Canonical,
                Author = new PostAuthor
                {
                    GravatarUrl = post.Author.Email.GravitarUrlFromEmail(),
                    Name = String.Format("{0} {1}", post.Author.FirstName, post.Author.LastName),
                    GooglePlusProfileUrl = post.Author.GooglePlusProfileUrl
                }
            };

            model.OtherPosts = new List<PartialPostForLinkModel>();
            model.OtherPosts.AddRange(_postRepository.PublishedPosts()
                .OrderBy(p => p.Posted)
                .Where(p => p.Posted > post.Posted)
                .Take(5)
                .OrderByDescending(p => p.Posted)
                .Select(p => new PartialPostForLinkModel { Title = p.Title, IsCurrentPost = false, Link = p.Path }));
            model.OtherPosts.Add(new PartialPostForLinkModel { Link = post.Path, IsCurrentPost = true, Title = post.Title });
            model.OtherPosts.AddRange(_postRepository.PublishedPosts()
                .OrderByDescending(p => p.Posted)
                .Where(p => p.Posted < post.Posted)
                .Take(5)
                .Select(p => new PartialPostForLinkModel { Title = p.Title, IsCurrentPost = false, Link = p.Path }));

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
            var post = _postRepository.GetBy(p => p.Id == id, p => p.Author);

			var md = new MarkdownDeep.Markdown();

            return View("Post", new PostModel
            {
                Body = md.Transform(post.DraftBody),
                Description=post.DraftDescription,
                Title = post.DraftTitle,
                Posted = DateTime.Now,
                CanonicalUrl = post.Canonical,
                OtherPosts = new List<PartialPostForLinkModel>(),
                Author = new PostAuthor
                {
                    GravatarUrl = post.Author.Email.GravitarUrlFromEmail(),
                    Name = String.Format("{0} {1}", post.Author.FirstName, post.Author.LastName),
                    GooglePlusProfileUrl = post.Author.GooglePlusProfileUrl
                }
            });
		}
	}
}
