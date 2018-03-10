using System.Data.Entity.ModelConfiguration;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Db.Mappings
{
	public class SubscriberMapping: EntityTypeConfiguration<Subscriber>
	{
		public SubscriberMapping()
		{
			HasKey(x => x.Id);
			Property(x => x.Key).HasMaxLength(128);
		}
	}
}