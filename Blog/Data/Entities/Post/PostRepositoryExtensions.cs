using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Data
{
	public static class PostRepositoryExtensions
	{
		public static IQueryable<Post> PublishedPosts(this IRepository<Post> repo)
		{
			return repo.GetAll().Where(p => p.Status == PostStatus.Published);
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
	}
}
