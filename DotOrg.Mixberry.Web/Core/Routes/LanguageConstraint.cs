using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Mixberry.Db;

namespace DotOrg.Mixberry.Web.Core.Routes
{
	public class LanguageConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
			RouteDirection routeDirection)
		{
			if (routeDirection == RouteDirection.UrlGeneration)
				return true;
			try
			{
				var val = (string)values[parameterName];
				var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
				return val == null || lang.ValidateLanguage(val, httpContext.User.IsInRole("admin"));
			}
			catch
			{
				// todo: log exception
				return false;
			}
		}
	}
}