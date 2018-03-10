using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace DotOrg.Db.Infrastructure.Implementation
{
    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        public HandleCommandResult Handle<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var result = new HandleCommandResult();
            result.ValidateResults = Validate(command).ToList();
            if (!result.ValidateResults.Any())
            {
                result.HandleResults = Submit(command);
            }
            return result;
        }

        public IEnumerable<HandleResult> Submit<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var handlers = DependencyResolver.Current.GetServices<ICommandHandler<TCommand>>();
            var commandHandlers = handlers as ICommandHandler<TCommand>[] ?? handlers.ToArray();
            if (handlers != null && commandHandlers.Any())
            {
                var orderedHandlers = commandHandlers.ToList().OrderBy(h => h.Priority);
                var list = new List<HandleResult>();
                foreach (var handler in orderedHandlers)
                {
                    var result = handler.Execute(command);
                    list.Add(result);
                }
                return list;
            }
            return new HandleResult[0];
        }

        public IEnumerable<ValidateResult> Validate<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var handlers = DependencyResolver.Current.GetServices<ICommandValidator<TCommand>>();
            var commandHandlers = handlers as ICommandValidator<TCommand>[] ?? handlers.ToArray();
            if (handlers != null && commandHandlers.Any())
            {
                var orderedHandlers = commandHandlers.ToList().OrderBy(h => h.Priority);
                var list = new List<ValidateResult>();
                foreach (var handler in orderedHandlers)
                {
                    var result = handler.Validate(command);
                    list.AddRange(result);
                }
                return list;
            }
            return new ValidateResult[0];
        }
    }
}

