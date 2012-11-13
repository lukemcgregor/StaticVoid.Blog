using StaticVoid.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site
{
    public class CachedCurrentBlogProvider : IProvideBlogConfiguration
    {
        private Dictionary<string, Blog.Data.Blog> _blogCache = new Dictionary<string, Data.Blog>();

        public CachedCurrentBlogProvider(IRepository<Blog.Data.Blog> blogRepository)
        {
            foreach (var b in blogRepository.GetAll())
            {
                _blogCache.Add(b.AuthoritiveUrl, b);
            }
        }

        public Data.Blog CurrentBlog
        {
            get
            {
                //todo make this work off url as part of making blog multi-tennanted
                return _blogCache.First().Value;
            }
        }
    }
}