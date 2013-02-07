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
    public static class PostModificationRepositoryExtensions
	{
        public static IQueryable<PostModification> ModificationsForPost(this IRepository<PostModification> repo, int postId)
		{
			return repo.GetAll().Where(p => p.Post.Id == postId);
		}

        public static DateTime PostLastModificationDate(this IRepository<PostModification> repo, int postId)
        {
            return repo.GetAll().Where(p => p.Post.Id == postId).Max(m=>m.Timestamp);
        }
	}
}
