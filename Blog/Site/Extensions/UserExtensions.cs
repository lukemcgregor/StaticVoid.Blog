using StaticVoid.Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site
{
    public static class UserExtensions
    {
        public static string FullName(this User user)
        {
            return String.Format("{0} {1}", user.FirstName, user.LastName);
        }
    }
}