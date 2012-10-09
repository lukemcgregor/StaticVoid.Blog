using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using StaticVoid.Blog.Data.Migrations;

namespace StaticVoid.Blog.Data
{
	public class BlogContext : DbContext
	{
		public BlogContext() : base("StaticVoid.Blog") { }

		public static void ConfigureInitializer()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogContext, Configuration>());
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
