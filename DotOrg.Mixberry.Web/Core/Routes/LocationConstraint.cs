using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Logging;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Web;

namespace DotOrg.Mixberry.Web.Core.Routes
{
	public class LocationConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			if (routeDirection == RouteDirection.UrlGeneration)
				return true;
			try
			{
				var db = DependencyResolver.Current.GetService<DbContext>();
				var localization = DependencyResolver.Current.GetService<ILocalizationContext>();
				var lang = ((string) values["lang"]) ?? localization.GetDefaultLang().Lang;
				var loggedIn = httpContext.User.Identity.IsAuthenticated;
				var locations = db.Set<Location>().Where(l => (loggedIn || l.Visibility) && l.Lang == lang).ToList();
					//httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("admin")
					//? db.Set<Location>().ToList()
					//: db.Set<Location>().Where(l => l.Visibility).ToList();

				var webContext = DependencyResolver.Current.GetService<IWebContext>();
				var path = ((string)values["location"]);
				var result = CheckIncomingRouteValues(path, locations);
				if (routeDirection == RouteDirection.IncomingRequest)
				{
					webContext.Location = result;
				}
				return result != null;
			}
			catch (Exception e)
			{
				LogManager.GetLogger("Navigation").Error("LocationConstraint", e);
			}
			return false;
		}

		private Location CheckIncomingRouteValues(string path, IList<Location> locations)
		{
			if (string.IsNullOrEmpty(path))
				return locations.FirstOrDefault(l => l.Alias == WebConfig.HomepageAlias);
			var parts = path.Split(new []{'/'}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower()).ToList();
			return CheckParts(parts, locations);
		}

		private Location CheckParts(IList<string> parts, IList<Location> locations)
		{
			var page = locations.FirstOrDefault(l => l.Alias == parts[0] && l.ParentId == null);
			return page != null && page.Alias != WebConfig.HomepageAlias ? CheckPartsRecursively(page, parts, 1, locations) : null;
		}

		private Location CheckPartsRecursively(Location current, IList<string> parts, int index, IList<Location> locations)
		{
			if (index >= parts.Count)
				return current;
			var page = locations.FirstOrDefault(l => l.ParentId == current.Id && l.Alias == parts[index]);
			return page != null ? CheckPartsRecursively(page, parts, index + 1, locations) : null;
		}

	}

}