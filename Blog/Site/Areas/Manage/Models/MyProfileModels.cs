using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
    public class MyProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DisplayName("Google+ Profile Url")]
        [Description("Used to populate photo into search results and as a link behind Author Gravatar")]
        public string GooglePlusProfileUrl { get; set; }
    }
}