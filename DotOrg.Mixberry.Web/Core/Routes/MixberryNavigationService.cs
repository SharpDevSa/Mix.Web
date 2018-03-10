using System.Data.Entity;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Web.Services;

namespace DotOrg.Mixberry.Web.Core.Routes
{
	public class MixberryNavigationService : NavigationService
	{
		private readonly ILocalizationContext _localization;

		public MixberryNavigationService(DbContext db, ILocalizationContext localization) : base(db)
		{
			_localization = localization;
		}

		public string GetCategoryUrl(Category category)
		{
			return "/" + _localization.Current.Lang + "/catalog/" + category.Alias;
		}

		public string GetGroupUrl(ProductGroup group, Category category)
		{
			return GetCategoryUrl(category) + "#" + group.Alias;
		}

		public string GetProductUrl(Product product, Category category)
		{
			if (product.ShowDetails)
			{
				return GetCategoryUrl(category) + "/" + product.Alias;
			}
			return GetCategoryUrl(category) + "#" + product.Alias;
		}
	}
}