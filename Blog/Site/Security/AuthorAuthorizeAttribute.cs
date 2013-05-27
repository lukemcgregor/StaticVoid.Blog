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
	public class AuthorAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Data.Blog> _blogRepository;
        private readonly IRepository<Securable> _securableRepository;
        private readonly ISecurityHelper _securityHelper;

        public AuthorAuthorizeFilter(IRepository<User> userRepository, IRepository<Data.Blog> blogRepository, IRepository<Securable> securableRepository, ISecurityHelper securityHelper)
		{
            _userRepository = userRepository;
            _blogRepository = blogRepository;
            _securableRepository = securableRepository;
            _securityHelper = securityHelper;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
            var currentBlog = _blogRepository.CurrentBlog();
			if (!HttpContext.Current.User.Identity.IsAuthenticated || 
				_securityHelper.CurrentUser == null ||
                String.IsNullOrWhiteSpace(_securityHelper.CurrentUser.ClaimedIdentifier) ||
                !_securableRepository.GetBy(s => s.Id == currentBlog.AuthorSecurableId, s => s.Members).Members.Any(m => m.ClaimedIdentifier == _securityHelper.CurrentUser.ClaimedIdentifier))
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
		}
	}
}