namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingVisits : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Visits");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IpAddress = c.String(),
                        Browser = c.String(),
                        UserAgent = c.String(),
                        Url = c.String(),
                        Languages = c.String(),
                        Referrer = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        AuthenticatedUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
