namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class PrepareLocalizationLogic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocalizationObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(maxLength: 2),
                        LocalizationType = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.Lang)
                .Index(t => t.Lang);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Lang = c.String(nullable: false, maxLength: 2),
                        Name = c.String(),
                        Prefix = c.String(),
                        StaticLocalizationFile = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        Primary = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Lang);
            
            AlterTableAnnotations(
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
                        LocalizationId = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "globalFilter_LocationFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.Filters.FilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
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
                        ImageId = c.Int(),
                        BigImageId = c.Int(),
                        LocalizationId = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "globalFilter_NewsFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.Filters.FilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Alias = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                        MetaDescription = c.String(),
                        MetaKeywords = c.String(),
                        MetaTitle = c.String(),
                        MetaData = c.String(),
                        ShowInMenu = c.Boolean(nullable: false),
                        LocalizationId = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "globalFilter_CategoryFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.Filters.FilterDefinition")
                    },
                });
            
            AddColumn("dbo.Locations", "LocalizationId", c => c.Int());
            AddColumn("dbo.News", "LocalizationId", c => c.Int());
            AddColumn("dbo.Categories", "LocalizationId", c => c.Int());
            CreateIndex("dbo.Locations", "LocalizationId");
            CreateIndex("dbo.News", "LocalizationId");
            CreateIndex("dbo.Categories", "LocalizationId");
            AddForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects", "Id");
            AddForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects", "Id");
            AddForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocalizationObjects", "Lang", "dbo.Languages");
            DropForeignKey("dbo.Categories", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.News", "LocalizationId", "dbo.LocalizationObjects");
            DropForeignKey("dbo.Locations", "LocalizationId", "dbo.LocalizationObjects");
            DropIndex("dbo.Categories", new[] { "LocalizationId" });
            DropIndex("dbo.News", new[] { "LocalizationId" });
            DropIndex("dbo.LocalizationObjects", new[] { "Lang" });
            DropIndex("dbo.Locations", new[] { "LocalizationId" });
            DropColumn("dbo.Categories", "LocalizationId");
            DropColumn("dbo.News", "LocalizationId");
            DropColumn("dbo.Locations", "LocalizationId");
            AlterTableAnnotations(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Alias = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                        MetaDescription = c.String(),
                        MetaKeywords = c.String(),
                        MetaTitle = c.String(),
                        MetaData = c.String(),
                        ShowInMenu = c.Boolean(nullable: false),
                        LocalizationId = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "globalFilter_CategoryFilter",
                        new AnnotationValues(oldValue: "EntityFramework.Filters.FilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
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
                        ImageId = c.Int(),
                        BigImageId = c.Int(),
                        LocalizationId = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "globalFilter_NewsFilter",
                        new AnnotationValues(oldValue: "EntityFramework.Filters.FilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
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
                        LocalizationId = c.Int(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "globalFilter_LocationFilter",
                        new AnnotationValues(oldValue: "EntityFramework.Filters.FilterDefinition", newValue: null)
                    },
                });
            
            DropTable("dbo.Languages");
            DropTable("dbo.LocalizationObjects");
        }
    }
}
