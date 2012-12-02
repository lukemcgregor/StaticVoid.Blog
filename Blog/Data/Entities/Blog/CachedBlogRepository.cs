using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StaticVoid.Blog.Data
{
    public class CachedBlogRepositoryStorage : ICachedBlogRepositoryStorage
    {
        public CachedBlogRepositoryStorage(SimpleRepository<Data.Blog> baseRepo)
        {
            CachedBlogs = baseRepo.GetAll().AsNoTracking().ToList();
        }

        public List<Data.Blog> CachedBlogs { get; private set; }
    }

    public interface ICachedBlogRepositoryStorage
    {
        List<Data.Blog> CachedBlogs { get; }
    }

    public class CachedBlogRepository : IRepository<Data.Blog>
    {
        private ICachedBlogRepositoryStorage _cache;
        private readonly IRepository<Data.Blog> _baseRepo;
        public CachedBlogRepository(SimpleRepository<Data.Blog> baseRepo, ICachedBlogRepositoryStorage cache)
        {
            _cache = cache;
            _baseRepo = baseRepo;
        }

        public void Create(Blog entity)
        {
            _baseRepo.Create(entity);
            _cache.CachedBlogs.Add(entity);
        }

        public void Delete(Blog entity)
        {
            _baseRepo.Delete(entity);
            InternalRemove(entity);
        }
        private void InternalRemove(Blog entity)
        {
            if (_cache.CachedBlogs.Any(b => b.Id == entity.Id))
            {
                _cache.CachedBlogs.Remove(_cache.CachedBlogs.Single(b => b.Id == entity.Id));
            }
        }

        public IQueryable<Blog> GetAll()
        {
            return _cache.CachedBlogs.AsQueryable();
        }

        public Blog GetBy(System.Linq.Expressions.Expression<Func<Blog, bool>> predicate, params System.Linq.Expressions.Expression<Func<Blog, object>>[] includes)
        {
            if (includes != null)
            {
                return _baseRepo.GetBy(predicate, includes);
            }
            return _cache.CachedBlogs.SingleOrDefault(predicate.Compile());
        }

        public void Update(Blog entity)
        {
            _baseRepo.Update(entity);
            InternalRemove(entity);
            _cache.CachedBlogs.Add(entity);
        }

        public void Dispose()
        {
        }
    }
}
