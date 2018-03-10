using System.Collections.Generic;

namespace DotOrg.Db.Infrastructure
{
	public class HandleCommandResult
	{
		public HandleCommandResult()
		{
			HandleResults = new List<HandleResult>();
			ValidateResults = new List<ValidateResult>();
		}
		public IEnumerable<HandleResult> HandleResults { get; set; }
		public IEnumerable<ValidateResult> ValidateResults { get; set; }
	}
}