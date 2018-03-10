using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class ProductGroupMapping : EntityTypeConfiguration<ProductGroup>
	{
		public ProductGroupMapping()
		{
			HasKey(x => x.Id);

			HasRequired(x => x.Category).WithMany(x => x.Groups).HasForeignKey(x => x.CategoryId);
			HasMany(x => x.Products).WithMany(x => x.Groups);
		}
	}
}
