using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Data
{
    public static class BlogRepositoryExtensions
    {
        public static Data.Blog CurrentBlog(this IRepository<Data.Blog> repo)
        {
            var host = HttpContext.Current.Request.Url.Host;

            //TODO use current url
            return repo.GetAll().Where(b=> new Uri(b.AuthoritiveUrl).Host == host).First();
        }
    }
}