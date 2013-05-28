using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Platform.Models
{
    public class PlatformDashboardModel
    {
        public List<DashboardBlogModel> Blogs { get; set; }
    }

    public class DashboardBlogModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}