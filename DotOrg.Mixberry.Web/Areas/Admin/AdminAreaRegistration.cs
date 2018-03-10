using System.Web.Mvc;

namespace DotOrg.Mixberry.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		private static string[] Namespaces
		{
			get { return new[] { "DotOrg.Mixberry.Web.Areas.Admin.Controllers" }; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Admin_default-lang",
				"{lang}/admin/{controller}/{action}/{id}",
				new { controller = "Locations", action = "Index", id = UrlParameter.Optional }, namespaces: Namespaces
			);
			context.MapRoute(
				"Admin_default",
				"admin/{controller}/{action}/{id}",
				new { controller = "Locations", action = "Index", id = UrlParameter.Optional }, namespaces: Namespaces
			);
		}
	}
}