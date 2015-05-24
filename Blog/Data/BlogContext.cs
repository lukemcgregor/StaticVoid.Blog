using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using StaticVoid.Blog.Data.Migrations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaticVoid.Blog.Data
{
	public class BlogContext : DbContext, IBlogContext
	{
		public BlogContext() : base("StaticVoid.Blog") { }

		public static void ConfigureInitializer()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogContext, Configuration>());
		}

		public DbSet<User> Users { get; set; }
		public IDbSet<ProviderLogin> ProviderLogins { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<PostModification> PostModifications { get; set; }
		public DbSet<Redirect> Redirects { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<BlogTemplate> BlogTemplates { get; set; }
		public DbSet<Securable> Securables { get; set; }
		public DbSet<Invitation> Invitations { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Redirect>().Property(r => r.NewRoute).IsRequired();
			modelBuilder.Entity<Redirect>().Property(r => r.OldRoute).IsRequired();
			modelBuilder.Entity<Redirect>().HasRequired(r => r.Blog).WithMany().HasForeignKey(r => r.BlogId);

			modelBuilder.Entity<Post>().HasRequired(r => r.Author).WithMany().HasForeignKey(p => p.AuthorId);
			modelBuilder.Entity<Post>().Property(r => r.Status).IsRequired();
			modelBuilder.Entity<Post>().Property(r => r.Canonical).IsRequired();
			modelBuilder.Entity<Post>().Property(r => r.Path).IsRequired();
			modelBuilder.Entity<Post>().HasRequired(r => r.Blog).WithMany().HasForeignKey(p => p.BlogId);

			modelBuilder.Entity<Blog>().HasOptional(b => b.BlogTemplate).WithMany().HasForeignKey(b => b.BlogTemplateId);
			modelBuilder.Entity<Blog>().HasRequired(b => b.AuthorSecurable).WithMany().HasForeignKey(b => b.AuthorSecurableId).WillCascadeOnDelete(false);
			modelBuilder.Entity<Blog>().HasRequired(b => b.AdminSecurable).WithMany().HasForeignKey(b => b.AdminSecurableId).WillCascadeOnDelete(false);
			modelBuilder.Entity<Blog>().Property(b => b.AuthoritiveUrl).IsRequired();

			modelBuilder.Entity<User>().HasMany(u => u.Securables).WithMany(s => s.Members);

			modelBuilder.Entity<PostModification>().HasRequired(b => b.Post).WithMany().HasForeignKey(b => b.PostId);

			modelBuilder.Entity<BlogTemplate>().Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			modelBuilder.Entity<Invitation>().HasRequired(i => i.Securable).WithMany().HasForeignKey(i => i.SecurableId);
			modelBuilder.Entity<Invitation>().HasOptional(i => i.AssignedTo).WithMany().HasForeignKey(i => i.AssignedToId);
			modelBuilder.Configurations.Add(new ProviderLoginMappings());
		}
	}

	public interface IBlogContext
	{

		DbSet<User> Users { get;  }
		IDbSet<ProviderLogin> ProviderLogins { get; }
		DbSet<Post> Posts { get; set; }
		DbSet<PostModification> PostModifications { get; }
		DbSet<Redirect> Redirects { get; }
		DbSet<Blog> Blogs { get; }
		DbSet<BlogTemplate> BlogTemplates { get; }
		DbSet<Securable> Securables { get; }
		DbSet<Invitation> Invitations { get; }
		int SaveChanges();
	}
}
