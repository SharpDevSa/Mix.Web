using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Routes;

namespace DotOrg.Mixberry.Web.Core
{
	public static class UrlExtensions
	{
		public static string ForPage(this UrlHelper helper, Location location)
		{
			if (location == null)
				return null;
			var navigation = DependencyResolver.Current.GetService<MixberryNavigationService>();
			return navigation.GetUrl(location);
		}

		public static string ForPage(this UrlHelper helper, string alias)
		{
			var locations = DependencyResolver.Current.GetService<DbContext>().Set<Location>();
			var location = locations.FirstOrDefault(l => l.Alias == alias);
			return helper.ForPage(location);
		}

		public static string ForProduct(this UrlHelper helper, Product p, Category c)
		{
			var navigation = DependencyResolver.Current.GetService<MixberryNavigationService>();
			return navigation.GetProductUrl(p, c);
		}
	}
}