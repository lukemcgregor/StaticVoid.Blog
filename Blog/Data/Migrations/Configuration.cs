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
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			var admin = new User
			{
				Id = 1,
				ClaimedIdentifier = "https://www.google.com/accounts/o8/id?id=AItOawlniRA6iVTSBAxw-dE10eUxy0Rgbek6u4A",
				Email = "luke.mcgregor@gmail.com",
				FirstName = "Luke",
				LastName = "McGregor",
				IsAuthor = true
			};

			context.Users.AddOrUpdate(admin);

			var postDate = new DateTime(2012, 10, 7, 12, 0, 0);

			context.Posts.AddOrUpdate(
				new Post
				{
					Id = 1,
					Author = admin,
					Title = "First Post",
					Body = "First Post!",
					Posted = postDate,
					Status = PostStatus.Published,
					Path = PostHelpers.MakeUrl(postDate.Year, postDate.Month, postDate.Day, "First Post")
				});

			postDate=postDate.AddDays(1);
			context.Posts.AddOrUpdate(
				new Post
				{
					Id = 2,
					Author = admin,
					Title = "Second Post",
					Body = "Second Post!",
					Posted = postDate,
					Status = PostStatus.Published,
					Path = PostHelpers.MakeUrl(postDate.Year, postDate.Month, postDate.Day, "Second Post")
				});
		}
	}
}
