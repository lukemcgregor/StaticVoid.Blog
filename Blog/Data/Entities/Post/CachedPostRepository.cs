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
    public class CachedPostRepositoryStorage : ICachedPostRepositoryStorage
    {
        public CachedPostRepositoryStorage(SimpleRepository<Data.Post> baseRepo, IRepository<Data.User> userRepo)
        {
            CachedPosts = baseRepo.GetAll().AsNoTracking().ToList();
            CachedPosts.ForEach((p) => { p.Author = userRepo.GetBy(a => a.Id == p.AuthorId); });
        }

        public List<Data.Post> CachedPosts { get; private set; }
    }

    public interface ICachedPostRepositoryStorage
    {
        List<Data.Post> CachedPosts { get; }
    }

    public class CachedPostRepository : IRepository<Data.Post>
    {
        private ICachedPostRepositoryStorage _cache;
        private readonly IRepository<Data.Post> _baseRepo;
        private readonly IRepository<Data.User> _userRepo;

        public CachedPostRepository(SimpleRepository<Data.Post> baseRepo, ICachedPostRepositoryStorage cache, IRepository<Data.User> userRepo)
        {
            _cache = cache;
            _baseRepo = baseRepo;
            _userRepo = userRepo;
        }

        public void Create(Data.Post entity)
        {
            _baseRepo.Create(entity);
            entity.Author = _userRepo.GetBy(u => u.Id == entity.AuthorId);
            _cache.CachedPosts.Add(entity);
        }

        public void Delete(Data.Post entity)
        {
            _baseRepo.Delete(entity);
            InternalRemove(entity);
        }

        private void InternalRemove(Data.Post entity)
        {
            if (_cache.CachedPosts.Any(b => b.Id == entity.Id))
            {
                _cache.CachedPosts.Remove(_cache.CachedPosts.Single(b => b.Id == entity.Id));
            }
        }

        public IQueryable<Data.Post> GetAll()
        {
            return _cache.CachedPosts.AsQueryable();
        }

        public Data.Post GetBy(Expression<Func<Data.Post, bool>> predicate, params Expression<Func<Data.Post, object>>[] includes)
        {
            return GetAll().SingleOrDefault(predicate.Compile());
        }

        public void Update(Data.Post entity)
        {
            _baseRepo.Update(entity);
            InternalRemove(entity);
            entity.Author = _userRepo.GetBy(u => u.Id == entity.AuthorId);
            _cache.CachedPosts.Add(entity);
        }

        public void Dispose()
        {
        }
    }
}
