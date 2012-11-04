using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StaticVoid.Blog.Site.Gravitar
{
    public static class GravatarExtensions
    {
        public static string GravitarUrlFromEmail(this string email)
        {
            var md5 = MD5.Create();
            var hashedEmail = BitConverter.ToString(md5.ComputeHash(new UTF8Encoding().GetBytes(email.Trim().ToLower()))).Replace("-", "").ToLower();

            return String.Format("http://www.gravatar.com/avatar/{0}", hashedEmail);
        }
    }
}