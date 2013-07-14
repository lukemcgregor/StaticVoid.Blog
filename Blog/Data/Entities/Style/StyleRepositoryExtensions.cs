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
    public static class StyleRepositoryExtensions
	{
        public static Style GetById(this IRepository<Style> repo, Guid id, params Expression<Func<Style, object>>[] includes)
        {
            return repo.GetAll(includes).SingleOrDefault(i => i.Id == id);
        }
	}
}
