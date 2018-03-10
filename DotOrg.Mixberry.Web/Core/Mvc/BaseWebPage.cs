using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using RazorEngine.Templating;
using System.Web.WebPages;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Templating;

namespace DotOrg.Mixberry.Web.Core.Mvc
{
	public abstract class BaseWebPage<TModel> : DotOrg.Web.Mvc.BaseWebPage<TModel>
	{
		private MixberryTemplateEngine _templateEngine;

		protected BaseWebPage()
		{
			WebContext.WebPage = this;
			_templateEngine = DependencyResolver.Current.GetService<MixberryTemplateEngine>();
			L = new ViewLocalizationHelper(Localization);
		}

		public bool IsMobileDevice
		{
			get { return Request.Browser.IsMobileDevice; }
		}

		protected ILocalizationContext Localization { get { return DependencyResolver.Current.GetService<ILocalizationContext>(); } }
		protected IWebContext WebContext { get { return Core.WebContext.Instance; } }

		public IHtmlString ByCondition(bool condition, string value)
		{
			return MvcHtmlString.Create(condition ? value : string.Empty);
		}

		public IHtmlString RenderBreadcrumbs()
		{
			return Html.Partial(MVC.Shared.Views._Breadcrumbs);
		}

		public IHtmlString RenderRazor(int categoryId, string key, string content, object model)
		{
			var vm = new TemplateModel
			{
				Category = model as Category,
				Group = model as ProductGroup,
				Product = model as Product
			};
			var result = _templateEngine.RenderTemplate(string.Format("{0}.{1}", categoryId, key), content, vm);
			return new HtmlString(result);
		}

		public static ViewLocalizationHelper L { get; private set; }

		public IHtmlString T(string key)
		{
			return MvcHtmlString.Create(L.Get(key));
		}

		public class ViewLocalizationHelper
		{
			private readonly ILocalizationContext _context;

			public ViewLocalizationHelper(ILocalizationContext context)
			{
				_context = context;
			}

			public string Url(string url)
			{
				return _context.SetUrlLang(url);
			}

			public string Get(string key)
			{
				return _context.GetStaticText(key);
			}
		}
	}

	public abstract class BaseWebPage : BaseWebPage<object>
	{
	}
}