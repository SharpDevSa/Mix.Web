using System.Data.Entity;
using System.Diagnostics;

namespace DotOrg.Db.Infrastructure.Implementation
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDbFactory _databaseFactory;
		private DbContext _dataContext;

		[DebuggerStepThrough]
		public UnitOfWork(IDbFactory databaseFactory)
		{
			_databaseFactory = databaseFactory;
		}

		protected DbContext DataContext
		{
			get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); }
		}

		public void Commit()
		{
			DataContext.SaveChanges();
		}
	}
}
