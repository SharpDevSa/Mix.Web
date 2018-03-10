using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DotOrg.Core.Helpers;

namespace DotOrg.Web.Ozi.Model
{
	public class Sitemap
	{
		private const string NodeName = "siteMapNode";
		private const string TitleAttribute = "title";
		private const string ActionAttribute = "action";
		private const string ControllerAttribute = "controller";
		private const string DescriptionAttribute = "description";

		private XDocument XDoc { get; set; }
		public List<SitemapElement> SitemapList { get; set; }
		private string SitemapFilePath { get; set; }

		private Sitemap()
		{
			string path = Path.Combine(WebConfig.SettingsFolderPath, "Modules.sitemap");
			SitemapFilePath = HttpContext.Current.Server.MapPath(path);
			XDoc = XDocument.Load(SitemapFilePath);
			var q = XDoc.Root;
			SitemapList = GetChilds(q);
		}

		private static Sitemap _instance;
		public static Sitemap Instance
		{
			get
			{
				return _instance ?? (_instance = new Sitemap());
			}
		}

		private static List<SitemapElement> GetChilds(XContainer element)
		{
			var childs = (from e in element.Elements(NodeName)
										   select new SitemapElement
										   {
											   Title = e.GetString(TitleAttribute),
											   Action = e.GetString(ActionAttribute),
											   Role = e.GetString("role"),
											   Controller = e.GetString(ControllerAttribute),
											   Description = e.GetString(DescriptionAttribute),
											   Childs = GetChilds(e)
										   }).ToList();

			return childs;
		}
	}
}