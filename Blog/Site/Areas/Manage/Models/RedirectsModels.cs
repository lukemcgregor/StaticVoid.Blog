using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    public class RedirectModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Temporary { get; set; }
    }
}