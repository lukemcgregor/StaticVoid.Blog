using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Data
{
	public class PersistanceModule : NinjectModule
	{
		public override void Load()
		{
			Bind(typeof(IRepositoryDataSource<>)).To(typeof(DbContextRepositoryDataSource<>));
			Bind(typeof(IRepository<>)).To(typeof(SimpleRepository<>));

            Bind<ICachedBlogRepositoryStorage>().To<CachedBlogRepositoryStorage>().InSingletonScope();
            Rebind<IRepository<Data.Blog>>().To<CachedBlogRepository>();

            Bind<ICachedPostRepositoryStorage>().To<CachedPostRepositoryStorage>().InSingletonScope();
            Rebind<IRepository<Data.Post>>().To<CachedPostRepository>();

            Bind<ICachedUserRepositoryStorage>().To<CachedUserRepositoryStorage>().InSingletonScope();
            Rebind<IRepository<Data.User>>().To<CachedUserRepository>();

            Bind<ICachedRedirectRepositoryStorage>().To<CachedRedirectRepositoryStorage>().InSingletonScope();
            Rebind<IRepository<Data.Redirect>>().To<CachedRedirectRepository>();
		}
	}
}
