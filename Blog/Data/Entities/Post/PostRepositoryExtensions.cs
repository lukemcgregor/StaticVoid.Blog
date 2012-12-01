using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StaticVoid.Core.Repository;
using System.Linq.Expressions;

namespace StaticVoid.Blog.Data
{
	public static class PostRepositoryExtensions
	{
        public static IQueryable<Post> PublishedPosts(this IRepository<Post> repo, params Expression<Func<Post, object>>[] includes)
		{
            var set = repo.GetAll();
            foreach (var include in includes)
            {
                set = set.Include(include);
            }
			return set.Where(p => p.Status == PostStatus.Published);
		}

		public static Post LatestPublishedPost(this IRepository<Post> repo)
		{
			return repo.PublishedPosts().OrderByDescending(p=>p.Posted).FirstOrDefault();
		}

		public static Post GetPostBefore(this IRepository<Post> repo, Post currentPost)
		{
			return repo.PublishedPosts().Where(p => p.Posted < currentPost.Posted).OrderByDescending(p => p.Posted).FirstOrDefault();
		}

		public static Post GetPostAfter(this IRepository<Post> repo, Post currentPost)
		{
			return repo.PublishedPosts().Where(p => p.Posted > currentPost.Posted).OrderBy(p => p.Posted).FirstOrDefault();
		}

        public static Post GetPostAtUrl(this IRepository<Post> repo, string url, params Expression<Func<Post, object>>[] includes)
        {
            return repo.PublishedPosts(includes).SingleOrDefault(p => p.Path.ToLower() == url.ToLower());
        }

        public static bool IsUrlAPost(this IRepository<Post> repo, string url)
        {
            return repo.PublishedPosts().Any(p => p.Path.ToLower() == url.ToLower());
        }
	}
}
