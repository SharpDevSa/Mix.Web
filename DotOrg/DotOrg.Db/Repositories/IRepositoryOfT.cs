using System.Collections.Generic;
using System.Data.Entity;

namespace DotOrg.Db.Repositories
{
    public interface IRepository<out TEntity> : IRepository
    {
        IEnumerable<TEntity> All { get; }
        TEntity GetById(int id);
    }
}
