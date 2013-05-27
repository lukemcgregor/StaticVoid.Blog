using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StaticVoid.Repository;
using System.Linq.Expressions;

namespace StaticVoid.Blog.Data
{
    public static class InvitationRepositoryExtensions
	{
        public static IQueryable<Invitation> GetActiveInvitations(this IRepository<Invitation> repo, params Expression<Func<Invitation, object>>[] includes)
        {
            var set = repo.GetAll();
            foreach (var include in includes)
            {
                set = set.Include(include);
            }
            return set;
        }

        public static void RevokeInvitation(this IRepository<Invitation> repo, string email, int securableId)
		{
            var toRemove = repo.GetActiveInvitations().Where(i => i.SecurableId == securableId && i.Email == email).ToArray();

            foreach (var i in toRemove)
            {
                repo.Delete(i);
            }
		}

        public static IQueryable<Invitation> GetActiveBySecurable(this IRepository<Invitation> repo, int securableId, params Expression<Func<Invitation, object>>[] includes)
        {
            return repo.GetActiveInvitations(includes).Where(i => i.SecurableId == securableId);
        }

        public static Invitation GetActiveByToken(this IRepository<Invitation> repo, string token, params Expression<Func<Invitation, object>>[] includes)
        {
            return repo.GetActiveInvitations(includes).SingleOrDefault(i => i.Token == token);
        }
	}
}
