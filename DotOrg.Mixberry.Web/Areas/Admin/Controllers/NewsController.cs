using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	public partial class NewsController : BaseAdminController<News>
	{
		protected override IEnumerable<News> GetIndexEntities()
		{
			var result = base.GetIndexEntities();
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			result = result.Where(x => x.Lang == lang.Current.Lang);
			return result;
		}

		protected override void OnEntityEdited(News entity, FormCollection collection)
		{
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			entity.Lang = lang.Current.Lang;
			base.OnEntityEdited(entity, collection);
		}
	}
}