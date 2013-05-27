using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StaticVoid.Blog.Data
{
    public class CachedStyleRepositoryStorage : ICachedStyleRepositoryStorage
    {
        public CachedStyleRepositoryStorage(SimpleRepository<Data.Style> baseRepo)
        {
            CachedStyles = baseRepo.GetAll().AsNoTracking().ToList();
        }

        public List<Style> CachedStyles { get; private set; }
    }

    public interface ICachedStyleRepositoryStorage
    {
        List<Style> CachedStyles { get; }
    }

    public class CachedStyleRepository : IRepository<Style>
    {
        private ICachedStyleRepositoryStorage _cache;
        private readonly IRepository<Data.Style> _baseRepo;
        public CachedStyleRepository(SimpleRepository<Data.Style> baseRepo, ICachedStyleRepositoryStorage cache)
        {
            _cache = cache;
            _baseRepo = baseRepo;
        }

        public void Create(Style entity)
        {
            _baseRepo.Create(entity);
            _cache.CachedStyles.Add(entity);
        }

        public void Delete(Style entity)
        {
            _baseRepo.Delete(entity);
            InternalRemove(entity);
        }
        private void InternalRemove(Style entity)
        {
            if (_cache.CachedStyles.Any(b => b.Id == entity.Id))
            {
                _cache.CachedStyles.Remove(_cache.CachedStyles.Single(b => b.Id == entity.Id));
            }
        }

        public IQueryable<Style> GetAll(params System.Linq.Expressions.Expression<Func<Style, object>>[] includes)
        {
            if (includes != null)
            {
                return _baseRepo.GetAll(includes);
            }
            return _cache.CachedStyles.AsQueryable();
        }

        public Style GetBy(System.Linq.Expressions.Expression<Func<Style, bool>> predicate, params System.Linq.Expressions.Expression<Func<Style, object>>[] includes)
        {
            if (includes != null && includes.Any())
            {
                return _baseRepo.GetBy(predicate, includes);
            }
            return _cache.CachedStyles.SingleOrDefault(predicate.Compile());
        }

        public void Update(Style entity)
        {
            _baseRepo.Update(entity);
            InternalRemove(entity);
            _cache.CachedStyles.Add(entity);
        }

        public void Dispose()
        {
        }
    }
}
