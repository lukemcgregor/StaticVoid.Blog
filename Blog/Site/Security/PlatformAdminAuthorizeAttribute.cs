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
	public class PlatformAdminAuthorizeAttribute : FilterAttribute { }

	//filter
	public class PlatformAdminAuthorizeFilter : IAuthorizationFilter
	{
		private readonly IRepository<Securable> _securableRepository;
		private readonly ISecurityHelper _securityHelper;

		public PlatformAdminAuthorizeFilter(IRepository<Securable> securableRepository, ISecurityHelper securityHelper)
		{
			_securityHelper = securityHelper;
			_securableRepository = securableRepository;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated &&
				_securityHelper.CurrentUser != null)
			{
				if (_securityHelper.CurrentUser.IsPlatformAdmin(_securableRepository))
				{
					return; //no unauthorized
				}
			}
			filterContext.Result = new HttpUnauthorizedResult();
		}
	}
}