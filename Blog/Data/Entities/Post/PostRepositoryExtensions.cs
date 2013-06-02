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
	public static class PostRepositoryExtensions
	{

        public static IQueryable<Post> PostsForBlog(this IRepository<Post> repo, int blogId, params Expression<Func<Post, object>>[] includes)
        {
            var set = repo.GetAll();
            foreach (var include in includes)
            {
                set = set.Include(include);
            }
            return set.Where(p => p.BlogId == blogId);
        }

        public static IQueryable<Post> PublishedPosts(this IRepository<Post> repo, int blogId, params Expression<Func<Post, object>>[] includes)
		{
            return repo.PostsForBlog(blogId, includes).Where(p => p.Status == PostStatus.Published);
		}

        public static IQueryable<Post> FeedPosts(this IRepository<Post> repo, int blogId, params Expression<Func<Post, object>>[] includes)
        {
            return repo.PublishedPosts(blogId, includes).Where(p => !p.ExcludeFromFeed);
        }

        public static Post LatestPublishedPost(this IRepository<Post> repo, int blogId)
		{
			return repo.PublishedPosts(blogId).OrderByDescending(p=>p.Posted).FirstOrDefault();
		}

		public static Post GetPostBefore(this IRepository<Post> repo, Post currentPost)
		{
			return repo.PublishedPosts(currentPost.BlogId).Where(p => p.Posted < currentPost.Posted).OrderByDescending(p => p.Posted).FirstOrDefault();
		}

		public static Post GetPostAfter(this IRepository<Post> repo, Post currentPost)
		{
            return repo.PublishedPosts(currentPost.BlogId).Where(p => p.Posted > currentPost.Posted).OrderBy(p => p.Posted).FirstOrDefault();
		}

        public static Post GetPostAtUrl(this IRepository<Post> repo, int blogId, string url, params Expression<Func<Post, object>>[] includes)
        {
            return repo.PublishedPosts(blogId, includes).SingleOrDefault(p => p.Path.ToLower() == url.ToLower());
        }

        public static bool IsUrlAPost(this IRepository<Post> repo, int blogId, string url)
        {
            return repo.PublishedPosts(blogId).Any(p => p.Path.ToLower() == url.ToLower());
        }
	}
}
