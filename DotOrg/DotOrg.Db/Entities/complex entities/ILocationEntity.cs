using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Db.Entities
{
	public interface ILocationEntity<T> : IArticleEntity, ISelfNestedEntity<T>
		where T : class, ILocationEntity<T>
	{
	}
}
