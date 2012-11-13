using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site
{
    public interface IProvideBlogConfiguration
    {
        Blog.Data.Blog CurrentBlog { get; }
    }
}