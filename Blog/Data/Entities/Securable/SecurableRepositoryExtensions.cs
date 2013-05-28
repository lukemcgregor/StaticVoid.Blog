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
		public static IEnumerable<Blog> BlogsUserIsAdminOf(this IRepository<Securable> repository, IRepository<Blog> blogRepo, int userId)
		{
            var securableIds = repository.GetAll().Where(s => s.Members.Any(u=>u.Id == userId)).Select(s=>s.Id);
            return blogRepo.GetAll().Where(b => securableIds.Contains(b.AdminSecurableId)).ToArray();
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

        public static bool IsMemberOfSecurable(this IRepository<Securable> repository, int securableId, int userId)
        {
            return repository.GetAll().Any(s => s.Id == securableId && s.Members.Any(m => m.Id == userId));
        }
	}
}
