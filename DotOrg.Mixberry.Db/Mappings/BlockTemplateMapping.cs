using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class BlockTemplateMapping : EntityTypeConfiguration<BlockTemplate>
	{
		public BlockTemplateMapping()
		{
			HasKey(x => x.Id);
		}
	}
}