using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Core.Helpers;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Entities;
using DotOrg.Web;
using DotOrg.Web.Ozi.Controllers;
using DotOrg.Web.Ozi.PagesSettings.Forms;
using DotOrg.Web.Ozi.Services;
using DotOrg.Web.Ozi.ViewModels;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	[Authorize]
	public abstract class BaseAdminController<T> : GenericController<T, MixberryDbContext> where T : class, IEntity, new()
	{
		protected override void Initialize(RequestContext requestContext)
		{
			var localization = DependencyResolver.Current.GetService<ILocalizationContext>();
			localization.ChangeUrl = null;

			base.Initialize(requestContext);
		}

		protected override void OnEntityEditing(T entity, FormCollection collection)
		{
			var interfaces = typeof(T).GetInterfaces();
			var blockHolderType = interfaces.FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHaveBlocksEntity<>));
			if (blockHolderType != null)
			{
				SaveBlocks(entity, collection);
			}
			base.OnEntityEditing(entity, collection);
		}

		protected virtual void SaveBlocks(T entity, FormCollection collection)
		{
			var vm = (dynamic)entity;
			var listToRemove = Enumerable.Cast<IBlockEntity>((IEnumerable)vm.Blocks).ToList();

			//var engine = DependencyResolver.Current.GetService<MixberryTemplateEngine>();

			foreach (var key in collection.AllKeys.Where(x => x.StartsWith("BlockId")).ToList())
			{
				var idValue = collection[key];
				var sortValue = collection["BlockSort" + idValue];
				var imageIdValue = collection["BlockImagesId" + idValue];
				var id = int.Parse(idValue);
				var sort = int.Parse(sortValue);
				var name = collection["BlockName" + idValue];
				var alias = collection["BlockAlias" + idValue];
				var content = collection["BlockContent" + idValue];

				var item = listToRemove.FirstOrDefault(x => x.Id == id);
				if (item != null)
				{
					item.Sort = sort;
					item.Name = name;
					item.Alias = alias;
					if (item.Alias.IsNullOrEmpty())
					{
						item.Alias = item.Name.Transliterate();
					}
					item.Content = content;
					if (!imageIdValue.IsNullOrEmpty())
					{
						var imageId = int.Parse(imageIdValue);
						if (imageId != 0)
						{
							item.Images.Clear();
							item.Images.Add(Repository.DataContext.Set<WebFile>().Find(imageId));
						}
					}
					listToRemove.Remove(item);
				}

				collection.Remove(key);
				collection.Remove("BlockSort" + idValue);
				collection.Remove("BlockImageId" + idValue);
				collection.Remove("BlockName" + idValue);
				collection.Remove("BlockAlias" + idValue);
				collection.Remove("BlockContent" + idValue);
			}

			listToRemove.ForEach(x =>
			{
				RemoveBlock(entity, x);
			});
		}
		[HttpPost]
		public virtual ActionResult CreateBlock(int entityId)
		{
			var entity = DataModelContext.Set<T>().First(x => x.Id == entityId);
			var interfaces = typeof(T).GetInterfaces();
			var blockHolderType = interfaces.FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHaveBlocksEntity<>));
			if (blockHolderType != null)
			{
				var vm = (dynamic)entity;
				var blocks = Enumerable.Cast<IBlockEntity>((IEnumerable)vm.Blocks);
				var blockType = blockHolderType.GetGenericArguments()[0];
				var block = (IBlockEntity)Activator.CreateInstance(blockType);
				block.Sort = blocks.Select(x => x.Sort).DefaultIfEmpty().Max() + 1;

				DataModelContext.Set(block.GetType()).Add(block);

				AppendBlock(entity, block);

				DataModelContext.SaveChanges();
				return PartialView(block);
			}
			return new EmptyResult();
		}

		protected virtual void AppendBlock(T entity, IBlockEntity block)
		{
		}
		protected virtual void RemoveBlock(T entity, IBlockEntity block)
		{
		}

		[HttpPost]
		public virtual ActionResult UploadImage(FineUpload file)
		{
			if (file != null && file.InputStream != null && file.InputStream.Length > 0)
			{
				var fileService = DependencyResolver.Current.GetService<IFileService>();
				var item = new WebFile { Date = DateTime.Now };
				fileService.SaveFile(file.InputStream, new UploadFileSettings { PathToSave = WebConfig.BasePathForImages }, item, file.Filename);
				Repository.DataContext.Set<WebFile>().Add(item);
				Repository.Save();
				return Json(new
				{
					success = true,
					id = item.Id,
					url = Url.Content(item.Url)
				});
			}
			return Json(new { success = false, error = "файл не был загружен" });
		}
	}
}