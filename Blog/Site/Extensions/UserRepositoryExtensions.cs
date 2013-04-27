using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StaticVoid.Blog.Data;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Site
{
	public static class UserRepositoryExtensions
	{
		public static User GetCurrentUser(this IRepository<User> repo, ISecurityHelper securityHelper)
		{
            var currentUser = securityHelper.CurrentUser;

			if (currentUser != null && !String.IsNullOrWhiteSpace(currentUser.ClaimedIdentifier))
			{
                return repo.GetBy(u => u.ClaimedIdentifier == currentUser.ClaimedIdentifier);
			}

			return null;
		}
	}
}