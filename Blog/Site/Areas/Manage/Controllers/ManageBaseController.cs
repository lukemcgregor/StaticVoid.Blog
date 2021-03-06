﻿using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
	[CurrentBlogAuthorAuthorize]
	public class ManageBaseController : BlogBaseController
	{
		public ManageBaseController(
			IRepository<Data.Blog> blogRepo,
			IHttpContextService httpContext,
			ISecurityHelper securityHelper,
			IRepository<User> userRepository,
			IRepository<Securable> securableRepository)
			: base(blogRepo, httpContext)
		{
			var currentUser = securityHelper.CurrentUser;
			if (currentUser != null && currentUser.IsAdminOfBlog(CurrentBlog, securableRepository))
			{
				ViewBag.IsBlogAdmin = true;
			}
			else
			{
				ViewBag.IsBlogAdmin = false;
			}
		}
	}
}
