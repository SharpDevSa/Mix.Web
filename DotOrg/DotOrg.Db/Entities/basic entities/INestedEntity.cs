using System.Collections.Generic;

namespace DotOrg.Db.Entities
{
    public interface INestedEntity<TEntity> : IPlainTreeItem
        where TEntity : class, IEntity
    {
        ICollection<TEntity> Children { get; set; }
    }
}
