using System;
using System.Linq;
using DotOrg.Db;
using DotOrg.Db.Models;

namespace DotOrg.Web.Services
{
	public class EmailLogsService : IEmailLogService
	{
		private readonly IDbFactory _factory;

		public EmailLogsService(IDbFactory factory)
		{
			_factory = factory;
		}

		public void Write(string @from, string to, string subject, string content, Exception exception)
		{
			using (var db = _factory.Create())
			{
				var bound = DateTime.Now.Subtract(TimeSpan.FromDays(7.0));
				var removeList = db.Set<EmailLog>().Where(entry => entry.Date < bound).ToList();
				foreach (var entry in removeList)
				{
					db.Set<EmailLog>().Remove(entry);
				}

				var log = new EmailLog
					{
						Content = content,
						Date = DateTime.Now,
						From = @from,
						Status = exception == null,
						To = to,
						Subject = subject
					};
				if (exception != null)
				{
					log.Type = exception.GetType().Name;
					log.Message = exception.Message;
					log.StackTrace = exception.StackTrace;
					if (exception.InnerException != null)
					{
						log.Type += " / " + exception.InnerException.GetType().Name;
						log.Message += " / " + exception.InnerException.Message;
						log.StackTrace += Environment.NewLine + "==================" + exception.InnerException.StackTrace;
					}
				}
				db.Set<EmailLog>().Add(log);
				db.SaveChanges();
			}
		}
	}
}
