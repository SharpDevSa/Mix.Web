namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StructureChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropForeignKey("dbo.ProductBlockWebFiles", "ProductBlock_Id", "dbo.ProductBlocks");
            DropForeignKey("dbo.ProductBlockWebFiles", "WebFile_Id", "dbo.WebFiles");
            DropForeignKey("dbo.ProductBlocks", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.ProductBlocks", new[] { "ProductId" });
            DropIndex("dbo.ProductBlockWebFiles", new[] { "ProductBlock_Id" });
            DropIndex("dbo.ProductBlockWebFiles", new[] { "WebFile_Id" });
            CreateTable(
                "dbo.CategoryProducts",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Product_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Product_Id);
            
            DropColumn("dbo.Categories", "ParentId");
            DropTable("dbo.ProductBlocks");
            DropTable("dbo.ProductBlockWebFiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductBlockWebFiles",
                c => new
                    {
                        ProductBlock_Id = c.Int(nullable: false),
                        WebFile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductBlock_Id, t.WebFile_Id });
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Categories", "ParentId", c => c.Int());
            DropForeignKey("dbo.CategoryProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.CategoryProducts", "Category_Id", "dbo.Categories");
            DropIndex("dbo.CategoryProducts", new[] { "Product_Id" });
            DropIndex("dbo.CategoryProducts", new[] { "Category_Id" });
            DropTable("dbo.CategoryProducts");
            CreateIndex("dbo.ProductBlockWebFiles", "WebFile_Id");
            CreateIndex("dbo.ProductBlockWebFiles", "ProductBlock_Id");
            CreateIndex("dbo.ProductBlocks", "ProductId");
            CreateIndex("dbo.Products", "CategoryId");
            CreateIndex("dbo.Categories", "ParentId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductBlocks", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductBlockWebFiles", "WebFile_Id", "dbo.WebFiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductBlockWebFiles", "ProductBlock_Id", "dbo.ProductBlocks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Categories", "ParentId", "dbo.Categories", "Id");
        }
    }
}
