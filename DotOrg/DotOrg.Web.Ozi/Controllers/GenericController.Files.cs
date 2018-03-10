using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common.Logging;
using DotOrg.Core.Helpers;
using DotOrg.Db.Entities;
using DotOrg.Db.Entities.Mocks;
using DotOrg.Web.Ozi.PagesSettings.Forms;
using DotOrg.Web.Ozi.Services;
using DotOrg.Web.Ozi.ViewModels;
using Newtonsoft.Json.Linq;

namespace DotOrg.Web.Ozi.Controllers
{
	partial class GenericController<TEntity, TDbContext>
	{
		[HttpPost]
		public virtual FineUploaderResult UploadFile(FineUpload upload, int id, string propName)
		{
			try
			{
				var fileService = DependencyResolver.Current.GetService<IFileService>();
				var settings = (UploadFileSettings) Settings.FormSettings.Fields.FirstOrDefault(s => s.Name == propName);
				if (settings != null && WebContext.IsUserInAnyRole(settings.Roles))
				{
					try
					{
						var entity = Repository.GetByPrimaryKey(id);
						var value = TypeHelpers.GetPropertyValue(entity, propName);

						Func<IFileEntity> saveImage = () =>
						{
							// надо создать новое value и добавить его в базу
							var t = TypeHelpers.GetPropertyType(entity, propName);
							var file = (IFileEntity) Activator.CreateInstance(t);
							// сохраняем файл
							if (Request["notUpload"] == "true")
							{
								file.Url = Request["url"];
								file.Date = DateTime.Now;
							}
							else if (upload.InputStream != null && upload.InputStream.Length != 0)
							{
								fileService.SaveFile(upload.InputStream, settings, file, upload.Filename);
							}
							//var entitySetName = TypeHelpers.GetEntitySetName(file, Repository.DataContext);
							Repository.DataContext.Set(file.GetType()).Add(file);
							return file;
						};

						IFileEntity result;
						if (value is IEnumerable)
						{
							result = saveImage();
							var list = ((IList) value);
							var sortList = list.Cast<IFileEntity>().ToList();
							if (sortList.Any())
							{
								result.Sort = sortList.Max(fileEntity => fileEntity.Sort) + 1;
							}
							list.Add(result);
						}
						else
						{
							result = (IFileEntity) value;
							if (result != null)
							{
								fileService.DeleteFile(result, settings);
								Repository.DataContext.Set(result.GetType()).Remove(result);
							}
							result = saveImage();
							TypeHelpers.SetPropertyValue(entity, propName, result);
						}
						Repository.Save();
						return new FineUploaderResult(true, new
						{
							result.Id,
							Url = fileService.GetUrl(result),
							result.Alt,
							result.Title,
							result.Class,
							result.Description,
							result.Visibility,
							result.SourceName
						});
					}
					catch (Exception exception)
					{
						return new FineUploaderResult(false, null, exception.Message);
					}
				}
			}
			catch (Exception ex)
			{
				LogManager.GetLogger("GenericController").Error("upload file fails", ex);
			}
			return new FineUploaderResult(false, null, "файл не был передан");
		}

		[HttpPost]
		public virtual ActionResult DeleteFile(CustomFileEntity model)
		{
			try
			{
				var fileService = DependencyResolver.Current.GetService<IFileService>();
				var db = Repository.DataContext;
				var entity = Repository.GetByPrimaryKey(model.ObjId);
				var type = TypeHelpers.GetPropertyType(entity, model.PropName);
				//var entitySetName = TypeHelpers.GetEntitySetName(type, db);
				//var key = new EntityKey(entitySetName, "Id", model.Id);
				var file = (IFileEntity)db.Set(type).Find(model.Id);

				fileService.DeleteFile(file, null); // todo: будет работать только для локальных файлов
				var propValue = TypeHelpers.GetPropertyValue(entity, model.PropName);
				if (propValue is IEnumerable)
				{
					var list = ((IList)propValue);
					list.Remove(file);
				}
				else
				{
					TypeHelpers.SetPropertyValue(entity, model.PropName, null);
				}
				db.Set(type).Remove(file);
				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception exception)
			{
				return Json(new { success = false, error = exception.Message });
			}
		}

		[HttpPost]
		public virtual ActionResult SaveOrder(string model, int objId, string propName)
		{
			try
			{
				var index = 0;
				var obj = JObject.Parse(model);
				var list = (JArray)obj["items"];
				var db = Repository.DataContext;
				var entity = Repository.GetByPrimaryKey(objId);
				var prop = TypeHelpers.GetPropertyValue(entity, propName);
				var files = ((IEnumerable<IFileEntity>)prop).ToList();
				foreach (var item in list)
				{
					var fileId = item["Id"].Value<int>();
					var file = files.FirstOrDefault(f => f.Id == fileId);
					if (file != null)
					{
						index++;
						file.Sort = index;
						file.Description = item["Description"].Value<string>();
					}
				}
				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception exception)
			{
				return Json(new { success = false, error = exception.Message });
			}
		}

		[HttpPost]
		public virtual ActionResult SaveFileData(CustomFileEntity model)
		{
			try
			{
				var db = Repository.DataContext;
				var entity = Repository.GetByPrimaryKey(model.ObjId);
				var type = TypeHelpers.GetPropertyType(entity, model.PropName);
				//var entitySetName = TypeHelpers.GetEntitySetName(type, db);
				//var key = new EntityKey(entitySetName, "Id", model.Id);
				var file = (IFileEntity)db.Set(type).Find(model.Id);
				file.Alt = model.Alt;
				file.Description = model.Description;
				file.Title = model.Title;
				file.Class = model.Class;
				file.Visibility = model.Visibility;
				if (!model.Url.IsNullOrEmpty())
					file.Url = model.Url;
				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception exception)
			{
				return Json(new { success = false, error = exception.Message });
			}
		}


	}
}
