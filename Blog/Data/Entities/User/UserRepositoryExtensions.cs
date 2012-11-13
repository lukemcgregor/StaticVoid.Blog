using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Data
{
	public static class UserRepositoryExtensions
	{
		public static User EnsureUser(this IRepository<User> repository, User user)
		{
			var existingUser = repository.GetBy(u => u.ClaimedIdentifier == user.ClaimedIdentifier);
			if (existingUser == null)
			{
				repository.Create(user);
                return user;
			}
			else//update existing deets
			{
				existingUser.Email = user.Email;
				existingUser.FirstName = user.FirstName;
				existingUser.LastName = user.LastName;
				repository.Update(existingUser);
                
                return existingUser;
			}

		}
	}
}
