using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class ProductMapping : EntityTypeConfiguration<Product>
	{
		public ProductMapping()
		{
			HasKey(x => x.Id);
			HasMany(x => x.Categories).WithMany(x => x.Products);
			HasMany(x => x.Groups).WithMany(x => x.Products);
			HasMany(x => x.Models).WithRequired(x => x.Product).HasForeignKey(x => x.ProductId);

			HasOptional(x => x.Image).WithMany().HasForeignKey(x => x.ImageId);
			HasOptional(x => x.PromoImage).WithMany().HasForeignKey(x => x.PromoImageId);

			HasMany(x => x.Banners).WithMany();
			HasMany(x => x.DetailsImages).WithMany();
			HasMany(x => x.Related).WithMany();
			HasMany(x => x.Blocks).WithRequired(x => x.Product).HasForeignKey(x => x.ProductId);

			Ignore(x => x.SpecImage);
			Ignore(x => x.ModelImage);
		}
	}
}