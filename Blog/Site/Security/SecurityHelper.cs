using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;
using System.Security.Claims;

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


		//public OpenIdUser CurrentUser
		//{
		//	get
		//	{
		//		if (HttpContext.Current.User.Identity.IsAuthenticated)
		//		{
		//			var authenticatedCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
		//			if (authenticatedCookie != null)
		//			{
		//				var authenticatedCookieValue = authenticatedCookie.Value.ToString();
		//				if (!string.IsNullOrWhiteSpace(authenticatedCookieValue))
		//				{
		//					var decryptedTicket = FormsAuthentication.Decrypt(authenticatedCookieValue);
		//					return new OpenIdUser(decryptedTicket.UserData);
		//				}
		//			}
		//		}
		//		return null;
		//	}
		//}

		///// <summary>
		///// This is not mockable, please dont use me anywhere you dont have to, this is only there so
		///// in a non-injectable place like a view with no model this can still be used eg _LoginPartial
		///// </summary>
		//public static OpenIdUser UnsafeCurrentUser
		//{
		//	get
		//	{
		//		return new SecurityHelper().CurrentUser;
		//	}
		//}
	}
}