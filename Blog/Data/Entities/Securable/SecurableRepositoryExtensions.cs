using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Data
{
    public static class SecurableRepositoryExtensions
	{
		public static IEnumerable<Blog> BlogsUserIsAuthorOf(this IRepository<Securable> repository, IRepository<Blog> blogRepo, int userId)
		{
            var securableIds = repository.GetAll().Where(s => s.Members.Any(u=>u.Id == userId)).Select(s=>s.Id);
            return blogRepo.GetAll().Where(b => securableIds.Contains(b.AuthorSecurableId)).ToArray();
		}

        public static void RemoveMember(this IRepository<Securable> repository, IAttacher<Securable> attacher, int userId, int securableId)
        {
            var securable = repository.GetBy(s => s.Id == securableId, s=>s.Members);
            attacher.EnsureAttached(securable);
            var toRemove = securable.Members.Where(m => m.Id == userId).ToList();
            foreach (var user in toRemove)
            {
                securable.Members.Remove(user);
            }

            repository.Update(securable);
        }

        public static void InviteMember(this IRepository<Securable> repository, IRepository<User> userRepo,  string email, int securableId)
        {
            //var securable = repository.GetBy(s => s.Id == securableId);
            //securable.Members.Where(m => m.Id == userId).ToList().ForEach(u => securable.Members.Remove(u));

            //repository.Update(securable);
        }
	}
}
