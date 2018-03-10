using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Entities;
using DotOrg.Mixberry.Models.Localization;
using Newtonsoft.Json;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	public partial class CategoryController : BaseAdminController<Category>
    {
		protected override IEnumerable<Category> GetIndexEntities()
		{
			var result = base.GetIndexEntities();
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			result = result.Where(x => x.Lang == lang.Current.Lang);
			return result;
		}

		protected override void PrepareEntity(Category entity)
		{
			var l = DependencyResolver.Current.GetService<ILocalizationContext>();
			WebContext.ViewBag.Products = DataModelContext.Set<Product>().Where(x => !x.Categories.Any() || x.Categories.Any(p => p.Lang == l.Current.Lang));
			base.PrepareEntity(entity);
		}

		protected override void AppendBlock(Category entity, IBlockEntity block)
		{
			entity.Blocks.Add((CategoryBlock)block);
			base.AppendBlock(entity, block);
		}

		protected override void RemoveBlock(Category entity, IBlockEntity block)
		{
			entity.Blocks.Remove((CategoryBlock)block);
			DataModelContext.Set<CategoryBlock>().Remove((CategoryBlock)block);
			base.RemoveBlock(entity, block);
		}

		protected override void OnEntityEditing(Category entity, FormCollection collection)
		{
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			entity.Lang = lang.Current.Lang;
			var groupsValue = collection["Groups"];
			collection.Remove("Groups");
			PopulateGroups(groupsValue, entity);
			base.OnEntityEditing(entity, collection);
		}

		private void PopulateGroups(string groupsValue, Category entity)
		{
			var model = groupsValue.IsNullOrEmpty()
				? new GroupsListModel { List = new List<ProductGroupModel>() }
				: JsonConvert.DeserializeObject<GroupsListModel>(groupsValue);

			var listToRemove = entity.Groups.ToList();

			foreach (var item in model.List)
			{
				if (item.Id == 0 && item.Deleted)
					continue;
				if (!item.Deleted)
				{
					ProductGroup group;
					if (item.Id == 0)
					{
						group = new ProductGroup();
						DbContext.Set<ProductGroup>().Add(group);
					}
					else
					{
						group = listToRemove.First(x => x.Id == item.Id);
						listToRemove.Remove(group);
					}
					group.Name = item.Name;
					group.Alias = item.Alias;

					if (item.Alias.IsNullOrEmpty())
					{
						group.Alias = item.Name.Transliterate();
					}
					group.Alias = group.Alias.Transliterate();
					group.Products.Clear();
					foreach (var product in item.Products.Where(x => x.IsChecked))
					{
						group.Products.Add(DbContext.Set<Product>().Find(product.Id));
					}
					group.Category = entity;
				}
			}

			listToRemove.ForEach(x => DbContext.Set<ProductGroup>().Remove(x));
			DbContext.SaveChanges();
		}

		private class GroupsListModel
		{
			public List<ProductGroupModel> List { get; set; }
		}

		private class ProductGroupModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Alias { get; set; }
			public bool Deleted { get; set; }
			public List<ProductModel> Products { get; set; }
		}

		private class ProductModel
		{
			public int Id { get; set; }
			public bool IsChecked { get; set; }
		}
    }
}