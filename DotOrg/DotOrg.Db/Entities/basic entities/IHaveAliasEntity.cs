namespace DotOrg.Db.Entities
{
	public interface IHaveAliasEntity : IEntity
	{
		string Name { get; set; }
		string Alias { get; set; }
	}
}
