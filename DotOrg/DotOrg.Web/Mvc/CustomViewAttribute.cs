using System;

namespace DotOrg.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CustomViewAttribute : Attribute
	{
		private readonly string _typeName;
		private readonly string _aliasName;

		public CustomViewAttribute(string typeName, string aliasName = "Alias")
		{
			_typeName = typeName;
			_aliasName = aliasName;
		}

		public string AliasName
		{
			get { return _aliasName; }
		}

		public string TypeName
		{
			get { return _typeName; }
		}
	}
}