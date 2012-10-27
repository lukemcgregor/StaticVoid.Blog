using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Areas.Authors.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Site.Areas.Authors.Controllers
{
	[AuthorAuthorize]
	public class PostAuthoringController : Controller
	{
		private readonly IRepository<Post> _postRepository;
		private readonly IRepository<User> _userRepository;

		public PostAuthoringController(IRepository<Post> postRepository, IRepository<User> userRepository)
		{
			_postRepository = postRepository;
			_userRepository = userRepository;
		}

		public ActionResult Index()
		{
			return View(_postRepository.GetAll().AsEnumerable().Select(p => new PostModel
					{
						Id = p.Id,
						Title = p.GetDraftTitle(),
						Status = p.Status,
						HasDraftContent= p.HasDraftContent()
					}));
		}

		public ActionResult Create()
		{
			ViewBag.Title = "Create";
			return View("Edit");
		}

		[HttpPost,ValidateInput(false)]
		public ActionResult Create(PostEditModel model)
		{
			ViewBag.Title = "Create";
			if (ModelState.IsValid)
			{
				var url = PostHelpers.MakeUrl(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,model.Title);

				_postRepository.Create(new Post
				{
					Author = _userRepository.GetCurrentUser(),
					DraftBody = model.Body,
					Posted = DateTime.Now,
					DraftTitle = model.Title,
					Status = PostStatus.Draft,
					Path = url,
					Canonical = model.Reposted ? model.CanonicalUrl : "/" + url
				});

				return RedirectToAction("Index");
			}
			return View("Edit", model);

		}

		public ActionResult Edit(int id)
		{
			ViewBag.Title = "Edit";
			var post = _postRepository.GetBy(p => p.Id == id);

			return View(new PostEditModel
			{
				Body = post.GetDraftBody(),
				Title = post.GetDraftTitle(),
				CanonicalUrl = post.Canonical,
				Reposted = !String.IsNullOrWhiteSpace(post.Canonical) && post.Canonical != "/"+post.Path
			});
		}

		[HttpPost,ValidateInput(false)]
		public ActionResult Edit(int id, PostEditModel model)
		{
			ViewBag.Title = "Edit";
			if (ModelState.IsValid)
			{
				var post = _postRepository.GetBy(p => p.Id == id);

				post.DraftTitle = model.Title;
				post.DraftBody = model.Body;
				post.Canonical = model.Reposted ? model.CanonicalUrl : "/" + post.Path;

				_postRepository.Update(post);
				return RedirectToAction("Index");
			}
			return View(model);
		}

		public ActionResult Publish(int id)
		{
			var post = _postRepository.GetBy(p => p.Id == id);

			post.Status = PostStatus.Published;
			post.Title = post.DraftTitle;
			post.Body = post.DraftBody;
			post.DraftBody = null;
			post.DraftTitle = null;

			_postRepository.Update(post);

			return RedirectToAction("Index");
		}

		public ActionResult UnPublish(int id)
		{
			var post = _postRepository.GetBy(p => p.Id == id);

			post.DraftTitle = post.Title;
			post.DraftBody = post.Body;
			post.Status = PostStatus.Unpublished;

			_postRepository.Update(post);

			return RedirectToAction("Index");
		}
	}
}
