using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class LocationBlockMapping : EntityTypeConfiguration<LocationBlock>
	{
		public LocationBlockMapping()
		{
			HasKey(x => x.Id);
			HasRequired(x => x.Location).WithMany(x => x.Blocks).HasForeignKey(x => x.LocationId);
			HasMany(x => x.Images).WithMany();
		}
		
	}
}