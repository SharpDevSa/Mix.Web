namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductGroupsDbChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductGroupProducts", newName: "ProductProductGroups");
            DropPrimaryKey("dbo.ProductProductGroups");
            AddPrimaryKey("dbo.ProductProductGroups", new[] { "Product_Id", "ProductGroup_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProductProductGroups");
            AddPrimaryKey("dbo.ProductProductGroups", new[] { "ProductGroup_Id", "Product_Id" });
            RenameTable(name: "dbo.ProductProductGroups", newName: "ProductGroupProducts");
        }
    }
}
