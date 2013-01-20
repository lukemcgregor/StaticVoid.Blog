using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    public class BlogConfigModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BlogGuid { get; set; }
        public string Twitter { get; set; }
        public string AuthoritiveUrl { get; set; }
        public string AnalyticsKey { get; set; }
        public string DisqusShortname { get; set; }
        public Guid? BlogStyleId { get; set; }
    }
}