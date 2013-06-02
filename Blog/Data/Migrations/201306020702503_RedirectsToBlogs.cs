namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedirectsToBlogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Redirects", "BlogId", c => c.Int());

            Sql("UPDATE dbo.Redirects SET BlogId=1 WHERE BlogId is null");

            AlterColumn("dbo.Redirects", "BlogId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Redirects", "BlogId", "dbo.Blogs", "Id", cascadeDelete: true);
            CreateIndex("dbo.Redirects", "BlogId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Redirects", new[] { "BlogId" });
            DropForeignKey("dbo.Redirects", "BlogId", "dbo.Blogs");
            DropColumn("dbo.Redirects", "BlogId");
        }
    }
}
