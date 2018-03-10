using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class LanguageMapping : EntityTypeConfiguration<Language>
	{
		public LanguageMapping()
		{
			HasKey(x => x.Lang);
			Property(x => x.Lang).HasMaxLength(2);
		}
	}
}