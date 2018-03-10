namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationPropertyChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocationBlocks", "Name", c => c.String());
            DropColumn("dbo.LocationBlocks", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LocationBlocks", "Title", c => c.String());
            DropColumn("dbo.LocationBlocks", "Name");
        }
    }
}
