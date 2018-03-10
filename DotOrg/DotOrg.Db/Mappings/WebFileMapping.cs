using System.Data.Entity.ModelConfiguration;
using DotOrg.Db.Entities;

namespace DotOrg.Db.Mappings
{
	public class WebFileMapping<TEntity> : EntityTypeConfiguration<TEntity> where TEntity: class, IFileEntity
	{
		public WebFileMapping()
		{
			HasKey(x => x.Id);
		}
	}
}
