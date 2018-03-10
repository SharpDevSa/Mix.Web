using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using DotOrg.Mixberry.Web.Startup;
using DotOrg.Web.Mvc;

namespace DotOrg.Mixberry.Web
{
	public class Global : HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new DotOrgViewEngine());
			DependencyContainerConfig.RegisterDependencies();
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);


			//var user = Membership.GetUser("manager");
			//if (user == null)
			//{
			//	Membership.CreateUser("manager", "JD4f9zM4U0Zz");
			//}

			//if (!Roles.RoleExists("manager"))
			//{
			//	Roles.CreateRole("manager");
			//}

			//if (!Roles.IsUserInRole("manager", "manager"))
			//{
			//	Roles.AddUserToRole("manager", "manager");
			//}
		}
	}
}