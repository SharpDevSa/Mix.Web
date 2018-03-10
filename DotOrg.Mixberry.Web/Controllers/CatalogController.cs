using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Mvc;
using DotOrg.Mixberry.Web.Core.Routes;
using DotOrg.Mixberry.Web.ViewModels;
using EntityFramework.Filters;

namespace DotOrg.Mixberry.Web.Controllers
{
	public partial class CatalogController : BaseController
    {
		private readonly ILocalizationContext _localization;

		public CatalogController(ILocalizationContext localization)
		{
			_localization = localization;
		}

		public virtual ActionResult Category(string alias)
		{
			var model = GetEntities<Category>()
				.Include(x => x.Images)
				.Include(x => x.Products)
				.Include(x => x.Groups)

				.Include(x => x.Blocks)
				.Include(x => x.Blocks.Select(b => b.Images))

				.Include(x => x.Products.Select(p => p.Models))
				.Include(x => x.Products.Select(p => p.Models.Select(m => m.Image)))

				.Include(x => x.Products.Select(p => p.Image))
				.Include(x => x.Products.Select(p => p.PromoImage))
				.Include(x => x.Products.Select(p => p.Groups))

				.First(x => x.Alias == alias && x.Lang == _localization.Current.Lang);

			model.Blocks = model.Blocks.OrderBy(x => x.Sort).ToList();
			model.Products = model.Products.OrderBy(x => x.Sort).ToList();

			SetCatalogLocalizationRedirects(alias);

			return View(model);
		}

		public virtual ActionResult Product(string category, string product)
		{
			var model = GetEntities<Product>()
				.Include(x => x.Banners)
				.Include(x => x.Blocks)
				.Include(x => x.DetailsImages)
				.Include(x => x.Models)
				.Include(x => x.Models.Select(m => m.Image))
				.Include(x => x.Related)
				.Include(x => x.Related.Select(p => p.Image))
				.Include(x => x.Related.Select(p => p.PromoImage))
				.Include(x => x.Related.Select(p => p.Models))
				.Include(x => x.Related.Select(p => p.Models.Select(m => m.Image)))
				.Include(x => x.Related.Select(p => p.Categories))
				.First(x => x.Alias == product && x.Categories.Any(c => c.Alias == category && c.Lang == _localization.Current.Lang));

			if (!User.Identity.IsAuthenticated)
			{
				model.Blocks = model.Blocks.Where(x => x.Visibility).ToList();
				model.Related = model.Related.Where(x => x.Visibility).ToList();
			}

			model.Blocks = model.Blocks.OrderBy(x => x.Sort).ToList();

			var vm = new ProductViewModel
			{
				Product = model,
				Category = GetEntities<Category>().First(x => x.Alias == category && x.Lang == _localization.Current.Lang)
			};
			return View(vm);
		}

		protected override void EnableFilters()
		{
			DataModelContext.EnableFilter(FilterNames.Category).SetParameter("lang", _localization.Current.Lang);
		}

		private void SetCatalogLocalizationRedirects(string alias)
		{
			var location = WebContext.Location.Alias;
			_localization.ChangeUrl = lang =>
			{
				var db = DataModelContext;
				var category = db.Set<Category>().Where(x => x.Lang == lang && x.Visibility).Select(x => x.Alias).FirstOrDefault(x => x == alias);
				return "/" + new[] {lang, location, category}.JoinWith("/");
			};
		}

		//public virtual ActionResult RootCategory()
		//{
		//	var model = GetEntities<Category>()
		//		.Include(x => x.Blocks)
		//		.Include(x => x.Images)
		//		.Include(x => x.Products)
		//		.Include(x => x.Products.Select(p => p.Image))
		//		.Include(x => x.Products.Select(p => p.Models))
		//		.Include(x => x.Products.Select(p => p.Models.Select(m => m.Image)))
		//		.Include(x => x.Blocks.Select(b => b.Images))
		//		.ToList();
		//	foreach (var item in model)
		//	{
		//		item.Blocks = item.Blocks.OrderBy(x => x.Sort).ToList();
		//		item.Products = item.Products.OrderBy(x => x.Sort).ToList();
		//	}
		//	return View(model);
		//}
    }
}