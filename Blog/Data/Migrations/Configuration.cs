namespace StaticVoid.Blog.Data.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<StaticVoid.Blog.Data.BlogContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(StaticVoid.Blog.Data.BlogContext context)
		{
			
		}
	}
}
