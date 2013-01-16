using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StaticVoid.Blog.Data;

namespace StaticVoid.Blog.Site.Areas.Manage.Models
{
	public class PostCreateModel
	{
        public String Title { get; set; }
        public String Description { get; set; }
		public String Body { get; set; }
		public String CanonicalUrl { get; set; }
		public bool Reposted { get; set; }
	}

	public class PostEditModel
	{
        public String Title { get; set; }
        public String Description { get; set; }
        public String Body { get; set; }
		public String CanonicalUrl { get; set; }
		public bool Reposted { get; set; }
	}

	public class PostModel
	{
		public int Id { get; set; }
		public String Title { get; set; }
		public PostStatus Status { get; set; }
		public bool HasDraftContent { get; set; }
        public DateTime Posted { get; set; }
    }

    public class PostUrlEditModel
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Url { get; set; }
    }

    public class ConfirmPublishModel
    {
        public int Id { get; set; }
        public String Title { get; set; }
    }
}