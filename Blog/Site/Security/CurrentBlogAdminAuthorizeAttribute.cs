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
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Security
{
	public class CurrentBlogAdminAuthorizeAttribute : FilterAttribute { }

	//filter
	public class CurrentBlogAdminAuthorizeFilter : IAuthorizationFilter
	{
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<Data.Blog> _blogRepository;
		private readonly IRepository<Securable> _securableRepository;
		private readonly ISecurityHelper _securityHelper;
		private readonly IHttpContextService _httpContext;

		public CurrentBlogAdminAuthorizeFilter(
			IRepository<User> userRepository,
			IRepository<Data.Blog> blogRepository,
			IRepository<Securable> securableRepository,
			ISecurityHelper securityHelper,
			IHttpContextService httpContext)
		{
			_userRepository = userRepository;
			_blogRepository = blogRepository;
			_securableRepository = securableRepository;
			_securityHelper = securityHelper;
			_httpContext = httpContext;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var currentBlog = _blogRepository.GetCurrentBlog(_httpContext);
			if (HttpContext.Current.User.Identity.IsAuthenticated &&
				_securityHelper.CurrentUser != null)
			{
				if (_securityHelper.CurrentUser.IsAdminOfBlog(currentBlog, _securableRepository))
				{
					return; //no unauthorized
				}
			}
			filterContext.Result = new HttpUnauthorizedResult();
		}
	}
}