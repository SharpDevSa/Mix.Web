using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class NewsMapping : EntityTypeConfiguration<News>
	{
		public NewsMapping()
		{
			HasKey(x => x.Id);
			HasOptional(x => x.Image).WithMany().HasForeignKey(x => x.ImageId);
			HasOptional(x => x.BigImage).WithMany().HasForeignKey(x => x.BigImageId);

			HasRequired(x => x.Language).WithMany().HasForeignKey(x => x.Lang);
		}
	}
}
