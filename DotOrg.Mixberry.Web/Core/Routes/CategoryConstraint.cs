using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Logging;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using EntityFramework.Filters;

namespace DotOrg.Mixberry.Web.Core.Routes
{
	public class CategoryConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
			RouteDirection routeDirection)
		{
			if (routeDirection == RouteDirection.UrlGeneration) return true;
			try
			{
				var db = DependencyResolver.Current.GetService<DbContext>();
				var _lang = DependencyResolver.Current.GetService<ILocalizationContext>();
				var lang = ((string) values["lang"]) ?? _lang.GetDefaultLang().Name;
				var loggedIn = httpContext.User.Identity.IsAuthenticated;
				var categories = db.Set<Category>().Where(l => (loggedIn || l.Visibility) && l.Lang == lang).ToList();
				var alias = ((string)values["alias"]);
				var result = categories.Any(x => x.Alias == alias);
				return result;
			}
			catch (Exception e)
			{
				LogManager.GetLogger("Navigation").Error("LocationConstraint", e);
			}
			return false;
		}
	}
}