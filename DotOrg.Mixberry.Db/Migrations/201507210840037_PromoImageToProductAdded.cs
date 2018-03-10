namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoImageToProductAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PromoImageId", c => c.Int());
            CreateIndex("dbo.Products", "PromoImageId");
            AddForeignKey("dbo.Products", "PromoImageId", "dbo.WebFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "PromoImageId", "dbo.WebFiles");
            DropIndex("dbo.Products", new[] { "PromoImageId" });
            DropColumn("dbo.Products", "PromoImageId");
        }
    }
}
