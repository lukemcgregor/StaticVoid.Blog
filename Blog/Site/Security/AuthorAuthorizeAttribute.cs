using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Site.Security
{
	public class AuthorAuthorizeAttribute : FilterAttribute { }

	//filter
	public class MyAuthorizeFilter : IAuthorizationFilter
	{
        private readonly IRepository<User> _userRepository;
        private readonly ISecurityHelper _securityHelper;
        public MyAuthorizeFilter(IRepository<User> userRepository, ISecurityHelper securityHelper)
		{
            _userRepository = userRepository;
            _securityHelper = securityHelper;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (!HttpContext.Current.User.Identity.IsAuthenticated || 
				_securityHelper.CurrentUser == null ||
                String.IsNullOrWhiteSpace(_securityHelper.CurrentUser.ClaimedIdentifier) ||
                !_userRepository.GetBy(u => u.ClaimedIdentifier == _securityHelper.CurrentUser.ClaimedIdentifier).IsAuthor)
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
		}
	}
}