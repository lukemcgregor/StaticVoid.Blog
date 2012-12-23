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
		public DbSet<Visit> Visits { get; set; }
        public DbSet<Redirect> Redirects { get; set; }
        public DbSet<Blog> Blogs { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Redirect>().Property(r => r.NewRoute).IsRequired();
            modelBuilder.Entity<Redirect>().Property(r => r.OldRoute).IsRequired();

            modelBuilder.Entity<Post>().HasRequired(r => r.Author).WithMany().HasForeignKey(p=>p.AuthorId);
            modelBuilder.Entity<Post>().Property(r => r.Status).IsRequired();
            modelBuilder.Entity<Post>().Property(r => r.Canonical).IsRequired();
            modelBuilder.Entity<Post>().Property(r => r.Path).IsRequired();
		}
	}
}
