using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class CountryMapping : EntityTypeConfiguration<Country>
	{
		public CountryMapping()
		{
			HasKey(x => x.Id);
		}
	}
}