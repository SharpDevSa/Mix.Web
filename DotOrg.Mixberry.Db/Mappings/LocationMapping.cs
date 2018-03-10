using DotOrg.Db.Mappings;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class LocationMapping : LocationMapping<Location>
	{
		public LocationMapping()
		{
			HasMany(x => x.Blocks).WithRequired(x => x.Location).HasForeignKey(x => x.LocationId);
			HasMany(x => x.Images).WithMany();

			HasRequired(x => x.Language).WithMany().HasForeignKey(x => x.Lang);
		}
	}
}
