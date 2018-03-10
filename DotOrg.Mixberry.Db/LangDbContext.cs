using System.Data.Entity;
using DotOrg.Mixberry.Db.Mappings;

namespace DotOrg.Mixberry.Db
{
	public class LangDbContext : DbContext
	{
		public LangDbContext():base("name=DefaultConnection")
		{
			Database.SetInitializer<LangDbContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new LanguageMapping());
		}
	}
}