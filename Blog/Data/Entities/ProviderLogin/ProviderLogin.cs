using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public class ProviderLogin
	{
		public ProviderType Provider { get; set; }
		public string ProviderKey { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}

	public enum ProviderType
	{
		Google = 1,
		Microsoft = 2,
		Facebook = 3,
		Twitter = 4
	}
}
