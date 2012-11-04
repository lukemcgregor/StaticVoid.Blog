using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Site.Security
{
	public class AuthorAuthorizeAttribute : FilterAttribute { }

	//filter
	public class MyAuthorizeFilter : IAuthorizationFilter
	{
		private readonly IRepository<User> _userRepository;
		public MyAuthorizeFilter(IRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (!HttpContext.Current.User.Identity.IsAuthenticated || 
				SecurityHelper.CurrentUser == null || 
				String.IsNullOrWhiteSpace(SecurityHelper.CurrentUser.ClaimedIdentifier) ||
                !_userRepository.GetBy(u => u.ClaimedIdentifier == SecurityHelper.CurrentUser.ClaimedIdentifier).IsAuthor)
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
		}
	}
}