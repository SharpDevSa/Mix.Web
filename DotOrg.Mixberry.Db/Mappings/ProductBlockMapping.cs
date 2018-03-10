using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class ProductBlockMapping : EntityTypeConfiguration<ProductBlock>
	{
		public ProductBlockMapping()
		{
			HasKey(x => x.Id);
			HasRequired(x => x.Product).WithMany(x => x.Blocks).HasForeignKey(x => x.ProductId);
		}
	}
}