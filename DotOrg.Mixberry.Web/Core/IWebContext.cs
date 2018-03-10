using System.Collections.Generic;
using System.Web.Mvc;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.ViewModels;
using DotOrg.Web.Models;
using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Mixberry.Web.Core
{
	public interface IWebContext : IDotOrgWebContext
	{
		Location Location { get; set; }
		void PushBreadcrumb(string name, string alias);
		void PushBreadcrumb<T>(ItemViewModel<T> item) where T : IHaveAliasEntity;
		IEnumerable<BreadcrumbsViewModel> GetBreadcrumb();
		void ClearBreadcrumb();

		HtmlHelper Html { get; }
		UrlHelper Url { get; }

		bool IsPjax { get; }

		WebViewPage WebPage { get; set; }
		ScriptsContext ScriptsContext { get; set; }
	}
}