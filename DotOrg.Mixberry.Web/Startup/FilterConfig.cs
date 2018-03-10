using System.Web.Mvc;

namespace DotOrg.Mixberry.Web.Startup
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
