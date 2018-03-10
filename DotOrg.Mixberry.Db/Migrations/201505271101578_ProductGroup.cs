namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.Int(nullable: false),
                        Name = c.String(),
                        Alias = c.String(),
                        Description = c.String(),
                        Visibility = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ProductGroupProducts",
                c => new
                    {
                        ProductGroup_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductGroup_Id, t.Product_Id })
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.ProductGroup_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductGroups", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ProductGroupProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductGroupProducts", "ProductGroup_Id", "dbo.ProductGroups");
            DropIndex("dbo.ProductGroupProducts", new[] { "Product_Id" });
            DropIndex("dbo.ProductGroupProducts", new[] { "ProductGroup_Id" });
            DropIndex("dbo.ProductGroups", new[] { "CategoryId" });
            DropTable("dbo.ProductGroupProducts");
            DropTable("dbo.ProductGroups");
        }
    }
}
