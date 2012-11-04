using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using StaticVoid.Core.Repository;

namespace StaticVoid.Blog.Data
{
	public class PersistanceModule : NinjectModule
	{
		public override void Load()
		{
			Bind(typeof(IRepositoryDataSource<>)).To(typeof(DbContextRepositoryDataSource<>));
			Bind(typeof(IRepository<>)).To(typeof(SimpleRepository<>));
		}
	}
}
