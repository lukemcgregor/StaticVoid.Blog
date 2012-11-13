using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public class Post
	{
		public int Id { get; set; }
		public DateTime Posted { get; set; }
		public String Title { get; set; }
		public String Body { get; set; }
		public String DraftTitle { get; set; }
		public String DraftBody { get; set; }
		public User Author { get; set; }

		public PostStatus Status { get; set; }

		public String Canonical { get; set; }
		public String Path { get; set; }

        public Guid PostGuid { get; set; }
	}

	public enum PostStatus
	{
		Draft,
		Published,
		Unpublished
	}
}
