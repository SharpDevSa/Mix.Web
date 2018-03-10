using System;

namespace DotOrg.Db.Exceptions
{
     public class ValidationHandlerNotFoundException : Exception
    {
         public ValidationHandlerNotFoundException(Type type)
            : base(string.Format("Не найден валидатор комманды для типа: {0}", type))
        {
        }
    }
}
