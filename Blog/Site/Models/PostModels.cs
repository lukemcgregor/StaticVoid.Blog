using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaticVoid.Blog.Site.Models
{
	public class PostModel
	{
		public String Title { get; set; }
		public String Body { get; set; }
		public DateTime Posted { get; set; }
		public String CanonicalUrl { get; set; }

        public PartialPostForLinkModel NextPost { get; set; }

        public PartialPostForLinkModel PreviousPost { get; set; }

        public PostAuthor Author { get; set; }
	}

    public class PartialPostForLinkModel
    {
        public String Title { get; set; }
        public String Link { get; set; }
        public DateTime Date { get; set; }
    }

    public class PostAuthor
    {
        public string Name { get; set; }
        public string GravatarUrl { get; set; }
        public string GooglePlusProfileUrl { get; set; }
    }
}