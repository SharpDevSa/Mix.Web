using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Db.Infrastructure
{
	public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
	{
		public virtual int Priority { get { return 0; } }
		public abstract HandleResult Execute(TCommand command);
	}
}
