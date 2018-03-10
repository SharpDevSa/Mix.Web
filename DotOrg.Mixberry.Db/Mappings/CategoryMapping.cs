using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class CategoryMapping : EntityTypeConfiguration<Category>
	{
		public CategoryMapping()
		{
			HasKey(x => x.Id);
			HasMany(x => x.Blocks).WithRequired(x => x.Category).HasForeignKey(x => x.CategoryId);
			HasMany(x => x.Products).WithMany(x => x.Categories);
			HasMany(x => x.Images).WithMany();
			HasMany(x => x.Groups).WithRequired(x => x.Category).HasForeignKey(x => x.CategoryId);

			HasRequired(x => x.Language).WithMany().HasForeignKey(x => x.Lang);
		}
	}
}