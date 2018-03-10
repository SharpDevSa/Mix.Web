using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class PartnerMapping : EntityTypeConfiguration<Partner>
	{
		public PartnerMapping()
		{
			HasKey(x => x.Id);
			HasOptional(x => x.Logo).WithMany().HasForeignKey(x => x.LogoId);
			HasMany(x => x.Countries).WithMany();
		}
	}
}