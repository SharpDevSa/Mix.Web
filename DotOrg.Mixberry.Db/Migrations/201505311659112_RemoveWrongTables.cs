namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWrongTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.LocalizationObjects", "Lang", "dbo.Languages");
            DropIndex("dbo.Locations", new[] { "LocalizationId" });
            DropIndex("dbo.LocalizationObjects", new[] { "Lang" });
            DropIndex("dbo.News", new[] { "LocalizationId" });
            DropIndex("dbo.Categories", new[] { "LocalizationId" });
            AddColumn("dbo.Locations", "Lang", c => c.String(nullable: false, maxLength: 2, defaultValue: "tt"));
			AddColumn("dbo.News", "Lang", c => c.String(nullable: false, maxLength: 2, defaultValue: "tt"));
			AddColumn("dbo.Categories", "Lang", c => c.String(nullable: false, maxLength: 2, defaultValue: "tt"));
            CreateIndex("dbo.Locations", "Lang");
            CreateIndex("dbo.News", "Lang");
            CreateIndex("dbo.Categories", "Lang");
            AddForeignKey("dbo.Locations", "Lang", "dbo.Languages", "Lang", cascadeDelete: true);
            AddForeignKey("dbo.News", "Lang", "dbo.Languages", "Lang", cascadeDelete: true);
            AddForeignKey("dbo.Categories", "Lang", "dbo.Languages", "Lang", cascadeDelete: true);
            DropColumn("dbo.Locations", "LocalizationId");
            DropColumn("dbo.News", "LocalizationId");
            DropColumn("dbo.Categories", "LocalizationId");
            DropTable("dbo.LocalizationObjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LocalizationObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(maxLength: 2),
                        LocalizationType = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Categories", "LocalizationId", c => c.Int(nullable: false));
            AddColumn("dbo.News", "LocalizationId", c => c.Int(nullable: false));
            AddColumn("dbo.Locations", "LocalizationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Categories", "Lang", "dbo.Languages");
            DropForeignKey("dbo.News", "Lang", "dbo.Languages");
            DropForeignKey("dbo.Locations", "Lang", "dbo.Languages");
            DropIndex("dbo.Categories", new[] { "Lang" });
            DropIndex("dbo.News", new[] { "Lang" });
            DropIndex("dbo.Locations", new[] { "Lang" });
            DropColumn("dbo.Categories", "Lang");
            DropColumn("dbo.News", "Lang");
            DropColumn("dbo.Locations", "Lang");
            CreateIndex("dbo.Categories", "LocalizationId");
            CreateIndex("dbo.News", "LocalizationId");
            CreateIndex("dbo.LocalizationObjects", "Lang");
            CreateIndex("dbo.Locations", "LocalizationId");
            AddForeignKey("dbo.LocalizationObjects", "Lang", "dbo.Languages", "Lang");
            AddForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects", "Id", cascadeDelete: true);
        }
    }
}
