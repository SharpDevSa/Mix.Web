namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTitleCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Title");
        }
    }
}
