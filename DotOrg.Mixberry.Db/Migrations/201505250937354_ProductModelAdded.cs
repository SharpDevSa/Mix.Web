namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductModelAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductWebFiles", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropIndex("dbo.ProductWebFiles", new[] { "Product_Id" });
            DropIndex("dbo.ProductWebFiles", new[] { "WebFile_Id" });
            CreateTable(
                "dbo.ProductModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                        Color = c.String(),
                        ImageId = c.Int(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebFiles", t => t.ImageId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ImageId)
                .Index(t => t.ProductId);
            
            DropTable("dbo.ProductWebFiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductWebFiles",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.WebFile_Id });
            
            DropForeignKey("dbo.ProductModels", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductModels", "ImageId", "dbo.WebFiles");
            DropIndex("dbo.ProductModels", new[] { "ProductId" });
            DropIndex("dbo.ProductModels", new[] { "ImageId" });
            DropTable("dbo.ProductModels");
            CreateIndex("dbo.ProductWebFiles", "WebFile_Id");
            CreateIndex("dbo.ProductWebFiles", "Product_Id");
            AddForeignKey("dbo.ProductWebFiles", "WebFile_Id", "dbo.WebFiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductWebFiles", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
