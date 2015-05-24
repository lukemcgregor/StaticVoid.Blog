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
using System.Reflection;
using System.IO;

namespace StaticVoid.Blog.Site.Controllers
{
	public class PostController : BlogBaseController
	{
		private readonly IRepository<Post> _postRepository;
		private readonly IRepository<Data.Blog> _blogRepo;
		private readonly IRepository<BlogTemplate> _blogTemplateRepo;

		public PostController(
			IRepository<Post> postRepository,
			IRepository<Data.Blog> blogRepo,
			IRepository<BlogTemplate> blogTemplateRepo,
			IHttpContextService httpContext)
			: base(blogRepo, httpContext)
		{
			_postRepository = postRepository;
			_blogTemplateRepo = blogTemplateRepo;
			_blogRepo = blogRepo;
		}

		public ActionResult Index()
		{
			var post = _postRepository.LatestPublishedPost(CurrentBlog.Id);

			if (post == null)
				throw new HttpException((int)HttpStatusCode.NotFound, "No posts have been published");

			return Redirect("/" + post.Path);
		}

		private TemplatedPostModel TemplatedPostModel(string path)
		{
			var model = new TemplatedPostModel();

			if (CurrentBlog.BlogTemplateId.HasValue)
			{
				var template = _blogTemplateRepo.GetById(CurrentBlog.BlogTemplateId.Value);
				model.Template = template.HtmlTemplate;
			}
			return (TemplatedPostModel)PostModel(path, model);
		}

		private PostModel PostModel(string path, PostModel model = null)
		{
			if (model == null)
			{
				model = new PostModel();
			}

			var currentBlog = CurrentBlog;

			var post = _postRepository.GetPostAtUrl(currentBlog.Id, path, p => p.Author);

			var prevPost = _postRepository.GetPostBefore(post);
			var nextPost = _postRepository.GetPostAfter(post);

			var md = new MarkdownDeep.Markdown();

			model.Body = md.Transform(post.Body);
			model.Description = post.Description;
			model.Title = post.Title;
			model.Posted = post.Posted;
			model.CanonicalUrl = post.Canonical;
			model.Author = new PostAuthor
			{
				GravatarUrl = post.Author.Email.GravitarUrlFromEmail(),
				Name = post.Author.FullName(),
				GooglePlusProfileUrl = post.Author.GooglePlusProfileUrl
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

			if (prevPost != null)
			{
				model.PreviousPost = new PartialPostForLinkModel
				{
					Date = prevPost.Posted,
					Link = prevPost.Path,
					Title = prevPost.Title
				};
			}

			if (nextPost != null)
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
			if (CurrentBlog.BlogTemplateId.HasValue)
			{
				var template = _blogTemplateRepo.GetById(CurrentBlog.BlogTemplateId.Value);

				switch (template.TemplateMode)
				{
					case TemplateMode.NoDomCustomisation:
						return View("PostNoDomCustomisation", PostModel(path));

					case TemplateMode.DomRip:
						return View("PostDomRip", TemplatedPostModel(path));

					case TemplateMode.BodyOnly:
						return View("PostBodyOnly", TemplatedPostModel(path));
				}
			}
			//default
			return View("PostNoDomCustomisation", PostModel(path));
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
				Description = post.DraftDescription,
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
