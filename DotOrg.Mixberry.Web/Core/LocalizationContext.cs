using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;
using DotOrg.Web;

namespace DotOrg.Mixberry.Web.Core
{
	public class LocalizationContext : ILocalizationContext
	{
		private readonly DbContext _db;
		private readonly RequestContext _requestContext;

		private Language _currentLanguage;
		private IEnumerable<Language> _cache;
		private Language _defaultLanguage;

		public LocalizationContext(LangDbContext db, RequestContext requestContext)
		{
			_db = db;
			_requestContext = requestContext;
		}

		public Language Current
		{
			get { return GetCurrentLanguage(); }
		}

		//public Location ChangeLocation(Location from, string langTo)
		//{
		//	var result = _db.Set<Location>()
		//		.Where(x => x.Id == from.Id)
		//		.SelectMany(x => x.Localization.Group)
		//		.FirstOrDefault(x => x.Localization.Lang == langTo);
		//	return result;
		//}

		public bool ValidateLanguage(string lang, bool isAdmin = false)
		{
			var result = _db.Set<Language>().Any(x => x.Lang == lang && (isAdmin || !x.Deleted));
			return result;
		}

		public Func<string, string> ChangeUrl
		{
			get { return ChangeUrlInternal; }
			set { _changeUrl = value; }
		}

		private Func<string, string> _changeUrl;

		private string ChangeUrlInternal(string lang)
		{
			if (lang == Current.Lang)
				return string.Empty;

			if (_changeUrl != null)
				return _changeUrl(lang);
			return ChangeLangForUrl(HttpContext.Current.Request.RawUrl, lang);
		}

		public IEnumerable<Language> All()
		{
			return _cache ?? (_cache = _db.Set<Language>().Where(x => !x.Deleted).ToList());
		}

		public Language GetDefaultLang()
		{
			return _defaultLanguage ?? (_defaultLanguage = _db.Set<Language>().FirstOrDefault(x => x.Primary));
		}

		public string SetUrlLang(string url)
		{
			var prefix = WebConfig.PathSeparator + Current.Lang;
			if (url.IsNullOrEmpty())
				return prefix;
			if (url.StartsWith(prefix))
				return url;
			return prefix + url;
		}

		private Language GetCurrentLanguage()
		{
			if (_currentLanguage == null)
			{
				var url = _requestContext.HttpContext.Request.RawUrl;
				var route = GetRoute(url);
				var name = (string)route.Values["lang"];// ?? _requestContext.HttpContext.Request.Params["lang"];

				_currentLanguage = name.IsNullOrEmpty()
					? GetDefaultLang()
					: _db.Set<Language>().FirstOrDefault(x => x.Lang == name);
				if (_currentLanguage == null) _currentLanguage = new Language();
			}
			return _currentLanguage;
		}

		#region lang redirects
		private static string ChangeLangForUrl(string url, string lang)
		{
			if (url == "/")
			{
				return "/" + lang;
			}

			var route = GetRoute(url);
			route.Values["lang"] = lang;
			route.Values.Remove("action");
			route.Values.Remove("id");
			var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), route));
			var result = urlHelper.RouteUrl(route.Values)
				?? ChangeLangRecursively(route.Values, lang, urlHelper);
			return result;
		}

		public static string ReplaceLangInUrl(string url, string lang)
		{
			if (url == "/")
			{
				return "/" + lang;
			}

			var route = GetRoute(url);
			route.Values["lang"] = lang;
			var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), route));
			var result = urlHelper.RouteUrl(route.Values)
				?? ChangeLangRecursively(route.Values, lang, urlHelper);
			return result;
		}

		private static string ChangeLangRecursively(RouteValueDictionary routeValues, string lang, UrlHelper urlHelper)
		{
			var name = (string)routeValues["localizationRedirectRouteName"];
			var rule = RouteTable.Routes[name] as Route;
			if (rule == null)
				return null;
			var d = new RouteValueDictionary();
			foreach (var item in rule.Defaults)
			{
				d.Add(item.Key, item.Value);
			}
			d["lang"] = lang;
			name += "-lang";
			var result = urlHelper.RouteUrl(name, d)
				?? ChangeLangRecursively(d, lang, urlHelper);
			return result;
		}

		private static RouteData GetRoute(Uri uri)
		{
			return GetRoute(uri == null ? "/" : uri.PathAndQuery);
		}

		private static RouteData GetRoute(string url)
		{
			var httpContext = new DummyHttpContext(url);
			var routeData = RouteTable.Routes.GetRouteData(httpContext);
			return routeData;
		}

		private class DummyHttpRequest : HttpRequestBase
		{

			private readonly string _url;

			public DummyHttpRequest(string url)
			{
				_url = url.StartsWith("~") ? url : "~" + url;
			}

			public override string AppRelativeCurrentExecutionFilePath
			{
				get
				{
					return _url;
				}
			}

			public override Uri UrlReferrer
			{
				get
				{
					return null;
				}
			}

			public override string PathInfo
			{
				get
				{
					return string.Empty;
				}
			}
		}

		private class DummyHttpContext : HttpContextBase
		{

			private readonly string _url;

			public DummyHttpContext(string url)
			{
				_url = url;
			}

			public override HttpSessionStateBase Session
			{
				get
				{
					return null;
				}
			}

			public override HttpRequestBase Request
			{
				get
				{
					return new DummyHttpRequest(_url);
				}
			}

			public override IPrincipal User
			{
				get { return HttpContext.Current.User; }
				set { HttpContext.Current.User = value; }
			}
		}

		#endregion

		#region static localization

		public void ResetStaticCache()
		{
			_loadedFiles = new Dictionary<string, Dictionary<string, string>>();
		}

		public string GetStaticText(string key)
		{
			var dictionary = GetDictionary();
			if (dictionary != null && dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			dictionary = GetDefaultDictionary();
			if (dictionary != null && dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			return key;
		}

		private Dictionary<string, string> GetDefaultDictionary()
		{
			return GetDictionaryByLang(GetDefaultLang().Lang);
		}

		private Dictionary<string, string> GetDictionary()
		{
			return GetDictionaryByLang(Current.Lang);
		}

		private static Dictionary<string, Dictionary<string, string>> _loadedFiles = new Dictionary<string, Dictionary<string, string>>();

		private Dictionary<string, string> GetDictionaryByLang(string lang)
		{
			if (!_loadedFiles.ContainsKey(lang))
			{
				var file = string.Format("~/App_data/{0}.xml", lang);
				var fullpath = HostingEnvironment.MapPath(file);
				if (File.Exists(fullpath))
				{
					try
					{
						var doc = XDocument.Load(fullpath);
						var root = doc.Element("mixberry");
						if (root != null)
						{
							var result = new Dictionary<string, string>();
							var elements = root.Elements().ToList();
							elements.ForEach(x => result.Add(x.Name.ToString(), x.Value));
							_loadedFiles.Add(lang, result);
						}
					}
					catch
					{
						return null;
					}
				}
				else return null;
			}
			return _loadedFiles[lang];
		}

		#endregion
	}
}