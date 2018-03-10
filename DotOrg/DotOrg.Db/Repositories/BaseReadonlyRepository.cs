using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DotOrg.Db.Repositories
{
	public abstract class BaseReadonlyRepository<TEntity> : BaseReadonlyRepository<TEntity, int>, IReadonlyRepository<TEntity> where TEntity : class, new()
	{
		protected BaseReadonlyRepository(IDbContext db) : base(db)
		{
		}
	}

	public abstract class BaseReadonlyRepository<TEntity, TKey> : IReadonlyRepository<TEntity, TKey> where TEntity : class, new()
	{
		private readonly IDbContext _db;

		protected BaseReadonlyRepository(IDbContext db)
		{
			_db = db;
		}

		public virtual TEntity FindById(TKey id)
		{
			return _db.Set<TEntity>().Find(id);
		}

		public void Load(object entity)
		{
			if (_db.Entry(entity).State != EntityState.Detached)
			{
				_db.Entry(entity).Reload();
			}
		}

		public TEntity FindOne(Expression<Func<TEntity, bool>> expr)
		{
			return _db.Set<TEntity>().AsNoTracking().Where(expr).FirstOrDefault();
		}

        public virtual IEnumerable<TEntity> All()
        {
            return _db.Set<TEntity>().AsNoTracking().ToList();
        }

		public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expr)
		{
			return _db.Set<TEntity>().AsNoTracking().Where(expr);
		}

		protected IDbContext GetDb()
		{
			return _db;
		}
	}
}
