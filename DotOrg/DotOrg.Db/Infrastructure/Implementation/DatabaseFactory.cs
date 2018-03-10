using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DotOrg.Db.Infrastructure.Implementation
{
	public class DatabaseFactory : IDbFactory
	{
		public DbContext Get()
		{
			return DependencyResolver.Current.GetService<DbContext>();
		}

		public DbContext Create()
		{
			throw new NotImplementedException();
		}
	}
}
