using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Mixberry.Web.Core.Routes;

namespace DotOrg.Mixberry.Web.Startup
{
	public class RouteConfig
	{
		private static string[] Namespaces
		{
			get { return new[] { "DotOrg.Mixberry.Web.Controllers" }; }
		}

		private static readonly LanguageConstraint LanguageConstraint = new LanguageConstraint();
		private static readonly LanguageRouteHandler LanguageHandler = new LanguageRouteHandler();

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("partners-lang", "{lang}/partners", MVC.Partners.Index(), new { location = "partners" }, new { lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("partners", "partners", MVC.Partners.Index(), new { location = "partners" }, null, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("news-lang", "{lang}/news", MVC.News.Index(), new { location = "news", page = 1 }, new { lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("news", "news", MVC.News.Index(), new { location = "news", page = 1 }, null, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("news-item-lang", "{lang}/news/{id}", MVC.News.Details(), new { location = "news" }, new { id = @"\d+", lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("news-item", "news/{id}", MVC.News.Details(), new { location = "news" }, new { id = @"\d+" }, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("feedback-lang", "{lang}/feedback", MVC.Common.Feedback(), new { location = "feedback" }, new { lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("feedback", "feedback", MVC.Common.Feedback(), new { location = "feedback" }, null, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("product-lang", "{lang}/catalog/{category}/{product}", MVC.Catalog.Product(), new { location = "catalog" }, new { product = new ProductConstraint() }, Namespaces);
			routes.MapRoute("product", "catalog/{category}/{product}", MVC.Catalog.Product(), new { location = "catalog" }, new { product = new ProductConstraint() }, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("category-lang", "{lang}/catalog/{alias}", MVC.Catalog.Category(), new { location = "catalog" }, new { alias = new CategoryConstraint(), lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("category", "catalog/{alias}", MVC.Catalog.Category(), new { location = "catalog" }, new { alias = new CategoryConstraint() }, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("homepage-lang", "{lang}", MVC.Default.Page(), new { location = "home" }, new { location = new LocationConstraint(), lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("homepage", "", MVC.Default.Page(), new { location = "home" }, new { location = new LocationConstraint() }, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute("pages-lang", "{lang}/{*location}", MVC.Default.Page(), null, new { location = new LocationConstraint(), lang = LanguageConstraint }, Namespaces);
			routes.MapRoute("pages", "{*location}", MVC.Default.Page(), null, new { location = new LocationConstraint() }, Namespaces).RouteHandler = LanguageHandler;

			routes.MapRoute(
				name: "Default-lang",
				url: "{lang}/{controller}/{action}/{id}",
				defaults: new { controller = "Default", action = "Page", id = UrlParameter.Optional,  },
				namespaces: Namespaces,
				constraints: new { lang = LanguageConstraint }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Default", action = "Page", id = UrlParameter.Optional },
				namespaces: Namespaces
			).RouteHandler = LanguageHandler;
		}
	}
}


