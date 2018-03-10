using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using DotOrg.Db.Entities;
using DotOrg.Db.Infrastructure;

namespace DotOrg.Db.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
    	private readonly IDbFactory _db;
		private DbContext _context;

		public DbContext Context
        {
            get
            {
            	return _context ?? (_context = _db.Get());
            }
        }

		[DebuggerStepThrough]
    	protected GenericRepository(IDbFactory db)
        {
        	_db = db;
        }

		private DbSet<TEntity> _sourceEntitySet;
		private IQueryable<TEntity> _entitySet;

	    protected bool IsLocalizable()
	    {
		    return typeof (ILocalizableEntity).IsAssignableFrom(typeof (TEntity));
	    }

	    protected DbSet<TEntity> SourceEntitySet
	    {
		    get { return _sourceEntitySet ?? (_sourceEntitySet = Context.Set<TEntity>()); }
	    }

	    protected IQueryable<TEntity> EntitySet
        {
            get
            {
	            return _entitySet ?? (_entitySet = SourceEntitySet);
            }
        }

        public IEnumerable<TEntity> All
        {
            get
            { 
                return EntitySet; 
            }
        }

	    public TEntity Create()
        {
			return Activator.CreateInstance<TEntity>();
        }

        public TEntity GetById(int id)
        {
            return EntitySet.SingleOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
			SourceEntitySet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            SourceEntitySet.Remove(entity);
        }

    	public void DeleteById(int id)
    	{
    		Delete(GetById(id));
    	}
    }
}
