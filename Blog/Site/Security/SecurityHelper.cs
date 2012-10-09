using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Models;

namespace StaticVoid.Blog.Site
{
	public static class SecurityHelper
	{
		public static OpenIdUser CurrentUser
		{
			get
			{
				if (HttpContext.Current.User.Identity.IsAuthenticated)
				{
					var authenticatedCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
					if (authenticatedCookie != null)
					{
						var authenticatedCookieValue = authenticatedCookie.Value.ToString();
						if (!string.IsNullOrWhiteSpace(authenticatedCookieValue))
						{
							var decryptedTicket = FormsAuthentication.Decrypt(authenticatedCookieValue);
							return new OpenIdUser(decryptedTicket.UserData);
						}
					}
				}
				return null;
			}
		}
	}
}