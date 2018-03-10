namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnusedLangProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Languages", "Prefix");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Languages", "Prefix", c => c.String());
        }
    }
}
