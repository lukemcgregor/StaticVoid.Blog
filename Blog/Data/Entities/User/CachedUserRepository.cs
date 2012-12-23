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
    public class CachedUserRepositoryStorage : ICachedUserRepositoryStorage
    {
        public CachedUserRepositoryStorage(SimpleRepository<Data.User> baseRepo)
        {
            CachedUsers = baseRepo.GetAll().AsNoTracking().ToList();
        }

        public List<Data.User> CachedUsers { get; private set; }
    }

    public interface ICachedUserRepositoryStorage
    {
        List<Data.User> CachedUsers { get; }
    }

    public class CachedUserRepository : IRepository<Data.User>
    {
        private ICachedUserRepositoryStorage _cache;
        private readonly IRepository<Data.User> _baseRepo;
        public CachedUserRepository(SimpleRepository<Data.User> baseRepo, ICachedUserRepositoryStorage cache)
        {
            _cache = cache;
            _baseRepo = baseRepo;
        }

        public void Create(Data.User entity)
        {
            _baseRepo.Create(entity);
            _cache.CachedUsers.Add(entity);
        }

        public void Delete(Data.User entity)
        {
            _baseRepo.Delete(entity);
            InternalRemove(entity);
        }

        private void InternalRemove(Data.User entity)
        {
            if (_cache.CachedUsers.Any(b => b.Id == entity.Id))
            {
                _cache.CachedUsers.Remove(_cache.CachedUsers.Single(b => b.Id == entity.Id));
            }
        }

        public IQueryable<Data.User> GetAll()
        {
            return _cache.CachedUsers.AsQueryable();
        }

        public Data.User GetBy(Expression<Func<Data.User, bool>> predicate, params Expression<Func<Data.User, object>>[] includes)
        {
            return GetAll().SingleOrDefault(predicate.Compile());
        }

        public void Update(Data.User entity)
        {
            _baseRepo.Update(entity);
            InternalRemove(entity);
            _cache.CachedUsers.Add(entity);
        }

        public void Dispose()
        {
        }
    }
}
