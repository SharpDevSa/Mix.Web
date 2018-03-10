using System.Data.Entity.ModelConfiguration;
using DotOrg.Db.Models;

namespace DotOrg.Db.Mappings
{
	public class FeedbackMapping : EntityTypeConfiguration<Feedback>
	{
		public FeedbackMapping()
		{
			HasKey(x => x.Id);
		}
	}
}