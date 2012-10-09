using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public static class PostExtensions
	{
		public static bool HasDraftContent(this Post post)
		{
			return !String.IsNullOrWhiteSpace(post.DraftBody) || !String.IsNullOrWhiteSpace(post.DraftTitle);
		}

		public static string GetDraftTitle(this Post post)
		{
			return String.IsNullOrWhiteSpace(post.DraftTitle) ? post.Title : post.DraftTitle;
		}

		public static string GetDraftBody(this Post post)
		{
			return String.IsNullOrWhiteSpace(post.DraftBody) ? post.Body : post.DraftBody;
		}
	}
}
