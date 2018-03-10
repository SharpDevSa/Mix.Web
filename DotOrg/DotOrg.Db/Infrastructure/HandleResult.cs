using System.Collections.Generic;

namespace DotOrg.Db.Infrastructure
{
	public class HandleResult
	{
		public bool Success { get; set; }
		public long? Id { get; set; }
		public IEnumerable<string> Errors { get; set; }
        public object Result { get; set; }

		public HandleResult() : this(false)
		{
		}

		public HandleResult(bool success)
		{
			Success = success;
			Errors = new string[0];
		}
	}
}