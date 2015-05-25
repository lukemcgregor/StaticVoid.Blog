using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using System.Security.Claims;
using StaticVoid.Blog.Site.Gravitar;

namespace StaticVoid.Blog.Site
{
    public class SecurityHelper : ISecurityHelper
	{
		private readonly IBlogContext _context;

		public SecurityHelper(IBlogContext context)
		{
			_context = context;
		}

		public User CurrentUser
		{
			get
			{
				return GetCurrentUser(HttpContext.Current);
			}
		}

		public User GetCurrentUser(HttpContext context)
		{
			
			if (context.User != null && context.User.Identity.IsAuthenticated)
			{
				var id = int.Parse(((ClaimsIdentity)context.User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
				return _context.Users.AsNoTracking().GetById(id);
			}
			return null;
		}

		public static int? CurrentUserId
		{
			get
			{
				if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
				{
					return int.Parse(((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
				}
				return null;
			}
		}

		public static string CurrentUserName
		{
			get
			{
				if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
				{
					return ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.Name).Value;
				}
				return null;
			}
		}

		public static string CurrentUserPicture
		{
			get
			{
				if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
				{
					return ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.Email).Value.GravitarUrlFromEmail();
				}
				return null;
			}
		}
	}
}