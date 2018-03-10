using System.Collections.Generic;
using ValidationResult = DotOrg.Db.Infrastructure.ValidationResult;

namespace DotOrg.Db.Infrastructure
{
    public interface IValidationHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<ValidationResult>  Validate(TCommand command);
    }
}
