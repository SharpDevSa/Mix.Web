using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EntityFramework.Filters;

namespace DotOrg.Db
{
	public abstract class BaseDbContext<TDbContext> : DbContext, IDbContext where TDbContext : DbContext, IDbContext
	{
		static BaseDbContext()
		{
			Database.SetInitializer<TDbContext>(null);
		}

		protected BaseDbContext()
            : this("DefaultConnection")
		{

		}


		protected BaseDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return Set<TEntity>();
		}

		public void Commit()
		{
			SaveChanges();
		}

		//public void EnableFilter(string filterName)
		//{
		//	FilterExtensions.EnableFilter(this, filterName);
		//}

		//public void DisableFilter(string filterName)
		//{
		//	FilterExtensions.DisableFilter(this, filterName);
		//}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//DbInterception.Add(new FilterInterceptor());
		}
	}
}
