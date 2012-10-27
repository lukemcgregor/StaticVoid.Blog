using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public class Visit
	{
		public int Id { get; set; }
		public String IpAddress { get; set; }
		public String Browser { get; set; }
		public String UserAgent { get; set; }
		public String Url { get; set; }
		public String Languages { get; set; }
		public String Referrer { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
