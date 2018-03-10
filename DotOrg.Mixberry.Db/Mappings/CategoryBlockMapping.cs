using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class CategoryBlockMapping : EntityTypeConfiguration<CategoryBlock>
	{
		public CategoryBlockMapping()
		{
			HasKey(x => x.Id);
			HasRequired(x => x.Category).WithMany(x => x.Blocks).HasForeignKey(x => x.CategoryId);
			HasMany(x => x.Images).WithMany();
		
		}
	}
}