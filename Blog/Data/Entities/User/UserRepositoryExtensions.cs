using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Data
{
	public static class UserRepositoryExtensions
	{
		public static User GetById(this IQueryable<User> users, int id)
		{
			return users.SingleOrDefault(u => u.Id == id);
		}
	}
}
