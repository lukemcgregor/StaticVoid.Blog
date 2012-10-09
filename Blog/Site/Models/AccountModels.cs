using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using System.Security.Cryptography;
using System.Text;

namespace StaticVoid.Blog.Site.Models
{
    public class OpenIdUser
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string FullName { get; set; }
        public bool IsSignedByProvider { get; set; }
		public string ClaimedIdentifier { get; set; }
		private string _gravatarUrl;


        public OpenIdUser(string data)
        {
            PopulateFromDelimitedString(data);
        }

		public OpenIdUser(FetchResponse claim, string identifier)
		{
            AddClaimInfo(claim, identifier);
        }

        private void AddClaimInfo(FetchResponse claim, string identifier)
        {
            Email = claim.Attributes[WellKnownAttributes.Contact.Email].Values.FirstOrDefault();

			if (claim.Attributes.Contains(WellKnownAttributes.Name.FullName))
			{
				FullName = claim.Attributes[WellKnownAttributes.Name.FullName].Values.First();
			}
			else
			{
				if (claim.Attributes.Contains(WellKnownAttributes.Name.First))
				{
					FullName = claim.Attributes[WellKnownAttributes.Name.First].Values.FirstOrDefault();
				}

				if (claim.Attributes.Contains(WellKnownAttributes.Name.Last))
				{
					FullName += " ";
					FullName += claim.Attributes[WellKnownAttributes.Name.Last].Values.FirstOrDefault();
				}
			}

            Nickname = FullName ?? Email;
            IsSignedByProvider = claim.IsSignedByProvider;
            ClaimedIdentifier = identifier;
        }

        private void PopulateFromDelimitedString(string data)
        {
            if (data.Contains(";"))
            {
                var stringParts = data.Split(';');
                if (stringParts.Length > 0) Email = stringParts[0];
                if (stringParts.Length > 1) FullName = stringParts[1];
                if (stringParts.Length > 2) Nickname = stringParts[2];
                if (stringParts.Length > 3) ClaimedIdentifier = stringParts[3];
            }
        }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3}", Email, FullName, Nickname, ClaimedIdentifier);
        }

		public string GravatarUrl
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_gravatarUrl))
				{
					var md5 = MD5.Create();
					var hashedEmail = BitConverter.ToString(md5.ComputeHash(new UTF8Encoding().GetBytes(Email.Trim().ToLower()))).Replace("-", "").ToLower();

					_gravatarUrl = String.Format("http://www.gravatar.com/avatar/{0}", hashedEmail);
				}

				return _gravatarUrl;
			}
		}
    }
}
