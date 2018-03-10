namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartnerUrlAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "Url");
        }
    }
}
