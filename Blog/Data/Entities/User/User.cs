using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticVoid.Blog.Data
{
	public class User
	{
		public int Id { get; set; }
		public string ClaimedIdentifier { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        public string CreatedVia { get; set; }
        public string GooglePlusProfileUrl { get; set; }
        public ICollection<Securable> Securables { get; set; }
	}
}
