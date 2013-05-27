namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StaticVoid.Blog.Data.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(StaticVoid.Blog.Data.BlogContext context)
        {
#if DEBUG
#if !DISABLE_SEED
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var admin = new User
            {
                Id = 1,
                ClaimedIdentifier = "https://www.google.com/accounts/o8/id?id=AItOawkczLfuxJhI-txNzlg53wsgWu2gdSALgVU",
                Email = "luke.mcgregor@gmail.com",
                FirstName = "Luke",
                LastName = "McGregor",
                IsAuthor = true
            };

            context.Users.AddOrUpdate(admin);

            var blogCreatorSecurable = new Securable { Name = "Blog Creator", Members = new List<User> { admin } };
            context.Securables.AddOrUpdate(blogCreatorSecurable);

            if (!context.Blogs.Any())
            {
                var blog = new Data.Blog
                {
                    AuthoritiveUrl = "http://localhost:34899",
                    Description = "",
                    BlogGuid = new Guid("fe9cde14-aa63-4022-a804-90041a6f3ad8"),
                    Id = 1,
                    Name = "StaticVoid - Test",
                    Twitter = "@staticv0id",
                    Style = new Style
                    {
                        Css =
@".test{
}"
                    },
                    AuthorSecurable = new Securable { Name = "Author : StaticVoid - Test" }
                };

                context.Blogs.AddOrUpdate(blog);
            }

            var postDate = new DateTime(2012, 10, 7, 12, 0, 0);

            context.Posts.AddOrUpdate(
                new Post
                {
                    Id = 1,
                    AuthorId = 1,
                    Title = "First Post",
                    Body = "First Post!",
                    Posted = postDate,
                    Status = PostStatus.Published,
                    Path = PostHelpers.MakeUrl(postDate.Year, postDate.Month, postDate.Day, "First Post"),
                    Canonical = "/" + PostHelpers.MakeUrl(postDate.Year, postDate.Month, postDate.Day, "First Post")
                });

            postDate = postDate.AddDays(1);
            context.Posts.AddOrUpdate(
                new Post
                {
                    Id = 2,
                    AuthorId = 1,
                    Title = "Second Post",
                    Body = "Second Post!",
                    Posted = postDate,
                    Status = PostStatus.Published,
                    Path = PostHelpers.MakeUrl(postDate.Year, postDate.Month, postDate.Day, "Second Post"),
                    Canonical = "/" + PostHelpers.MakeUrl(postDate.Year, postDate.Month, postDate.Day, "Second Post")
                });
#endif
#endif
        }
    }
}
