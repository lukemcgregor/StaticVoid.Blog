using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Repository;
using System.Data.Entity;

namespace StaticVoid.Blog.Data
{
	public static class RedirectRepositoryExtensions
	{
        public static IEnumerable<Redirect> GetRedirects(this IRepository<Redirect> repo, int blogId)
		{
			return repo.GetAll().Where(r=>r.BlogId == blogId).AsNoTracking().AsEnumerable();
		}

        public static Redirect GetRedirectFor(this IRepository<Redirect> repo, string url, int blogId)
        {
            return repo.GetRedirects(blogId).FirstOrDefault(r => r.OldRoute == url);
        }
	}
}
