using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Mixberry.Models;
using System.Data.Entity;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models.Entities;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	public partial class ProductController : BaseAdminController<Product>
    {
		protected override IEnumerable<Product> GetIndexEntities()
		{
			var result = base.GetIndexEntities();
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			result = result.Where(x => x.Categories.Any(c => c.Lang == lang.Current.Lang));
			return result;
		}

		protected override void PrepareEntity(Product entity)
		{
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			WebContext.ViewBag.Categories = DataModelContext.Set<Category>().Where(x => x.Lang == l.Current.Lang);
			WebContext.ViewBag.Related = DataModelContext.Set<Product>().Where(x => x.Categories.Any(c => c.Lang == l.Current.Lang) && x.Id != entity.Id);
			base.PrepareEntity(entity);
		}
    }
}