namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteLocalizationLogic : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects");
            DropIndex("dbo.Locations", new[] { "LocalizationId" });
            DropIndex("dbo.News", new[] { "LocalizationId" });
            DropIndex("dbo.Categories", new[] { "LocalizationId" });
            AlterColumn("dbo.Locations", "LocalizationId", c => c.Int(nullable: false));
            AlterColumn("dbo.News", "LocalizationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Categories", "LocalizationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Locations", "LocalizationId");
            CreateIndex("dbo.News", "LocalizationId");
            CreateIndex("dbo.Categories", "LocalizationId");
            AddForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects");
            DropIndex("dbo.Categories", new[] { "LocalizationId" });
            DropIndex("dbo.News", new[] { "LocalizationId" });
            DropIndex("dbo.Locations", new[] { "LocalizationId" });
            AlterColumn("dbo.Categories", "LocalizationId", c => c.Int());
            AlterColumn("dbo.News", "LocalizationId", c => c.Int());
            AlterColumn("dbo.Locations", "LocalizationId", c => c.Int());
            CreateIndex("dbo.Categories", "LocalizationId");
            CreateIndex("dbo.News", "LocalizationId");
            CreateIndex("dbo.Locations", "LocalizationId");
            AddForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects", "Id");
            AddForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects", "Id");
            AddForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects", "Id");
        }
    }
}
