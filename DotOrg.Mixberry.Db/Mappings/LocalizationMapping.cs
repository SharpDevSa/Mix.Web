using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class LocalizationMapping : EntityTypeConfiguration<LocalizationText>
	{
		public LocalizationMapping()
		{
			HasKey(x => x.Id);

			Property(x => x.Lang).HasMaxLength(2);
			Property(x => x.Key).HasMaxLength(128);
		}
	}
}