using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using StaticVoid.Blog.Site.Models;

namespace StaticVoid.Blog.Site.Security
{
    public class OpenIdMembershipService : IOpenIdMembershipService
    {
        private readonly OpenIdRelyingParty openId;

        public OpenIdMembershipService()
        {
            openId = new OpenIdRelyingParty();
        }

        public IAuthenticationRequest ValidateAtOpenIdProvider(string openIdIdentifier)
        {
            // We only want supported providers as unsupported providers could claim a email address
            // and gain access without the owner of that address ever giving consent. The only way to
            // get a google OpenID account is with an email address which has been validated.
            if (openIdIdentifier != "https://www.google.com/accounts/o8/id")
            {
                throw new NotSupportedException("Only google is supported for openauth");
            }

            IAuthenticationRequest openIdRequest = openId.CreateRequest(Identifier.Parse(openIdIdentifier));
			
            var fetch = new FetchRequest();

			fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email,true,1));
			fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.FullName, true, 1));
			fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true,1));
			fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true,1));
			fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Alias, true, 1));


            openIdRequest.AddExtension(fetch);
            return openIdRequest;
        }

        public OpenIdUser GetUser()
        {
            OpenIdUser user = null;
            IAuthenticationResponse openIdResponse = openId.GetResponse();

            if (openIdResponse.IsSuccessful())
            {
                user = ResponseIntoUser(openIdResponse);
            }

            return user;
        }

        private OpenIdUser ResponseIntoUser(IAuthenticationResponse response)
        {
            OpenIdUser user = null;
            var claimResponseUntrusted = response.GetUntrustedExtension<FetchResponse>();
			var claimResponse = response.GetExtension<FetchResponse>();

            if (claimResponse != null)
            {
                user = new OpenIdUser(claimResponse, response.ClaimedIdentifier);
            }
            else if (claimResponseUntrusted != null)
            {
                user = new OpenIdUser(claimResponseUntrusted, response.ClaimedIdentifier);
            }

            return user;
        }

        public HttpCookie CreateFormsAuthenticationCookie(OpenIdUser user)
        {
            var ticket = new FormsAuthenticationTicket(1, user.Nickname, DateTime.Now, DateTime.Now.AddDays(7), true, user.ToString());
            var encrypted = FormsAuthentication.Encrypt(ticket).ToString();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

            return cookie;
        }
    }
}