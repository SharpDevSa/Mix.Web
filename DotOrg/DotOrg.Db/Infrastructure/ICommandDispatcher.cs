using System.Collections.Generic;

namespace DotOrg.Db.Infrastructure
{
    public interface ICommandDispatcher
    {
        HandleCommandResult Handle<TCommand>(TCommand command) where TCommand : class, ICommand;
        IEnumerable<HandleResult> Submit<TCommand>(TCommand command) where TCommand : class, ICommand;
        IEnumerable<ValidateResult> Validate<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}

