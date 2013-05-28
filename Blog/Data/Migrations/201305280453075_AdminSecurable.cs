namespace StaticVoid.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminSecurable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SecurableId = c.Int(nullable: false),
                        Token = c.String(),
                        Email = c.String(),
                        InviteDate = c.DateTime(nullable: false),
                        AssignedToId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Securables", t => t.SecurableId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.AssignedToId)
                .Index(t => t.SecurableId)
                .Index(t => t.AssignedToId);

            AddColumn("dbo.Securables", "Name", c => c.String());

            DropForeignKey("dbo.Blogs", "AuthorSecurableId", "dbo.Securables");
            DropIndex("dbo.Blogs", new[] { "AuthorSecurableId" });
            AlterColumn("dbo.Blogs", "AuthorSecurableId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Blogs", "AuthorSecurableId", "dbo.Securables", "Id");
            CreateIndex("dbo.Blogs", "AuthorSecurableId");

            AddColumn("dbo.Blogs", "AdminSecurableId", c => c.Int());
            AlterColumn("dbo.Blogs", "AuthoritiveUrl", c => c.String(nullable: false));

            Sql(@"
declare @Temp table
(
  SecID int,
  BlogID int
);

merge Securables Temp
using (
      select 'Admin : '+B.Name as Name, B.Id
      from dbo.Blogs as B
      where B.AdminSecurableId is null
      ) as S
on 0 = 1
when not matched by target then
  insert (Name) values (S.Name)
output inserted.Id, S.Id
  into @Temp;

update dbo.Blogs
set AdminSecurableId = Temp.SecID
from @Temp as Temp
where Blogs.Id = Temp.BlogID;");

            AlterColumn("dbo.Blogs", "AdminSecurableId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Blogs", "AdminSecurableId", "dbo.Securables", "Id");
            CreateIndex("dbo.Blogs", "AdminSecurableId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Invitations", new[] { "AssignedToId" });
            DropIndex("dbo.Invitations", new[] { "SecurableId" });
            DropIndex("dbo.Blogs", new[] { "AdminSecurableId" });
            DropForeignKey("dbo.Invitations", "AssignedToId", "dbo.Users");
            DropForeignKey("dbo.Invitations", "SecurableId", "dbo.Securables");
            DropForeignKey("dbo.Blogs", "AdminSecurableId", "dbo.Securables");
            AlterColumn("dbo.Blogs", "AuthorSecurableId", c => c.Int());
            AlterColumn("dbo.Blogs", "AuthoritiveUrl", c => c.String());
            DropColumn("dbo.Blogs", "AdminSecurableId");
            DropColumn("dbo.Securables", "Name");
            DropTable("dbo.Invitations");
        }
    }
}
