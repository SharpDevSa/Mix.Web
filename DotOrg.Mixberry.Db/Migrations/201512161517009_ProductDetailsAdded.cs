namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductDetailsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                        Name = c.String(),
                        Alias = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductWebFiles",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.WebFile_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.WebFile_Id);
            
            CreateTable(
                "dbo.ProductWebFile1",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.WebFile_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.WebFile_Id);
            
            CreateTable(
                "dbo.ProductProducts",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Product_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Product_Id1 })
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id1)
                .Index(t => t.Product_Id)
                .Index(t => t.Product_Id1);
            
            AddColumn("dbo.Products", "ShowDetails", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "DetailsTitle", c => c.String());
            AddColumn("dbo.Products", "DetailsSubTitle", c => c.String());
            AddColumn("dbo.Products", "DetailsDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductProducts", "Product_Id1", "dbo.Products");
            DropForeignKey("dbo.ProductProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductWebFile1", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.ProductWebFile1", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductBlocks", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.ProductWebFiles", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductProducts", new[] { "Product_Id1" });
            DropIndex("dbo.ProductProducts", new[] { "Product_Id" });
            DropIndex("dbo.ProductWebFile1", new[] { "WebFile_Id" });
            DropIndex("dbo.ProductWebFile1", new[] { "Product_Id" });
            DropIndex("dbo.ProductWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.ProductWebFiles", new[] { "Product_Id" });
            DropIndex("dbo.ProductBlocks", new[] { "ProductId" });
            DropColumn("dbo.Products", "DetailsDescription");
            DropColumn("dbo.Products", "DetailsSubTitle");
            DropColumn("dbo.Products", "DetailsTitle");
            DropColumn("dbo.Products", "ShowDetails");
            DropTable("dbo.ProductProducts");
            DropTable("dbo.ProductWebFile1");
            DropTable("dbo.ProductWebFiles");
            DropTable("dbo.ProductBlocks");
        }
    }
}
