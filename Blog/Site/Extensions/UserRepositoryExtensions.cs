using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StaticVoid.Blog.Data;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Site
{
	public static class UserRepositoryExtensions
	{
		public static User GetCurrentUser(this IRepository<User> repo)
		{
			var currentUser = SecurityHelper.CurrentUser;

			if (currentUser != null && !String.IsNullOrWhiteSpace(currentUser.Email))
			{
				return repo.GetBy(u=>u.Email == currentUser.Email);
			}

			return null;
		}
	}
}