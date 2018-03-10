using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Db.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<DotOrg.Mixberry.Db.MixberryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

		protected override void Seed(DotOrg.Mixberry.Db.MixberryDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

			//context.Set<Language>().AddOrUpdate(x => x.Lang,
			//	new Language{ Deleted = false, Lang = "tt", /*Prefix = "test", */Primary = true, StaticLocalizationFile = null}
			//);

			//context.Set<News>().ToList().ForEach(x => { if (x.LocalizationId == 0 || x.LocalizationId == null) x.Localization = new NewsLocalizations{ Lang = "tt" }; });
			//context.Set<Category>().ToList().ForEach(x => { if (x.LocalizationId == 0 || x.LocalizationId == null) x.Localization = new CategoryLocalizations { Lang = "tt" }; });
			//context.Set<Location>().ToList().ForEach(x => { if (x.LocalizationId == 0 || x.LocalizationId == null) x.Localization = new LocationLocalizations { Lang = "tt" }; });
        }
    }
}
