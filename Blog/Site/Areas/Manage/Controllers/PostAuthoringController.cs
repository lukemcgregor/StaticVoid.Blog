using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Repository;
using System.Net;
using StaticVoid.Mockable;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
	[AuthorAuthorize]
	public class PostAuthoringController : Controller
	{
		private readonly IRepository<Post> _postRepository;
        private readonly IRepository<PostModification> _postModificationRepository;
		private readonly IRepository<User> _userRepository;
        private readonly IRepository<Redirect> _redirectRepository;
        private readonly IRepository<Data.Blog> _blogRepository;
        private readonly ISecurityHelper _securityHelper;
        private readonly IProvideDateTime _dateTime;

        public PostAuthoringController(
            IRepository<Post> postRepository, 
            IRepository<PostModification> postModificationRepository,
            IRepository<User> userRepository, 
            IRepository<Redirect> redirectRepository,
            IRepository<Data.Blog> blogRepository,
            ISecurityHelper securityHelper,
            IProvideDateTime dateTime)
		{
			_postRepository = postRepository;
            _postModificationRepository = postModificationRepository;
			_userRepository = userRepository;
            _redirectRepository = redirectRepository;
            _blogRepository = blogRepository;
            _securityHelper = securityHelper;
            _dateTime = dateTime;
		}

		public ActionResult Index()
		{
			return View(_postRepository.GetAll().AsEnumerable().OrderByDescending(p=>p.Posted).Select(p => new PostModel
					{
						Id = p.Id,
						Title = p.GetDraftTitle(),
						Status = p.Status,
                        Posted = p.Posted,
						HasDraftContent= p.HasDraftContent()
					}));
		}

		public ActionResult Create()
		{
			ViewBag.Title = "Create";
            return View("Edit", new PostEditModel { BlogStyleId = _blogRepository.CurrentBlog().StyleId });
		}

		[HttpPost,ValidateInput(false)]
		public ActionResult Create(PostEditModel model)
		{
			ViewBag.Title = "Create";
			if (ModelState.IsValid)
			{
                var url = PostHelpers.MakeUrl(_dateTime.Today.Year, _dateTime.Today.Month, _dateTime.Today.Day, model.Title);

				_postRepository.Create(new Post
				{
					AuthorId = _userRepository.GetCurrentUser(_securityHelper).Id,
					DraftBody = model.Body,
                    DraftDescription = model.Description,
					Posted = _dateTime.Now,
					DraftTitle = model.Title,
					Status = PostStatus.Draft,
					Path = url,
					Canonical = model.Reposted ? model.CanonicalUrl : "/" + url,
                    ExcludeFromFeed = false,
                    PostGuid = Guid.NewGuid()
				});

				return RedirectToAction("Index", "Dashboard");
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
                Description = post.GetDraftDescription(),
				CanonicalUrl = post.Canonical,
				Reposted = !String.IsNullOrWhiteSpace(post.Canonical) && post.Canonical != "/"+post.Path,
                BlogStyleId = _blogRepository.CurrentBlog().StyleId
			});
		}

		[HttpPost,ValidateInput(false)]
		public ActionResult Edit(int id, PostEditModel model)
		{
			ViewBag.Title = "Edit";
			if (ModelState.IsValid)
			{
				var post = _postRepository.GetBy(p => p.Id == id);

                var pm = PostModification.GetUnmodifiedPostModification();
                pm.PostId = id;

                if (model.Body != post.Body && model.Body != post.DraftBody)
                {
                    pm.BodyModified = true;
                    pm.NewBody = model.Body;
                }

                if (model.Description != post.Description && model.Description != post.DraftDescription)
                {
                    pm.DescriptionModified = true;
                    pm.NewDescription = model.Description;
                }

                if (model.CanonicalUrl != post.Canonical)
                {
                    pm.CannonicalModified = true;
                    pm.NewCannonical = model.CanonicalUrl;
                }

                if (model.Title != post.Title && model.Title != post.DraftTitle)
                {
                    pm.TitleModified = true;
                    pm.NewTitle = model.Title;
                }

                if (pm.HasModifications())
                {
                    _postModificationRepository.Create(pm);
                    post.DraftTitle = model.Title;
                    post.DraftDescription = model.Description;
                    post.DraftBody = model.Body;
				    post.Canonical = model.Reposted ? model.CanonicalUrl : "/" + post.Path;
				    _postRepository.Update(post);
                }

                return RedirectToAction("Index", "Dashboard");
			}
			return View(model);
		}

        public ActionResult ConfirmPublish(int id)
        {
            var post = _postRepository.GetBy(p => p.Id == id);

            if (post == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "The specified post was not found");
            }

            return PartialView("ConfirmPublishModal", new ConfirmPublishModel
            {
                Id = post.Id,
                Title = post.DraftTitle
            });
        }

        [HttpPost]
        public ActionResult ConfirmPublish(int id, ConfirmPublishModel model)
        {
            var post = _postRepository.GetBy(p => p.Id == id);

            if (post == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "The specified post was not found");
            }
            if (ModelState.IsValid)
            {
                //We want to update the published date if we are publishing for the first time
                if (post.Status == PostStatus.Draft)
                {
                    post.Posted = _dateTime.Now;
                }
                post.Status = PostStatus.Published;
                post.Title = post.DraftTitle;
                post.Description = post.DraftDescription;
                post.Body = post.DraftBody;
                post.DraftBody = null;
                post.DraftDescription = null;
                post.DraftTitle = null;

                var pm = PostModification.GetUnmodifiedPostModification(_dateTime.Now);
                pm.PostId = id;
                pm.StatusModified = true;
                pm.NewStatus = PostStatus.Published;

                _postModificationRepository.Create(pm);

                _postRepository.Update(post);

                return Json(new { success = true });
            }

            return PartialView("ConfirmPublishModal", model);
        }

        public ActionResult ConfirmUnPublish(int id)
        {
            var post = _postRepository.GetBy(p => p.Id == id);

            if (post == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "The specified post was not found");
            }

            return PartialView("ConfirmUnPublishModal", new ConfirmPublishModel
            {
                Id = post.Id,
                Title = post.DraftTitle
            });
        }

        [HttpPost]
        public ActionResult ConfirmUnPublish(int id, ConfirmPublishModel model)
        {
            var post = _postRepository.GetBy(p => p.Id == id);

            if (post == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "The specified post was not found");
            }
            if (ModelState.IsValid)
            {
                post.DraftTitle = post.Title;
                post.DraftDescription = post.Description;
                post.DraftBody = post.Body;
                post.Status = PostStatus.Unpublished;

                var pm = PostModification.GetUnmodifiedPostModification();
                pm.PostId = id;
                pm.StatusModified = true;
                pm.NewStatus = PostStatus.Unpublished;

                _postModificationRepository.Create(pm);

                _postRepository.Update(post);

                return Json(new { success = true });
            }

            return PartialView("ConfirmUnPublishModal", model);
        }

        public ActionResult EditPostUrl(int id)
        {
            var post = _postRepository.GetBy(p => p.Id == id);
            
            if (post == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "The specified post was not found");
            }

            return PartialView("EditPostUrlModal", new PostUrlEditModel
            {
                Id = post.Id,
                Title = post.Title,
                Url = post.Path
            });
        }

        [HttpPost]
        public ActionResult EditPostUrl(int id, PostUrlEditModel model)
        {
            var post = _postRepository.GetBy(p => p.Id == id);

            if (post == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "The specified post was not found");
            }
            if (ModelState.IsValid)
            {                
                _redirectRepository.Create(new Redirect
                {
                    IsPermanent = false,//TODO when im more confident in this functionality make this permanant
                    NewRoute= model.Url,
                    OldRoute = post.Path
                });

                //Change the cannonical URL if the current URL was set to canonical
                if (post.Path.TrimStart('/') == post.Canonical.TrimStart('/'))
                {
                    post.Canonical = "/" + model.Url.TrimStart('/');
                }
                post.Path = model.Url;

                _postRepository.Update(post);

                return Json(new { success = true });
            }

            return PartialView("EditPostUrlModal", model);
        }

        public ActionResult RebuildAllUrls()
        {
            var posts = _postRepository.GetAll().ToList();
            foreach (var post in posts)
            {
                post.Path = PostHelpers.MakeUrl(post.Posted.Year, post.Posted.Month, post.Posted.Day, post.Title);
                _postRepository.Update(post);
            }
            return RedirectToAction("Index", "Dashboard");
        }
	}
}
