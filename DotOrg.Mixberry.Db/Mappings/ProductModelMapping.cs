using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class ProductModelMapping : EntityTypeConfiguration<ProductModel>
	{
		public ProductModelMapping()
		{
			HasKey(x => x.Id);
			HasOptional(x => x.Image).WithMany().HasForeignKey(x => x.ImageId);
			HasRequired(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
		}
	}
}