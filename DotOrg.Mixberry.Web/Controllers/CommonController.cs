using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Mvc;
using DotOrg.Mixberry.Web.Core.Routes;
using DotOrg.Mixberry.Web.ViewModels;
using DotOrg.Web.Services;
using EntityFramework.Filters;

namespace DotOrg.Mixberry.Web.Controllers
{
	public partial class CommonController : BaseController
    {
		private readonly MixberryNavigationService _navigation;
		private readonly ILocalizationContext _localization;

		public CommonController(MixberryNavigationService navigation, ILocalizationContext localization)
		{
			_navigation = navigation;
			_localization = localization;
		}

		public virtual ActionResult Feedback()
		{
			var vm = new FeedbackViewModel();
			return PartialView(vm);
		}

		[HttpPost]
	    public virtual ActionResult Feedback(FeedbackViewModel model)
		{
			return PartialView();
		}

		[ChildActionOnly]
		public virtual ActionResult SearchPanel()
		{
			return PartialView();
		}

		[ChildActionOnly]
		public virtual ActionResult LanguagePanel()
		{
			var model = new LanguagesViewModel
			{
				Current = _localization.Current,
				All = _localization.All()
			};
			return PartialView(model);
		}

		[ChildActionOnly]
		public virtual ActionResult Menu()
		{
			var locations = GetEntities<Location>().Where(x => x.Visibility && x.ShowInMenu && x.Lang == _localization.Current.Lang).OrderBy(x => x.Sort).ToList();

			var model = locations.Where(x => !x.ParentId.HasValue).Select(x => new MenuItemViewModel
			{
				Alias = x.Alias,
				Name = x.Name,
				Header = x.Header,
				Url = _navigation.GetUrl(x),
				Children = GetChildrenItems(x)
			}).ToList();

			return PartialView(model);
		}

		private List<MenuItemViewModel> GetChildrenItems(Location location)
		{
			if (location.Alias == "catalog")
			{
				var categories = GetEntities<Category>().Include(x => x.Groups).Where(x => x.Visibility && x.ShowInMenu && x.Lang == _localization.Current.Lang).OrderBy(x => x.Sort).ToList();
				return categories.Select(c => new MenuItemViewModel
				{
					Alias = c.Alias,
					Name = c.Name,
					Url = _navigation.GetCategoryUrl(c),
					Children = c.Groups.Select(p => new MenuItemViewModel
					{
						Alias = p.Alias,
						Name = p.Name,
						Url = _navigation.GetGroupUrl(p, c),
						Children = new List<MenuItemViewModel>()
					}).ToList()
				}).ToList();
			}
			else
			{
				return new List<MenuItemViewModel>();
			}
		}

		[ChildActionOnly]
		public virtual ActionResult FooterMenu()
		{
			var locations = GetEntities<Location>().Where(x => x.Visibility && x.ShowInMenu && x.ParentId == null && x.Lang == _localization.Current.Lang).OrderBy(x => x.Sort).ToList();
			var model = locations.Select(x => new MenuItemViewModel
			{
				Alias = x.Alias,
				Name = x.Name,
				Url = _navigation.GetUrl(x),
				Children = new List<MenuItemViewModel>()
			}).ToList();

			return PartialView(model);
		}

		protected override void EnableFilters()
		{
			DataModelContext.EnableFilter(FilterNames.Location).SetParameter("lang", _localization.Current.Lang);
			DataModelContext.EnableFilter(FilterNames.Category).SetParameter("lang", _localization.Current.Lang);
		}
    }
}