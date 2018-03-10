using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Routes;
using DotOrg.Web.Services;

namespace DotOrg.Mixberry.Web.Core.Templating
{
	public class TemplateModel
	{
		private ProductGroup _group;
		private Category _category;
		private Product _product;

		public ProductGroup Group
		{
			get { return _group ?? new ProductGroup(); }
			set { _group = value; }
		}

		public Category Category
		{
			get { return _category ?? new Category(); }
			set { _category = value; }
		}

		public Product Product
		{
			get { return _product ?? new Product(); }
			set { _product = value; }
		}

		public List<Category> GetCatalog()
		{
			var db = DependencyResolver.Current.GetService<DbContext>();
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			var result = db.Set<Category>().Where(x => x.Visibility && x.Lang == l.Current.Lang).ToList();
			return result;
		}

		public string ProductUrl
		{
			get
			{
				var navigation = DependencyResolver.Current.GetService<MixberryNavigationService>();
				return navigation.GetProductUrl(Product, Product.Categories.FirstOrDefault());
			}
		}
	}
}