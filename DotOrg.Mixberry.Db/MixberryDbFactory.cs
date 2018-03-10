using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DotOrg.Db;

namespace DotOrg.Mixberry.Db
{
	public class MixberryDbFactory: IDbFactory
	{
		public DbContext Get()
		{
			return DependencyResolver.Current.GetService<DbContext>();
		}

		public DbContext Create()
		{
			return new MixberryDbContext();
		}
	}
}
