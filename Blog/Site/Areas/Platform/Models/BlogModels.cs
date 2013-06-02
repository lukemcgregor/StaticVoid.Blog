using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Platform.Models
{
    public class CreateBlogModel
    {
        [Required]
        public string Name { get; set; }
        [Required, Url]
        public string Url { get; set; }
        [Required, EmailAddress]
        public string AdminEmail { get; set; }
    }
}