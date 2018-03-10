namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CatalogMetaData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "MetaDescription", c => c.String());
            AddColumn("dbo.Categories", "MetaKeywords", c => c.String());
            AddColumn("dbo.Categories", "MetaTitle", c => c.String());
            AddColumn("dbo.Categories", "MetaData", c => c.String());
            AddColumn("dbo.Categories", "ShowInMenu", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "MetaDescription", c => c.String());
            AddColumn("dbo.Products", "MetaKeywords", c => c.String());
            AddColumn("dbo.Products", "MetaTitle", c => c.String());
            AddColumn("dbo.Products", "MetaData", c => c.String());
            AddColumn("dbo.Products", "ShowInMenu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ShowInMenu");
            DropColumn("dbo.Products", "MetaData");
            DropColumn("dbo.Products", "MetaTitle");
            DropColumn("dbo.Products", "MetaKeywords");
            DropColumn("dbo.Products", "MetaDescription");
            DropColumn("dbo.Categories", "ShowInMenu");
            DropColumn("dbo.Categories", "MetaData");
            DropColumn("dbo.Categories", "MetaTitle");
            DropColumn("dbo.Categories", "MetaKeywords");
            DropColumn("dbo.Categories", "MetaDescription");
        }
    }
}
