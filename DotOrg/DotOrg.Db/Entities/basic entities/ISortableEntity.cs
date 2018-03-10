namespace DotOrg.Db.Entities
{
    public interface ISortableEntity : IEntity
    {
        int Sort { get; set; }
    }
}
