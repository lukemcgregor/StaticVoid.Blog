using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace StaticVoid.Blog.Data
{
    public class CachedRedirectRepositoryStorage : ICachedRedirectRepositoryStorage
    {
        public CachedRedirectRepositoryStorage(SimpleRepository<Data.Redirect> baseRepo)
        {
            CachedRedirects = baseRepo.GetAll().AsNoTracking().ToList();
        }

        public List<Data.Redirect> CachedRedirects { get; private set; }
    }

    public interface ICachedRedirectRepositoryStorage
    {
        List<Data.Redirect> CachedRedirects { get; }
    }

    public class CachedRedirectRepository : IRepository<Data.Redirect>
    {
        private ICachedRedirectRepositoryStorage _cache;
        private readonly IRepository<Data.Redirect> _baseRepo;
        public CachedRedirectRepository(SimpleRepository<Data.Redirect> baseRepo, ICachedRedirectRepositoryStorage cache)
        {
            _cache = cache;
            _baseRepo = baseRepo;
        }

        public void Create(Data.Redirect entity)
        {
            _baseRepo.Create(entity);
            _cache.CachedRedirects.Add(entity);
        }

        public void Delete(Data.Redirect entity)
        {
            _baseRepo.Delete(entity);
            InternalRemove(entity);
        }

        private void InternalRemove(Data.Redirect entity)
        {
            if (_cache.CachedRedirects.Any(b => b.Id == entity.Id))
            {
                _cache.CachedRedirects.Remove(_cache.CachedRedirects.Single(b => b.Id == entity.Id));
            }
        }

        public IQueryable<Data.Redirect> GetAll()
        {
            return _cache.CachedRedirects.AsQueryable();
        }

        public Data.Redirect GetBy(Expression<Func<Data.Redirect, bool>> predicate, params Expression<Func<Data.Redirect, object>>[] includes)
        {
            return GetAll().SingleOrDefault(predicate.Compile());
        }

        public void Update(Data.Redirect entity)
        {
            _baseRepo.Update(entity);
            InternalRemove(entity);
            _cache.CachedRedirects.Add(entity);
        }

        public void Dispose()
        {
        }
    }
}
