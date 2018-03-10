namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCategoryIdFromProduct : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
        }
    }
}
