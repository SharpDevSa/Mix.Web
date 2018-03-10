using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Web;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace DotOrg.Mixberry.Web.Core.Templating
{
	public class MixberryTemplate<TModel> : HtmlTemplateBase<TModel>
	{
		private ILocalizationContext _localization;
		//public Product[] Products
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.CategoryIsPrimary && model.Category != null)
		//			{
		//				return model.Category.Products.OrderBy(x => x.Sort).ToArray();
		//			}
		//			if (!model.CategoryIsPrimary && model.Product != null)
		//			{
		//				return new[]{model.Product};
		//			}
		//		}
		//		return new Product[0];
		//	}
		//}

		//public string[] Images
		//{
		//	get
		//	{
		//		var result = new string[0];
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.CategoryIsPrimary && model.Category != null)
		//			{
		//				result = model.Category.Images.OrderBy(i => i.Sort).Select(x => x.Url).ToArray();
		//			}
		//			if (!model.CategoryIsPrimary && model.Product != null)
		//			{
		//				result = model.Product.Models.OrderBy(i => i.Sort).Select(x => x.Image != null ? x.Image.Url : string.Empty).ToArray();
		//			}
		//			if (result.Any())
		//			{
		//				return result.Select(x => x.IsNullOrEmpty() ? string.Empty : UrlHelper.GenerateContentUrl(x, new HttpContextWrapper(HttpContext.Current))).ToArray();
		//			}
		//		}
		//		return new string[0];
		//	}
		//}

		//public ProductModel[] Models
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.Product != null)
		//			{
		//				return model.Product.Models.ToArray();
		//			}
		//		}
		//		return new ProductModel[0];
		//	}
		//}

		//public string FirstImage
		//{
		//	get { return Images.FirstOrDefault(); }
		//}

		//public IEncodedString Name
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.CategoryIsPrimary && model.Category != null)
		//				return new RawString(model.Category.Name);
		//			if (model.Product != null)
		//			{
		//				return new RawString(model.Product.Name);
		//			}
		//		}
		//		return null;
		//	}
		//}

		//public IEncodedString SpecImage
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.Product != null && model.Product.Image != null)
		//			{
		//				var url = model.Product.Image.Url;
		//				return GetImageUrl(url, 0, 0);
		//			}
		//		}
		//		return null;
		//	}
		//}
		//public IEncodedString Specification
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.Product != null)
		//			{
		//				return new RawString(model.Product.Specification);
		//			}
		//		}
		//		return null;
		//	}
		//}

		//public IEncodedString Description
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.CategoryIsPrimary && model.Category != null)
		//				return new RawString(model.Category.Description);
		//			if (model.Product != null)
		//			{
		//				return new RawString(model.Product.Description);
		//			}
		//		}
		//		return null;
		//	}
		//}

		//public IEncodedString Alias
		//{
		//	get
		//	{
		//		var model = Model as TemplateModel;
		//		if (model != null)
		//		{
		//			if (model.CategoryIsPrimary && model.Category != null)
		//				return new RawString(model.Category.Alias);
		//			if (model.Product != null)
		//			{
		//				return new RawString(model.Product.Alias);
		//			}
		//		}
		//		return null;
		//	}
		//}

		//public IEncodedString GetImage(int index, int width = 0, int height = 0)
		//{
		//	var images = Images;
		//	if (images.Length > index && index >=0)
		//	{
		//		return GetImageUrl(images[index], width, height);
		//	}
		//	return null;
		//}

		//public IEncodedString Image(int width = 0, int height = 0)
		//{
		//	var url = FirstImage;
		//	return GetImageUrl(url, width, height);
		//}

		//public IEncodedString EachProduct(string oddTemplate, string evenTemplate)
		//{
		//	var db = DependencyResolver.Current.GetService<DbContext>();
		//	var ot = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == oddTemplate);
		//	if (ot == null)
		//		return null;
		//	var et = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == evenTemplate);
		//	if (et == null)
		//		return null;
		//	var engine = DependencyResolver.Current.GetService<MixberryTemplateEngine>();
		//	var result = Products.Select((p, i) => (i % 2 == 0)
		//					? engine.RenderTemplate(evenTemplate + p.Alias, et.Template, p)
		//					: engine.RenderTemplate(oddTemplate + p.Alias, ot.Template, p)
		//				).ToArray();
		//	return new RawString(result.JoinWith(""));
		//}

		//public IEncodedString EachProduct(string template)
		//{
		//	var db = DependencyResolver.Current.GetService<DbContext>();
		//	var t = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == template);
		//	if (t == null)
		//		return null;
		//	var engine = DependencyResolver.Current.GetService<MixberryTemplateEngine>();
		//	var result = Products.Select(p => engine.RenderTemplate(template + p.Alias, t.Template, p)).ToList();
		//	return new RawString(result.ToArray().JoinWith(""));
		//}
		public MixberryTemplate()
		{
			_localization = DependencyResolver.Current.GetService<ILocalizationContext>();
		}
		private static IEncodedString GetImageUrl(string url)
		{
			if (url.IsNullOrEmpty())
				url = WebConfig.NoImage;

			return new RawString(UrlHelper.GenerateContentUrl(url, new HttpContextWrapper(HttpContext.Current)));
		}

		private static IEncodedString RenderProductsTemplate(List<TemplateModel> models, string[] templates, DbContext db)
		{
			var blockTemplates = templates.Select(t => db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == t)).Where(t => t != null).ToList();
			var engine = DependencyResolver.Current.GetService<MixberryTemplateEngine>();
			var result = models.Select((x, i) =>
			{
				var t = blockTemplates[i % blockTemplates.Count];
				var html = engine.RenderTemplate(t.Name, t.Template, x);
				return html;
			}).ToArray();
			return new RawString(result.ToArray().JoinWith(""));
		}

		private static DbContext GetDb()
		{
			return DependencyResolver.Current.GetService<DbContext>();
		}

		public static IEncodedString Template(BlockTemplate template, TemplateModel model)
		{
			var engine = DependencyResolver.Current.GetService<MixberryTemplateEngine>();
			var result = engine.RenderTemplate(template.Name, template.Template, model);
			return new RawString(result);
		}

		public static IEncodedString Image(WebFile file)
		{
			return GetImageUrl(file == null ? null : file.Url);
		}

		public static IEncodedString CategoryProducts(string alias, params string[] templates)
		{
			if (templates.Length == 0)
				return null;
			var db = GetDb();
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			var products = db.Set<Category>().Where(x => x.Alias == alias && x.Lang == l.Current.Lang && x.Visibility)
				.SelectMany(x => x.Products).Where(x => x.Visibility).OrderBy(x => x.Sort).Select(x => new TemplateModel { Product = x }).ToList();
			return RenderProductsTemplate(products, templates, db);
		}

		public static IEncodedString CategoryGroups(string alias, params string[] templates)
		{
			if (templates.Length == 0)
				return null;
			var db = GetDb();
			var groups = db.Set<Category>().Where(x => x.Alias == alias).SelectMany(x => x.Groups).Select(x => new TemplateModel{ Group = x }).ToList();
			return RenderProductsTemplate(groups, templates, db);
		}

		public static IEncodedString GroupProducts(string alias, params string[] templates)
		{
			if (templates.Length == 0)
				return null;
			var db = GetDb();
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			var products = db.Set<ProductGroup>().Where(x => x.Alias == alias && x.Category.Lang == l.Current.Lang).SelectMany(x => x.Products).Where(x => x.Visibility).Select(x => new TemplateModel { Product = x }).ToList();
			return RenderProductsTemplate(products, templates, db);
		}

		public static IEncodedString Category(string alias, string template)
		{
			var db = GetDb();
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			var t = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == template);
			if (t == null)
				return null;
			var category = db.Set<Category>()
				.Include(x => x.Images)
				.Include(x => x.Blocks)
				.Include(x => x.Products)
				.Include(x => x.Products.Select(p => p.Image))
				.Include(x => x.Products.Select(p => p.PromoImage))
				.Include(x => x.Products.Select(p => p.Groups))
				.Include(x => x.Products.Select(p => p.Groups.Select(g => g.Products)))
				.Include(x => x.Products.Select(p => p.Models))
				.Include(x => x.Products.Select(p => p.Models.Select(m => m.Image)))
				.FirstOrDefault(x => x.Alias == alias && x.Lang == l.Current.Lang);
			if (category != null)
			{
                category.Products = category.Products.Where(x => x.Visibility).OrderBy(x => x.Sort).ToList();
				foreach (var group in category.Groups)
				{
                    group.Products = group.Products.Where(x => x.Visibility).OrderBy(x => x.Sort).ToList();
				}
			}
			return Template(t, new TemplateModel{Category = category});
		}

		public static IEncodedString Group(string alias, string template, Category category = null)
		{
			var db = GetDb();
			var t = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == template);
			if (t == null)
				return null;
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			var group = db.Set<ProductGroup>()
				.Include(x => x.Category)
				.Include(x => x.Products)
				.Include(x => x.Products.Select(p => p.Models))
				.Include(x => x.Products.Select(p => p.Image))
				.Include(x => x.Products.Select(p => p.PromoImage))
				.Include(x => x.Products.Select(p => p.Models.Select(m => m.Image)))
				.FirstOrDefault(x => x.Alias == alias && x.Category.Lang == l.Current.Lang);
			if (group != null)
			{
				group.Products = group.Products.Where(x => x.Visibility).ToList();
				if (group.Category != null)
				{
					group.Category.Products = group.Category.Products.Where(x => x.Visibility).ToList();
				}
			}
			return Template(t, new TemplateModel { Group = group, Category = category });
		}

		public static IEncodedString Product(string alias, string template, Category category = null, ProductGroup group = null)
		{
			var db = GetDb();
			var t = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == template);
			if (t == null)
				return null;
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			var product = db.Set<Product>()
				.Include(x => x.Categories)
				.Include(x => x.Models)
				.Include(x => x.Image)
				.Include(x => x.PromoImage)
				.Include(x => x.Categories.Select(c => c.Blocks))
				.Include(x => x.Categories.Select(c => c.Images))
				.Include(x => x.Models.Select(m => m.Image))
				.FirstOrDefault(x => x.Alias == alias && x.Categories.All(c => c.Lang == l.Current.Lang) && x.Visibility);

			return Template(t, new TemplateModel {Product = product, Category = category, Group = group});
		}

		public static IEncodedString Template(string template, TemplateModel model)
		{
			var db = GetDb();
			var t = db.Set<BlockTemplate>().FirstOrDefault(x => x.Name == template);
			if (t == null)
				return null;
			return Template(t, model);
		}

		public IEncodedString T(string key)
		{
			return new RawString(_localization.GetStaticText(key));
		}

		public string Lang
		{
			get { return _localization.Current.Lang; }
		}
	}
}