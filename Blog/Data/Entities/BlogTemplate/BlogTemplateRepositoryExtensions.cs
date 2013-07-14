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
    public static class BlogTemplateRepositoryExtensions
	{
        public static BlogTemplate GetById(this IRepository<BlogTemplate> repo, Guid id, params Expression<Func<BlogTemplate, object>>[] includes)
        {
            return repo.GetAll(includes).SingleOrDefault(i => i.Id == id);
        }

        public static BlogTemplate GetLatestEditForBlog(this IRepository<BlogTemplate> repo, int blogId, params Expression<Func<BlogTemplate, object>>[] includes)
        {
            return repo
                .GetAll(includes)
                .Where(t=>t.BlogId == blogId)
                .OrderByDescending(t=>t.LastModified)
                .FirstOrDefault();
        }
	}
}
