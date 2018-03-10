namespace DotOrg.Db.Entities
{
    public interface IVisibleEntity : Db.Entities.IEntity
    {
        bool Visibility { get; set; }
    }
}
