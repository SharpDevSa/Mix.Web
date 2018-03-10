using System.Data.Entity;
using System.Linq;
using DotOrg.Db.Entities;

namespace DotOrg.Web.Ozi.Repositories
{
	public class GenericRepository<TEntity, TDbContext> : IRepository<TEntity, TDbContext>
        where TEntity : class, IEntity
		where TDbContext : DbContext, new()
    {

		private TDbContext _dataContext;
		public TDbContext DataContext
        {
            get
            {
	            if (_dataContext == null)
	            {
		            _dataContext = new TDbContext();
		            _dataContext.Configuration.LazyLoadingEnabled = true;
		            _dataContext.Configuration.ProxyCreationEnabled = true;
	            }
				return _dataContext;
            }
        }

        public TEntity GetByPrimaryKey(int id)
        {
	        return DataContext.Set<TEntity>().FirstOrDefault(entity => entity.Id == id);
        }

		//public PropertyInfo GetPrimaryKeyInfo()
		//{
		//	var properties = typeof(TEntity).GetProperties();
		//	foreach (var pI in properties)
		//	{
		//		var attributes = pI.GetCustomAttributes(true);
		//		foreach (var attribute in attributes)
		//		{
		//			if (attribute is EdmScalarPropertyAttribute)
		//			{
		//				if ((attribute as EdmScalarPropertyAttribute).EntityKeyProperty)
		//					return pI;
		//			}
		//			else if (attribute is ColumnAttribute)
		//			{

		//				if ((attribute as ColumnAttribute).IsPrimaryKey)
		//					return pI;
		//			}
		//		}
		//	}
		//	return null;
		//}

		public IQueryable<TEntity> All()
        {
			return DataContext.Set<TEntity>();
        }

		protected TEntity SetLang(TEntity entity)
		{
			return entity;
		}

		public void AddObject(TEntity entity)
        {
			//DataContext.Set<TEntity>().Add(SetLang(entity));
			DataContext.Set<TEntity>().Add(entity);
        }
        
        public void Save()
        {
			DataContext.SaveChanges();
        }


        public void Delete(TEntity entity)
        {
            DataContext.Set<TEntity>().Remove(entity);
            Save();
        }
    }
}