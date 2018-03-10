using System.Data.Entity.ModelConfiguration;
using DotOrg.Db.Models;

namespace DotOrg.Db.Mappings
{
	public class SiteSettingMapping : EntityTypeConfiguration<SiteSetting>
	{
		public SiteSettingMapping()
		{
			HasKey(x => x.Name);
		}
	}
}
