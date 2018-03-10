namespace DotOrg.Db.Entities
{
	public interface ISelfNestedEntity<TEntity> : INestedEntity<TEntity> where TEntity : class, IEntity
	{
		TEntity Parent { get; set; }
	}
}