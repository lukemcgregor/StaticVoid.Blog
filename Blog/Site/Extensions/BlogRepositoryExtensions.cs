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
            //TODO use current url
            return repo.GetAll().First();
        }
    }
}