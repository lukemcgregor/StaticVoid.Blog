using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OpenId.RelyingParty;
using StaticVoid.Blog.Site.Models;

namespace StaticVoid.Blog.Site.Security
{
    public interface IOpenIdMembershipService
    {
        IAuthenticationRequest ValidateAtOpenIdProvider(string openIdIdentifier);
        OpenIdUser GetUser();
    }
}