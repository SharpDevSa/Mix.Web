using System.Data.Entity.ModelConfiguration;
using DotOrg.Db.Entities;

namespace DotOrg.Db.Mappings
{
	public class LocationMapping<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : class, ILocationEntity<TEntity>
	{
		public LocationMapping()
		{
			HasKey(x => x.Id);

			HasOptional(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId);

			Ignore(x => x.Level);
			Ignore(x => x.HasChilds);
		}
	}
}
