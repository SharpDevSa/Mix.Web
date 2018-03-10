using System;

namespace DotOrg.Db.Models
{
	public class EmailLog
	{
		public int Id { get; set; }
		public bool Status { get; set; }
		public DateTime Date { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Type { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public string Content { get; set; }
	}
}
