using System.Data.Entity;

namespace DotOrg.Db
{
	public interface IDbFactory<out TDbContext> where TDbContext : IDbContext
	{
		TDbContext Get();
		TDbContext Create();
	}
}