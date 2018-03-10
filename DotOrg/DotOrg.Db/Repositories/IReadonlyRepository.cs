using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotOrg.Db.Repositories
{
	public interface IReadonlyRepository<TEntity> : IReadonlyRepository<TEntity, int> 
	{
		
	}

	public interface IReadonlyRepository<TEntity, in TKey> : IRepository
	{
		TEntity FindById(TKey id);
		void Load(object entity);
		TEntity FindOne(Expression<Func<TEntity, bool>> expr);
        IEnumerable<TEntity> All();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expr);

		//Task<TEntity> FindByIdAsync(int id);
		//Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> expr);
		//Task<IEnumerable<TEntity>> AllAsync();
		//Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expr); 
	}
}