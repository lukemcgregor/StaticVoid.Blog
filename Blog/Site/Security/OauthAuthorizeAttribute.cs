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
    public class OauthAuthorizeAttribute : FilterAttribute { }

	//filter
	public class OauthAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IRepository<User> _userRepository;
        private readonly ISecurityHelper _securityHelper;

        public OauthAuthorizeFilter(IRepository<User> userRepository, ISecurityHelper securityHelper)
		{
            _userRepository = userRepository;
            _securityHelper = securityHelper;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (!HttpContext.Current.User.Identity.IsAuthenticated)
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
		}
	}
}