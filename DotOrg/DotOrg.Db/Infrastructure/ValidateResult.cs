namespace DotOrg.Db.Infrastructure
{
	public class ValidateResult
	{
		public ValidateResult()
		{
		}

		public ValidateResult(string memberName, string message)
		{
			MemberName = memberName;
			Message = message;
		}

		public ValidateResult(string message)
		{
			Message = message;
		}

		public string MemberName { get; set; }

		public string Message { get; set; }
	}
}