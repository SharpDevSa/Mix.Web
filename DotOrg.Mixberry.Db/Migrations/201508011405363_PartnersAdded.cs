namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartnersAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.Int(nullable: false),
                        Visibility = c.Boolean(nullable: false),
                        Name = c.String(),
                        Alias = c.String(),
                        LogoId = c.Int(),
                        Distributor = c.Boolean(nullable: false),
                        Retailer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebFiles", t => t.LogoId)
                .Index(t => t.LogoId);
            
            CreateTable(
                "dbo.LocalizationTexts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(maxLength: 2),
                        Key = c.String(maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartnerCountries",
                c => new
                    {
                        Partner_Id = c.Int(nullable: false),
                        Country_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Partner_Id, t.Country_Id })
                .ForeignKey("dbo.Partners", t => t.Partner_Id, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.Country_Id, cascadeDelete: true)
                .Index(t => t.Partner_Id)
                .Index(t => t.Country_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Partners", "LogoId", "dbo.WebFiles");
            DropForeignKey("dbo.PartnerCountries", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.PartnerCountries", "Partner_Id", "dbo.Partners");
            DropIndex("dbo.PartnerCountries", new[] { "Country_Id" });
            DropIndex("dbo.PartnerCountries", new[] { "Partner_Id" });
            DropIndex("dbo.Partners", new[] { "LogoId" });
            DropTable("dbo.PartnerCountries");
            DropTable("dbo.LocalizationTexts");
            DropTable("dbo.Partners");
            DropTable("dbo.Countries");
        }
    }
}
