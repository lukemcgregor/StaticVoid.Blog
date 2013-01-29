using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    public class DashboardModel
    {
        public List<Tuple<string,int>> Posts { get; set; }
    }

    public class DashboardPostModel
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Url { get; set; }
        public String Description { get; set; }
    }
}