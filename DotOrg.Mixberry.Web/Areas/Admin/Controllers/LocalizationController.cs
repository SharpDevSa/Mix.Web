using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;
using DotOrg.Mixberry.Web.Areas.Admin.Models;
using DotOrg.Mixberry.Web.ViewModels;
using WebGrease;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	[Authorize]
	public partial class LocalizationController : Controller
    {
		private readonly DbContext _db;

		public LocalizationController(DbContext db)
		{
			_db = db;
		}

		public virtual ActionResult Index()
		{
			var model = _db.Set<Language>().Where(x => !x.Deleted).ToList();
            return View(model);
        }

		public virtual ActionResult Create()
		{
			var model = new Language();
			return View(MVC.Admin.Localization.Views.Edit, (object)model);
		}

		public virtual ActionResult Edit(string key)
		{
			var entity = _db.Set<Language>().FirstOrDefault(x => x.Lang == key);
			if (entity == null)
				return HttpNotFound();
			return View(entity);
		}

		[HttpPost]
		public virtual ActionResult Edit(string key, FormCollection collection)
		{
			var entity = key.IsNullOrEmpty()
				? new Language { Lang = key }
				: _db.Set<Language>().Find(key);

			TryUpdateModel(entity);

			//if (key.HasValue()) entity.Lang = key;
			if (!new []{EntityState.Modified, EntityState.Unchanged}.Contains(_db.Entry(entity).State)) _db.Entry(entity).State = EntityState.Added;

			if (entity.Primary)
			{
				_db.Set<Language>().Where(x => x.Lang != entity.Lang).ToList().ForEach(x => x.Primary = false);
			}

			_db.SaveChanges();

			var protoLang = collection["sourceLang"];
			if (protoLang.HasValue())
			{
				CopyDataFrom(protoLang, entity.Lang);
			}

			SaveStaticData(key, collection);

			if (collection["_apply"] != null)
				return RedirectToAction(MVC.Admin.Localization.Edit((string)entity.Lang));
			return RedirectToAction(MVC.Admin.Localization.Index());
		}

		private void SaveStaticData(string lang, FormCollection collection)
		{
			var file = HostingEnvironment.MapPath(string.Format("~/App_Data/{0}.xml", lang));
			XElement root;
			XDocument doc;
			if (System.IO.File.Exists(file))
			{
				doc = XDocument.Load(file);
				root = doc.Element("mixberry");
			}
			else
			{
				root = new XElement("mixberry");
				doc = new XDocument(root);
			}

			foreach (var key in collection.AllKeys.Where(x => x.StartsWith("localize_")))
			{
				var value = collection[key];
				var k = key.Substring("localize_".Length);

				var elem = root.Element(k);
				if (elem == null)
				{
					elem = new XElement(k);
					root.Add(elem);
				}
				elem.Value = value;
			}

			doc.Save(file);

		}

		public virtual ActionResult Delete(string key)
		{
			var lang = _db.Set<Language>().FirstOrDefault(x => x.Lang == key);
			if (lang != null && _db.Set<Language>().Count(x => !x.Deleted) > 1)
			{
				lang.Deleted = true;

				if (lang.Primary)
				{
					var otherLangs = _db.Set<Language>().Where(x => x.Primary);
					otherLangs.ToList().ForEach(x => x.Primary = false);
					var primaryLang = _db.Set<Language>().First(x => !x.Deleted);
					primaryLang.Primary = true;
					lang.Primary = false;
				}

				_db.Set<Language>().Remove(lang);

				_db.SaveChanges();
			}
			return RedirectToAction(MVC.Admin.Localization.Index());
		}

		[ChildActionOnly]
		public virtual ActionResult LangPanel()
		{
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			var model = new LanguagesViewModel
			{
				All = lang.All(),
				Current = lang.Current
			};
			return PartialView(model);
		}

		[ChildActionOnly]
		public virtual ActionResult StaticData(string key)
		{
			var model = new EditStaticLocalizationModel
			{
				Items = GetLocalizationItems(key)
			};
			return PartialView(model);
		}

		private List<EditStaticLocalizationItem> GetLocalizationItems(string key)
		{
			var givenFile = string.Format("~/App_data/{0}.xml", key);
			var givenPath = HostingEnvironment.MapPath(givenFile);

			var context = DependencyResolver.Current.GetService<ILocalizationContext>();

			var defaultFile = string.Format("~/App_data/{0}.xml", context.GetDefaultLang().Lang);
			var defaultPath = HostingEnvironment.MapPath(defaultFile);

			var doc = XDocument.Load(defaultPath);
			var root = doc.Element("mixberry");
			var elements = root.Elements().ToList();

			var result = elements.Select(x => new EditStaticLocalizationItem
			{
				Name = x.Name.ToString(),
				DefaultValue = x.Value,
				Label = x.Attribute("label") == null ? x.Name.ToString() : x.Attribute("label").Value
			}).ToList();

			XElement currentDoc;

			if (System.IO.File.Exists(givenPath))
			{
				currentDoc = XDocument.Load(givenPath).Element("mixberry");
			}
			else
			{
				currentDoc = new XElement("mixberry");
			}
			var list = currentDoc.Elements().ToList();

			list.ForEach(x =>
			{
				var k = x.Name.ToString();
				var label = (x.Attribute("label") == null ? k : x.Attribute("label").Value).Or(k);
				var item = result.FirstOrDefault(a => a.Name == k);

				if (item == null)
				{
					item = new EditStaticLocalizationItem
					{
						Name = k,
						Label = label
					};
					result.Add(item);
				}
				item.Label = label;
				item.Value = x.Value;
			});
			return result;

		}

		private void CopyDataFrom(string fromLang, string toLang)
		{
			var news = _db.Set<News>().AsNoTracking()
				.Include(x => x.Image)
				.Include(x => x.BigImage)
				.Where(x => x.Lang == fromLang).ToList();
			CopyNews(news, toLang);

			var categories = _db.Set<Category>().AsNoTracking()
				.Include(x => x.Groups)
				.Include(x => x.Blocks.Select(b => b.Images))
				.Include(x => x.Products.Select(p => p.Image))
				.Include(x => x.Products.Select(p => p.PromoImage))
				.Include(x => x.Products.Select(p => p.Models.Select(m => m.Image)))
				.Include(x => x.Products.Select(p => p.Groups))
				.Include(x => x.Images)
				.Where(x => x.Lang == fromLang).ToList();
			CopyCategories(categories, toLang);

			var locations = _db.Set<Location>().AsNoTracking()
				.Include(x => x.Blocks)
				.Include(x => x.Blocks.Select(b => b.Images))
				.Include(x => x.Images)
				.Where(x => x.Lang == fromLang).ToList();
			CopyLocation(locations, toLang);
		}

		private void CopyLocation(List<Location> list, string lang)
		{
			foreach (var item in list)
			{
				_db.Entry(item).State = EntityState.Detached;
				item.Lang = lang;
				item.Id = 0;
				item.ParentId = null;
				foreach (var image in item.Images)
				{
					image.Id = 0;
					_db.Entry(image).State = EntityState.Added;
				}
				foreach (var block in item.Blocks)
				{
					block.Id = 0;
					block.LocationId = 0;
					foreach (var image in block.Images)
					{
						image.Id = 0;
						_db.Entry(image).State = EntityState.Added;
					}
					_db.Entry(block).State = EntityState.Added;
				}
				_db.Entry(item).State = EntityState.Added;
			}
			_db.SaveChanges();
		}

		private void CopyCategories(List<Category> list, string lang)
		{
			// copy all products
			// copy all categories
			// update references

			var store = new Dictionary<Category, List<Product>>();

			foreach (var item in list)
			{
				item.Lang = lang;
				item.Id = 0;
				store.Add(item, item.Products.ToList());
				item.Products.Clear();
				foreach (var block in item.Blocks)
				{
					block.Id = 0;
					block.CategoryId = 0;
					foreach (var image in block.Images)
					{
						image.Id = 0;
					}
				}
				foreach (var image in item.Images)
				{
					image.Id = 0;
				}
				foreach (var group in item.Groups)
				{
					group.Id = 0;
					group.CategoryId = 0;
					group.Category = item;
					group.Products.Clear();
				}
				_db.Entry(item).State = EntityState.Added;
			}
			var allProducts = store.SelectMany(x => x.Value).Distinct((a, b) => a.Id == b.Id).ToList().ToDictionary(x => x.Id);

			_db.SaveChanges();

			var allGroups = list.SelectMany(x => x.Groups).ToList();

			foreach (var item in store.Keys)
			{
				var products = store[item];

				products.ForEach(x =>
				{
					var product = allProducts[x.Id];
					var groups = product.Groups.ToList();
					product.Groups.Clear();

					groups.ForEach(g =>
					{
						var group = allGroups.First(cg => cg.Alias == g.Alias);
						product.Groups.Add(group);
					});

					product.Id = 0;
					item.Products.Add(product);
					product.Models.ToList().ForEach(m =>
					{
						m.Id = 0;
						m.ImageId = null;
						m.ProductId = 0;
						if (m.Image != null)
						{
							m.Image.Id = 0;
						}
					});

					if (_db.Entry(product).State != EntityState.Added)
						_db.Entry(product).State = EntityState.Added;

				});
			}
			_db.SaveChanges();




			//foreach (var product in item.Products)
			//{
			//	product.Id = 0;
			//	product.ImageId = null;
			//	if (product.Image != null)
			//	{
			//		product.Image.Id = 0;
			//		_db.Entry(product.Image).State = EntityState.Added;
			//	}
			//	foreach (var model in product.Models)
			//	{
			//		model.Id = 0;
			//		model.ImageId = null;
			//		model.ProductId = 0;
			//		if (model.Image != null)
			//		{
			//			model.Image.Id = 0;
			//			_db.Entry(model.Image).State = EntityState.Added;
			//		}
			//		_db.Entry(model).State = EntityState.Added;
			//	}
			//	foreach (var group in product.Groups)
			//	{
			//		group.Id = 0;
			//		group.CategoryId = 0;
			//		_db.Entry(group).State = EntityState.Added;
			//	}
			//	_db.Entry(product).State = EntityState.Added;
			//}
		}

		private void CopyNews(IEnumerable<News> list, string toLang)
		{
			foreach (var item in list)
			{
				item.Id = 0;
				item.Lang = toLang;
				item.ImageId = null;
				item.BigImageId = null;
				if (item.Image != null)
				{
					item.Image.Id = 0;
					_db.Entry(item.Image).State = EntityState.Added;
				}
				if (item.BigImage != null)
				{
					item.BigImage.Id = 0;
					_db.Entry(item.BigImage).State = EntityState.Added;
				}
				_db.Entry(item).State = EntityState.Added;
			}
			_db.SaveChanges();
		}
    }
}