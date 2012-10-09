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

		public bool HasNextPost { get; set; }
		public String NextPostTitle { get; set; }
		public String NextPostLink { get; set; }
		public DateTime NextPostDate { get; set; }

		public bool HasPreviousPost { get; set; }
		public String PreviousPostTitle { get; set; }
		public String PreviousPostLink { get; set; }
		public DateTime PreviousPostDate { get; set; }
	}
}