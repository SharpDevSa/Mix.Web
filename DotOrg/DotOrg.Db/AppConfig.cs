using System.Configuration;
using System.Web.Mvc;
using DotOrg.Libs.Services;

namespace DotOrg.Db
{
	public static class AppConfig
	{
		public static readonly string BasePathForImages = "BasePathForImages";

		public static readonly string NoImageRelativePath = "NoImageRelativePath";
		public static readonly string BuildingsPhotosPageSize = "BuildingsPhotosPageSize";

		public static readonly string HomePagePopularProjectsCount = "HomePagePopularProjectsCount";

		public static string GoogleMapKey
		{
			get { return ConfigurationManager.AppSettings["GoogleMapKey"]; }
		}

		public static string GetValue(string name)
		{
			var settings = DependencyResolver.Current.GetService<ISettingsProvider>();
			return settings.GetValue(name);
		}
	}
}