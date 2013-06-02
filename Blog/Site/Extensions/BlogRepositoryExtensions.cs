using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Data
{
    public static class BlogRepositoryExtensions
    {
        public static Data.Blog GetCurrentBlog(this IRepository<Data.Blog> repo, IHttpContextService httpContext)
        {
            var host = httpContext.RequestUrl.Host;

            //TODO use current url
            return repo.GetAll().ToArray().Where(b=> new Uri(b.AuthoritiveUrl).Host == host).FirstOrDefault();
        }
    }
}