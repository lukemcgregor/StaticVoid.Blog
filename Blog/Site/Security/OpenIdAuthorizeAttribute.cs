using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StaticVoid.Blog.Site.Models;

namespace StaticVoid.Blog.Site.Security
{
    public class OpenIdAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized)
            {
                var authenticatedCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authenticatedCookie != null)
                {
                    var authenticatedCookieValue = authenticatedCookie.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(authenticatedCookieValue))
                    {
                        var decryptedTicket = FormsAuthentication.Decrypt(authenticatedCookieValue);
                        var user = new OpenIdUser(decryptedTicket.UserData);
                        var openIdIdentity = new OpenIdIdentity(user);
                        httpContext.User = new GenericPrincipal(openIdIdentity, null);
                    }
                }
            }
            return isAuthorized;
        }
    }
}