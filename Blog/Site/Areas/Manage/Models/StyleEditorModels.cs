using StaticVoid.Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    public class StyleModel
    {
        public string HtmlTemplate { get; set; }
        public string Css { get; set; }
        public TemplateMode TemplateMode { get; set; }
    }
}