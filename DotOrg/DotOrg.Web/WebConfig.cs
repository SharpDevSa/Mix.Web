using System.Collections.Specialized;
using System.Configuration;

namespace DotOrg.Web
{
	public class WebConfig
	{
	    public const int ListPageSize = 50;
        public const int TablePageSize = 80;
        public const int VisiblePages = 5;
        public const string NoImage = "~/Content/img/no-image.png";

		public static readonly string HomepageAlias = "home";
		public static readonly string BasePathForImages = "~/content/data";
		public static readonly string SettingsFolderPath = "~/areas/admin/settings";
        public static readonly char PathSeparator = '/';
		public static readonly bool RemoveTrailingSlash = true; // see also file rewriteRules.config

		public static string GoogleMapKey
		{
			get { return AppSettings["GoogleMapKey"]; }
		}

	    public static NameValueCollection AppSettings
		{
			get { return ConfigurationManager.AppSettings; }
		}
	}
}
