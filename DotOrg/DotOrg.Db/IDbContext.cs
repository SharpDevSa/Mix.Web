using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DotOrg.Db
{
	public interface IDbContext : IDisposable
	{
		IDbSet<TEntity> Set<TEntity>() where TEntity : class;
		void Commit();

		DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

		//void EnableFilter(string filterName);
		//void DisableFilter(string filterName);
	}
}
