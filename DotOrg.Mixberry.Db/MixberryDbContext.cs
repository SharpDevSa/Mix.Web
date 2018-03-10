using System.Data.Entity;
using DotOrg.Db.Mappings;
using DotOrg.Mixberry.Db.Mappings;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;
//using EntityFramework.Filters;

namespace DotOrg.Mixberry.Db
{
    public class MixberryDbContext : DbContext
    {
		public MixberryDbContext()
			: base("name=DefaultConnection")
		{
			Configuration.LazyLoadingEnabled = true;
			Configuration.ProxyCreationEnabled = false;
		}

	    protected override void OnModelCreating(DbModelBuilder modelBuilder)
	    {
			modelBuilder.Configurations.Add(new EmailLogMapping());
			modelBuilder.Configurations.Add(new FeedbackMapping());
			modelBuilder.Configurations.Add(new LocationMapping());
			modelBuilder.Configurations.Add(new NewsMapping());
			modelBuilder.Configurations.Add(new SiteSettingMapping());
		    modelBuilder.Configurations.Add(new WebFileMapping<WebFile>());
		    modelBuilder.Configurations.Add(new LocationBlockMapping());
		    modelBuilder.Configurations.Add(new CategoryMapping());
		    modelBuilder.Configurations.Add(new CategoryBlockMapping());
		    modelBuilder.Configurations.Add(new ProductMapping());
		    modelBuilder.Configurations.Add(new ProductModelMapping());
		    modelBuilder.Configurations.Add(new SubscriberMapping());
		    modelBuilder.Configurations.Add(new BlockTemplateMapping());
		    modelBuilder.Configurations.Add(new ProductGroupMapping());

			modelBuilder.Configurations.Add(new LanguageMapping());
			modelBuilder.Configurations.Add(new CountryMapping());
			modelBuilder.Configurations.Add(new PartnerMapping());
			modelBuilder.Configurations.Add(new LocalizationMapping());

			//modelBuilder.Entity<LocalizationObject>()
			//	.Map<LocationLocalizations>(m => m.Requires("LocalizationType").HasValue("Location"))
			//	.Map<NewsLocalizations>(m => m.Requires("LocalizationType").HasValue("News"))
			//	.Map<CategoryLocalizations>(m => m.Requires("LocalizationType").HasValue("Category"))
			//	;

			modelBuilder.Conventions.Add(EntityFramework.Filters.FilterConvention.Create<News, string>(FilterNames.News, (e, lang) => e.Lang == lang));
			modelBuilder.Conventions.Add(EntityFramework.Filters.FilterConvention.Create<Category, string>(FilterNames.Category, (e, lang) => e.Lang == lang));
			modelBuilder.Conventions.Add(EntityFramework.Filters.FilterConvention.Create<Location, string>(FilterNames.Location, (e, lang) => e.Lang == lang));
	    }
    }
}
