using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Mvc;

namespace DotOrg.Mixberry.Web.Controllers
{
	public partial class PartnersController : BaseController
	{
		private readonly ILocalizationContext _localization;
		private readonly IEnumerable<LocalizationText> _items;

		public PartnersController(ILocalizationContext localization)
		{
			_localization = localization;
			_items = GetEntities<LocalizationText>().Where(x => x.Lang == _localization.Current.Lang).ToList();
		}

		public virtual ActionResult Index()
		{
			var model = GetEntities<Partner>().Include(x => x.Countries).Include(x => x.Logo).Where(x => x.Visibility).ToList();

			if (DataModelContext.Entry(WebContext.Location).State == EntityState.Unchanged) DataModelContext.Entry(WebContext.Location).Collection(x => x.Images).Load();

			var countries = model.SelectMany(x => x.Countries).Distinct();

			countries.ToList().ForEach(x => { x.Name = Localize("country.name." + x.Name); });

			foreach (var p in model)
			{
				p.Name = Localize(p.Name);
				p.Url = Localize(p.Url);
			}

			return View(model);
		}

		private string Localize(string key)
		{
			return _items.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault();
		}
	}
}