namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class catalog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Alias = c.String(),
                        Content = c.String(),
                        Sort = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Alias = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.CategoryBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Alias = c.String(),
                        Name = c.String(),
                        Sort = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Alias = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ProductBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Alias = c.String(),
                        Name = c.String(),
                        Sort = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.LocationBlockWebFiles",
                c => new
                    {
                        LocationBlock_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationBlock_Id, t.WebFile_Id })
                .ForeignKey("dbo.LocationBlocks", t => t.LocationBlock_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.LocationBlock_Id)
                .Index(t => t.WebFile_Id);
            
            CreateTable(
                "dbo.LocationWebFiles",
                c => new
                    {
                        Location_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location_Id, t.WebFile_Id })
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.Location_Id)
                .Index(t => t.WebFile_Id);
            
            CreateTable(
                "dbo.CategoryBlockWebFiles",
                c => new
                    {
                        CategoryBlock_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryBlock_Id, t.WebFile_Id })
                .ForeignKey("dbo.CategoryBlocks", t => t.CategoryBlock_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.CategoryBlock_Id)
                .Index(t => t.WebFile_Id);
            
            CreateTable(
                "dbo.CategoryWebFiles",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.WebFile_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.WebFile_Id);
            
            CreateTable(
                "dbo.ProductBlockWebFiles",
                c => new
                    {
                        ProductBlock_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductBlock_Id, t.WebFile_Id })
                .ForeignKey("dbo.ProductBlocks", t => t.ProductBlock_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebFiles", t => t.WebFile_Id, cascadeDelete: true)
                .Index(t => t.ProductBlock_Id)
                .Index(t => t.WebFile_Id);
            
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
            
            AddColumn("dbo.News", "BigImageId", c => c.Int());
            CreateIndex("dbo.News", "BigImageId");
            AddForeignKey("dbo.News", "BigImageId", "dbo.WebFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ProductWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.ProductWebFiles", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductBlocks", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductBlockWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.ProductBlockWebFiles", "ProductBlock_Id", "dbo.ProductBlocks");
            DropForeignKey("dbo.CategoryWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.CategoryWebFiles", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropForeignKey("dbo.CategoryBlocks", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryBlockWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.CategoryBlockWebFiles", "CategoryBlock_Id", "dbo.CategoryBlocks");
            DropForeignKey("dbo.News", "BigImageId", "dbo.WebFiles");
            DropForeignKey("dbo.LocationWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.LocationWebFiles", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.LocationBlocks", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.LocationBlockWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.LocationBlockWebFiles", "LocationBlock_Id", "dbo.LocationBlocks");
            DropIndex("dbo.ProductWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.ProductWebFiles", new[] { "Product_Id" });
            DropIndex("dbo.ProductBlockWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.ProductBlockWebFiles", new[] { "ProductBlock_Id" });
            DropIndex("dbo.CategoryWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.CategoryWebFiles", new[] { "Category_Id" });
            DropIndex("dbo.CategoryBlockWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.CategoryBlockWebFiles", new[] { "CategoryBlock_Id" });
            DropIndex("dbo.LocationWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.LocationWebFiles", new[] { "Location_Id" });
            DropIndex("dbo.LocationBlockWebFiles", new[] { "WebFile_Id" });
            DropIndex("dbo.LocationBlockWebFiles", new[] { "LocationBlock_Id" });
            DropIndex("dbo.ProductBlocks", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.CategoryBlocks", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropIndex("dbo.News", new[] { "BigImageId" });
            DropIndex("dbo.LocationBlocks", new[] { "LocationId" });
            DropColumn("dbo.News", "BigImageId");
            DropTable("dbo.ProductWebFiles");
            DropTable("dbo.ProductBlockWebFiles");
            DropTable("dbo.CategoryWebFiles");
            DropTable("dbo.CategoryBlockWebFiles");
            DropTable("dbo.LocationWebFiles");
            DropTable("dbo.LocationBlockWebFiles");
            DropTable("dbo.ProductBlocks");
            DropTable("dbo.Products");
            DropTable("dbo.CategoryBlocks");
            DropTable("dbo.Categories");
            DropTable("dbo.LocationBlocks");
        }
    }
}
