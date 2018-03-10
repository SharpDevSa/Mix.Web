namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        From = c.String(),
                        To = c.String(),
                        Subject = c.String(),
                        Type = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        AdminComment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.Int(nullable: false),
                        Name = c.String(),
                        Header = c.String(),
                        Content = c.String(),
                        Alias = c.String(),
                        Visibility = c.Boolean(nullable: false),
                        ShowInMenu = c.Boolean(nullable: false),
                        MetaDescription = c.String(),
                        MetaKeywords = c.String(),
                        MetaTitle = c.String(),
                        MetaData = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.Int(nullable: false),
                        Name = c.String(),
                        Content = c.String(),
                        Date = c.DateTime(nullable: false),
                        Alias = c.String(),
                        Visibility = c.Boolean(nullable: false),
                        MetaDescription = c.String(),
                        MetaKeywords = c.String(),
                        MetaTitle = c.String(),
                        MetaData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteSettings",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Input = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.WebFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Visibility = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                        Url = c.String(),
                        SourceName = c.String(),
                        Alt = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        Size = c.Long(),
                        Class = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "ParentId", "dbo.Locations");
            DropIndex("dbo.Locations", new[] { "ParentId" });
            DropTable("dbo.WebFiles");
            DropTable("dbo.SiteSettings");
            DropTable("dbo.News");
            DropTable("dbo.Locations");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.EmailLogs");
        }
    }
}
