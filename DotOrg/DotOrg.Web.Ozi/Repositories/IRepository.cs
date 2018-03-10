using System.Data.Entity;
using System.Linq;
using DotOrg.Db.Entities;

namespace DotOrg.Web.Ozi.Repositories
{
	public interface IRepository<TEntity, out TDbContext>
		where TDbContext : DbContext, new()
		where TEntity : class, IEntity
	{
		IQueryable<TEntity> All();
		TEntity GetByPrimaryKey(int id);
		TDbContext DataContext
		{
			get;
		}
		//PropertyInfo GetPrimaryKeyInfo();
		void AddObject(TEntity entity);
		void Save();
		void Delete(TEntity entity);
		//IQueryable<TEntity> AllByForeignKey(string foreignKeyName, object foreignKeyValue);
		//TEntity GetByAlias(string alias);
	}
}
