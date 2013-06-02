namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_IsAuthor : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "IsAuthor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "IsAuthor", c => c.Boolean(nullable: false));
        }
    }
}
