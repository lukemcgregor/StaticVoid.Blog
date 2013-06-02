namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostsToBlogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "BlogId", c => c.Int());

            Sql("UPDATE dbo.Posts SET BlogId=1 WHERE BlogId is null");

            AlterColumn("dbo.Posts", "BlogId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Posts", "BlogId", "dbo.Blogs", "Id", cascadeDelete: true);
            CreateIndex("dbo.Posts", "BlogId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Posts", new[] { "BlogId" });
            DropForeignKey("dbo.Posts", "BlogId", "dbo.Blogs");
            DropColumn("dbo.Posts", "BlogId");
        }
    }
}
