namespace StaticVoid.Blog.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class MissingIndex : DbMigration
	{

		public override void Up()
		{
			CreateIndex("dbo.Posts", "AuthorId");
		}

		public override void Down()
		{
			DropIndex("dbo.Posts", new[] { "AuthorId" });
		}
	}
}
