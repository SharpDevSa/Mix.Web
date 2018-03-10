namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsImageAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "ImageId", c => c.Int());
            CreateIndex("dbo.News", "ImageId");
            AddForeignKey("dbo.News", "ImageId", "dbo.WebFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "ImageId", "dbo.WebFiles");
            DropIndex("dbo.News", new[] { "ImageId" });
            DropColumn("dbo.News", "ImageId");
        }
    }
}
