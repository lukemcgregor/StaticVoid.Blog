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
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Controllers
{
	public class PostController : BlogBaseController
	{
		private readonly IRepository<Post> _postRepository;
		private readonly IVisitLoggerService _visitLogger;
        private readonly IRepository<Data.Blog> _blogRepo;

        public PostController(
            IRepository<Post> postRepository, 
            IVisitLoggerService visitLogger, 
            IRepository<Data.Blog> blogRepo,
            IHttpContextService httpContext)
            : base(blogRepo, httpContext)
		{
			_postRepository = postRepository;
			_visitLogger = visitLogger;
            _blogRepo = blogRepo;
		}

		public ActionResult Index()
		{
			var post = _postRepository.LatestPublishedPost(CurrentBlog.Id);

            if(post== null)
                throw new HttpException((int)HttpStatusCode.NotFound, "No posts have been published");

			return Redirect("/" + post.Path);
		}

        //public ActionResult Script(string path)
        //{
        //    Response.ContentType = "text/javascript";
        //    return View(PostModel(path.Substring(0,path.Length -3)));
        //}

        private PostModel PostModel(string path)
        {
             var currentBlog = CurrentBlog;
            _visitLogger.LogCurrentRequest();

            var post = _postRepository.GetPostAtUrl(currentBlog.Id, path, p => p.Author);

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
                    Name = post.Author.FullName(),
                    GooglePlusProfileUrl = post.Author.GooglePlusProfileUrl
                }
            };

            model.OtherPosts = new List<PartialPostForLinkModel>();
            model.OtherPosts.AddRange(_postRepository.PublishedPosts(currentBlog.Id)
                .OrderBy(p => p.Posted)
                .Where(p => p.Posted > post.Posted)
                .Take(5)
                .OrderByDescending(p => p.Posted)
                .Select(p => new PartialPostForLinkModel { Title = p.Title, IsCurrentPost = false, Link = p.Path }));
            model.OtherPosts.Add(new PartialPostForLinkModel { Link = post.Path, IsCurrentPost = true, Title = post.Title });
            model.OtherPosts.AddRange(_postRepository.PublishedPosts(currentBlog.Id)
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
            return model;
        }


		public ActionResult Display(string path)
		{
			return View("Post", PostModel(path));
		}

		public ActionResult Preview(int id)
        {
            var currentBlog = CurrentBlog;
            int blogId = currentBlog.Id;

            var post = _postRepository.PostsForBlog(currentBlog.Id, p => p.Author).FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

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
                    Name = post.Author.FullName(),
                    GooglePlusProfileUrl = post.Author.GooglePlusProfileUrl
                }
            });
		}
	}
}
