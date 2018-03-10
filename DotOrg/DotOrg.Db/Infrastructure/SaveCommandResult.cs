using System.Collections;
using System.Collections.Generic;

namespace DotOrg.Db.Infrastructure
{
	public class SaveCommandResult
	{
		public SaveCommandResult()
		{
			Errors = new List<string>();
			Objects = new List<string>();
		}
		public long EntityId { get; set; }
		public IEnumerable<string> Errors { get; set; }
		public IEnumerable<object> Objects { get; set; }
	}
}