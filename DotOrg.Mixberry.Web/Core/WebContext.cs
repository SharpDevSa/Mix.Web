using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Db.Entities;
using DotOrg.Db.Entities.Mocks;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.ViewModels;
using DotOrg.Web.Models;

namespace DotOrg.Mixberry.Web.Core
{
	public class WebContext : IWebContext
	{
		public string OgImage { get; set; }

		public static IWebContext Instance
		{
			get { return DependencyResolver.Current.GetService<IWebContext>(); }
		}

		public bool IsPjax
		{
			get
			{
				return HttpContext.Current.Request.Headers["X-PJAX"] != null;
			}
		}

		public WebContext()
		{
			ScriptsContext = new ScriptsContext();
		}
		private Location _location;
		private bool _checked;
		public Location Location
		{
			get
			{
				if (_location == null && !_checked)
				{
					_checked = true;
					var locations = DependencyResolver.Current.GetService<DbContext>().Set<Location>().ToList();
					string alias = null;
					var route = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
					if (route != null)
					{
						var r = (Route)route.Route;
						alias = (string)r.Defaults["location"];
						if (string.IsNullOrEmpty(alias))
						{
							alias = (string)r.Defaults["controller"];
						}
					}
					if (!string.IsNullOrEmpty(alias))
					{
						var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
						var currentLang = lang.Current.Lang;
						_location = locations.FirstOrDefault(l => l.Visibility && l.Alias == alias && l.Lang == currentLang);
					}
				}
				return _location ?? new Location();
			}
			set { _location = value; }
		}

		private IMetadataEntity _defaultMetadata;
		public IMetadataEntity Metadata
		{
			get
			{
				return _metadata ?? _defaultMetadata ?? (_defaultMetadata = new CustomMetadata
				{
					MetaDescription = Location.MetaDescription,
					MetaKeywords = Location.MetaKeywords,
					MetaTitle = Location.MetaTitle ?? Location.Name,
					MetaData = Location.MetaData
				});
			}
			set { _metadata = value; }
		}


		private ViewDataDictionary _viewData;
		private DotOrg.Web.Mvc.DynamicViewDataDictionary _dynamicViewData;
		private IMetadataEntity _metadata;

		public dynamic ViewBag
		{
			get { return _dynamicViewData ?? (_dynamicViewData = new DotOrg.Web.Mvc.DynamicViewDataDictionary(() => ViewData)); }
		}

		protected virtual void SetViewData(ViewDataDictionary viewData)
		{
			_viewData = viewData;
		}

		public ViewDataDictionary ViewData
		{
			get
			{
				if (_viewData == null)
				{
					SetViewData(new ViewDataDictionary());
				}
				return _viewData;
			}
			set { SetViewData(value); }
		}

		private readonly List<BreadcrumbsViewModel> _breadcrumbs = new List<BreadcrumbsViewModel>();

		public void PushBreadcrumb<T>(ItemViewModel<T> item) where T : IHaveAliasEntity
		{
			PushBreadcrumb(item.Item.Name, item.Item.Alias);
		}

		public void PushBreadcrumb(string name, string alias)
		{
			if (_breadcrumbs.All(model => model.Alias != alias))
			{
				_breadcrumbs.Add(new BreadcrumbsViewModel { Text = name, Alias = alias });
			}
		}

		public IEnumerable<BreadcrumbsViewModel> GetBreadcrumb()
		{
			return _breadcrumbs.ToArray();
		}

		public void ClearBreadcrumb()
		{
			_breadcrumbs.Clear();
		}

		public HtmlHelper Html { get { return WebPage.Html; } }
		public UrlHelper Url { get { return WebPage.Url; } }

		public WebViewPage WebPage { get; set; }

		public ScriptsContext ScriptsContext { get; set; }
	}
}