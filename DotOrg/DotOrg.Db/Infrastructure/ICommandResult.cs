namespace DotOrg.Db.Infrastructure
{
    public interface ICommandResult
    {
    	object Result { get; }
        bool Success { get; }
    }
}

