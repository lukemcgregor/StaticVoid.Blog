using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StaticVoid.Blog.Data
{
    public class CachedBlogTemplateRepositoryStorage : ICachedBlogTemplateRepositoryStorage
    {
        public CachedBlogTemplateRepositoryStorage(SimpleRepository<Data.BlogTemplate> baseRepo)
        {
            CachedBlogTemplates = baseRepo.GetAll().AsNoTracking().ToList();
        }

        public List<BlogTemplate> CachedBlogTemplates { get; private set; }
    }

    public interface ICachedBlogTemplateRepositoryStorage
    {
        List<BlogTemplate> CachedBlogTemplates { get; }
    }

    public class CachedBlogTemplateRepository : IRepository<BlogTemplate>
    {
        private ICachedBlogTemplateRepositoryStorage _cache;
        private readonly IRepository<Data.BlogTemplate> _baseRepo;
        public CachedBlogTemplateRepository(SimpleRepository<Data.BlogTemplate> baseRepo, ICachedBlogTemplateRepositoryStorage cache)
        {
            _cache = cache;
            _baseRepo = baseRepo;
        }

        public void Create(BlogTemplate entity)
        {
            _baseRepo.Create(entity);
            _cache.CachedBlogTemplates.Add(entity);
        }

        public void Delete(BlogTemplate entity)
        {
            _baseRepo.Delete(entity);
            InternalRemove(entity);
        }
        private void InternalRemove(BlogTemplate entity)
        {
            if (_cache.CachedBlogTemplates.Any(b => b.Id == entity.Id))
            {
                _cache.CachedBlogTemplates.Remove(_cache.CachedBlogTemplates.Single(b => b.Id == entity.Id));
            }
        }

        public IQueryable<BlogTemplate> GetAll(params System.Linq.Expressions.Expression<Func<BlogTemplate, object>>[] includes)
        {
            if (includes != null)
            {
                return _baseRepo.GetAll(includes);
            }
            return _cache.CachedBlogTemplates.AsQueryable();
        }

        public BlogTemplate GetBy(System.Linq.Expressions.Expression<Func<BlogTemplate, bool>> predicate, params System.Linq.Expressions.Expression<Func<BlogTemplate, object>>[] includes)
        {
            if (includes != null && includes.Any())
            {
                return _baseRepo.GetBy(predicate, includes);
            }
            return _cache.CachedBlogTemplates.SingleOrDefault(predicate.Compile());
        }

        public void Update(BlogTemplate entity)
        {
            _baseRepo.Update(entity);
            InternalRemove(entity);
            _cache.CachedBlogTemplates.Add(entity);
        }

        public void Dispose()
        {
        }
    }
}
