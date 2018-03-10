namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductSpecs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Specification", c => c.String());
            AddColumn("dbo.Products", "ImageId", c => c.Int());
            CreateIndex("dbo.Products", "ImageId");
            AddForeignKey("dbo.Products", "ImageId", "dbo.WebFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ImageId", "dbo.WebFiles");
            DropIndex("dbo.Products", new[] { "ImageId" });
            DropColumn("dbo.Products", "ImageId");
            DropColumn("dbo.Products", "Specification");
        }
    }
}
