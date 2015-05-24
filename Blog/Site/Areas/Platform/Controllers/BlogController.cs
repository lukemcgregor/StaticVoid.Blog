using StaticVoid.Blog.Data;
using StaticVoid.Blog.Email;
using StaticVoid.Blog.Site.Areas.Platform.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Platform.Controllers
{
	[PlatformAdminAuthorize]
	public class BlogController : Controller
	{
		private readonly IRepository<Data.Blog> _blogRepo;
		private readonly IRepository<Data.Securable> _securableRepo;
		private readonly IInvitationService _invitationService;

		public BlogController(
			IRepository<Data.Blog> blogRepo,
			IRepository<Data.Securable> securableRepo,
			IInvitationService invitationService)
		{
			_blogRepo = blogRepo;
			_securableRepo = securableRepo;
			_invitationService = invitationService;
			ViewBag.IsBlogAdmin = true;
		}

		public ActionResult Create()
		{
			return PartialView("CreateModal");
		}

		[HttpPost]
		public ActionResult Create(CreateBlogModel model)
		{
			if (ModelState.IsValid)
			{
				var adminSecurable = new Data.Securable { Name = "Admin: " + model.Name };
				_securableRepo.Create(adminSecurable);
				var authorSecurable = new Data.Securable { Name = "Author: " + model.Name };
				_securableRepo.Create(authorSecurable);

				_blogRepo.Create(new Data.Blog
				{
					AdminSecurableId = adminSecurable.Id,
					AuthorSecurableId = authorSecurable.Id,
					Name = model.Name,
					AuthoritiveUrl = model.Url,
					BlogGuid = Guid.NewGuid()
				});

				_invitationService.Invite(adminSecurable.Id, model.AdminEmail, "StaticVoid.Blog");

				return Json(new { success = true });
			}
			return PartialView("CreateModal", model);
		}
	}
}
