using System.Data.Entity.ModelConfiguration;
using DotOrg.Db.Models;

namespace DotOrg.Db.Mappings
{
	public class EmailLogMapping : EntityTypeConfiguration<EmailLog>
	{
		public EmailLogMapping()
		{
			HasKey(x => x.Id);
		}
	}
}
