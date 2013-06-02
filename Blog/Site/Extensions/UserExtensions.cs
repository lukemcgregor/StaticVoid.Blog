using StaticVoid.Blog.Data;
using StaticVoid.Repository;
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

        public static bool IsAuthorOfBlog(this User user, Data.Blog blog, IRepository<Securable> securableRepo)
        {
            return securableRepo.IsMemberOfSecurable(blog.AuthorSecurableId, user.Id) || user.IsAdminOfBlog(blog,securableRepo);
        }

        public static bool IsAdminOfBlog(this User user, Data.Blog blog, IRepository<Securable> securableRepo)
        {
            return securableRepo.IsMemberOfSecurable(blog.AdminSecurableId, user.Id) || user.IsPlatformAdmin(securableRepo);
        }

        public static bool IsPlatformAdmin(this User user, IRepository<Securable> securableRepo)
        {
            //TODO no magic number here would be nice. Find somewhere to put the platform admin securable id
            return securableRepo.IsMemberOfSecurable(1, user.Id);
        }
    }
}