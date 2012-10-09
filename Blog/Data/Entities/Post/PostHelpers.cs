using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public static class PostHelpers
	{
		public static string MakeUrl(int year, int month, int day, string title)
		{
			return String.Format("{0}/{1}/{2}/{3}",year,month,day, title.ToLower().Replace(' ', '_'));
		}
	}
}
