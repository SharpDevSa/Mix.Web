using System.Diagnostics;
using System.IO;
using System.Web.Mvc;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	[Authorize]
	public partial class UtilsController : Controller
	{
		public virtual ActionResult ClearApplicationCache()
		{
			var server = System.Web.HttpContext.Current.Server;
			var streamReader = System.IO.File.OpenText(server.MapPath("~/Web.config"));
			var configContent = streamReader.ReadToEnd();
			streamReader.Close();

			var streamWriter = new StreamWriter(server.MapPath("~/Web.config"));
			streamWriter.Write(configContent);
			streamWriter.Close();

		    var request = System.Web.HttpContext.Current.Request;
            Debug.Assert(request.UrlReferrer != null);
            return Redirect(request.UrlReferrer.ToString());
		}

		public virtual ActionResult NotRealizedYet()
		{
			return View();
		}

		//public virtual ActionResult Sitemap()
		//{
		//	const string header = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
		//	const string root = "<urlset xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
		//	const string elemTemplate = "<url><loc>{0}</loc></url>";
		//	const string rootEnd = "</urlset>";
		//	var result = new StringBuilder();
		//	result.AppendLine(header);
		//	result.AppendLine(root);

		//	using (var db = new DbContext())
		//	{
		//		// todo: sitemap


		//		foreach (var item in db.Locations.Where(x => x.Visibility))
		//		{
		//			result.AppendFormat(elemTemplate, Url.FullUrl(Url.ForPage(item)));
		//			result.AppendLine();
		//		}
		//		//foreach (var item in db.Categories.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.Catalog.Details(item.Alias)));
		//		//	result.AppendLine();
		//		//}
		//		//foreach (var item in db.News.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.News.Details(item.Id)));
		//		//	result.AppendLine();
		//		//}
		//		//foreach (var item in db.Projects.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.Projects.Details(item.Alias)));
		//		//	result.AppendLine();
		//		//}
		//		//foreach (var item in db.Buildings.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.Buildings.Details(item.Alias)));
		//		//	result.AppendLine();
		//		//}
		//		//foreach (var item in db.Projects.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.Projects.Details(item.Alias)));
		//		//	result.AppendLine();
		//		//}
		//		//foreach (var item in db.ArticleCategories.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.Articles.Index(item.Alias)));
		//		//	result.AppendLine();
		//		//}
		//		//foreach (var item in db.Banks.Where(x => x.Visibility))
		//		//{
		//		//	result.AppendFormat(elemTemplate, Url.ActionAbsolute(MVC.Banks.Details(item.Alias)));
		//		//	result.AppendLine();
		//		//}
		//	}

		//	result.AppendLine(rootEnd);
		//	Debug.Assert(Request.UrlReferrer != null);
		//	const string relativeFilename = "~/sitemap.xml";
		//	var fullname = Server.MapPath(relativeFilename);
		//	if (System.IO.File.Exists(fullname))
		//	{
		//		System.IO.File.Delete(fullname);
		//	}
		//	using (var writer = new StreamWriter(fullname, false))
		//	{
		//		writer.Write(result.ToString());
		//	}
		//	if (Request.UrlReferrer == null)
		//		return RedirectToAction(MVC.Admin.Locations.Index());
		//	return Redirect(Request.UrlReferrer.ToString());
		//}
	}
}
