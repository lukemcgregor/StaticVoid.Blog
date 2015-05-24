namespace StaticVoid.Blog.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class NewUserSystem : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.ProviderLogins",
				c => new
					{
						ProviderKey = c.String(nullable: false, maxLength: 128),
						Provider = c.Int(nullable: false),
						UserId = c.Int(nullable: false),
					})
				.PrimaryKey(t => new { t.ProviderKey, t.Provider })
				.ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
				.Index(t => t.UserId);

			Sql(
@"Insert into dbo.ProviderLogins (ProviderKey, Provider, UserId)
SELECT u.ClaimedIdentifier, 1, u.Id
FROM dbo.Users u");

			DropColumn("dbo.Users", "ClaimedIdentifier");
		}

		public override void Down()
		{
			AddColumn("dbo.Users", "ClaimedIdentifier", c => c.String());
			DropIndex("dbo.ProviderLogins", new[] { "UserId" });
			DropForeignKey("dbo.ProviderLogins", "UserId", "dbo.Users");
			DropTable("dbo.ProviderLogins");
		}
	}
}
