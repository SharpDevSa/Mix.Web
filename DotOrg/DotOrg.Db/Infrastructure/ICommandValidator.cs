using System.Collections.Generic;

namespace DotOrg.Db.Infrastructure
{
	public interface ICommandValidator<in TCommand> where TCommand: class, ICommand
	{
		IEnumerable<ValidateResult> Validate(TCommand command);
		int Priority { get; }
	}
}