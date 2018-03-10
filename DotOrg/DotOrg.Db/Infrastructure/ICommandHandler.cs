namespace DotOrg.Db.Infrastructure
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        int Priority { get; }
        HandleResult Execute(TCommand command);
    }
}

