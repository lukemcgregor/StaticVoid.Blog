namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogTemplate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Blogs", "StyleId", "dbo.Styles");
            DropIndex("dbo.Blogs", new[] { "StyleId" });
            CreateTable(
                "dbo.BlogTemplates",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TemplateMode = c.Int(nullable: false),
                        BlogId = c.Int(nullable: false),
                        Css = c.String(),
                        HtmlTemplate = c.String(),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
            AddColumn("dbo.Blogs", "BlogTemplateId", c => c.Guid());
            AddForeignKey("dbo.Blogs", "BlogTemplateId", "dbo.BlogTemplates", "Id");
            CreateIndex("dbo.Blogs", "BlogTemplateId");
            DropColumn("dbo.Blogs", "StyleId");
            DropTable("dbo.Styles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Styles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Css = c.String(),
                        Template = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Blogs", "StyleId", c => c.Guid());
            DropIndex("dbo.BlogTemplates", new[] { "BlogId" });
            DropIndex("dbo.Blogs", new[] { "BlogTemplateId" });
            DropForeignKey("dbo.BlogTemplates", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Blogs", "BlogTemplateId", "dbo.BlogTemplates");
            DropColumn("dbo.Blogs", "BlogTemplateId");
            DropTable("dbo.BlogTemplates");
            CreateIndex("dbo.Blogs", "StyleId");
            AddForeignKey("dbo.Blogs", "StyleId", "dbo.Styles", "Id");
        }
    }
}
