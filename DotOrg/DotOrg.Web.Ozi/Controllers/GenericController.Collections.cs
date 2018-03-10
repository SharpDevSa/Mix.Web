using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using DotOrg.Core.Helpers;
using DotOrg.Web.Ozi.Helpers;
using DotOrg.Web.Ozi.Services;
using IOFile = System.IO.File;
using System.Text;
using System.Web.Mvc;
using DotOrg.Db.Entities;
using DotOrg.Web.Ozi.PagesSettings.Forms;
using DotOrg.Web.Ozi.ViewModels;

namespace DotOrg.Web.Ozi.Controllers
{
	partial class GenericController<TEntity, TDbContext>
    {
		private void UpdateCollections(TEntity entity, FormCollection collection)
		{
			UpdateMultipleSelects(entity, collection);
			UpdateEditableMultipleSelect(entity, collection);
			UpdateEditableSelect(entity, collection);

			UpdateCollectionProperties(entity, collection);
		}

		private void UpdateCollectionProperties(TEntity entity, FormCollection collection)
		{
			const string prefix = "collection_";
			UpdateCollectionPropertiesRecursive(prefix, entity, collection, Settings.FormSettings.Fields.Where(settings => settings is CollectionSettings).Cast<CollectionSettings>());
		}

		private void UpdateCollectionPropertiesRecursive(string prefix, IEntity entity, IValueProvider valueProvider, IEnumerable<CollectionSettings> settingsList)
		{
			foreach (var item in settingsList)
			{
				var propertyName = string.Format("{0}{1}", prefix, item.Name);
				var entities = (IList)TypeHelpers.GetPropertyValue(entity, item.Name);
				var list = Enumerable.Cast<IEntity>(entities).ToList();
				var index = 0;
				var listToRemove = list.Select(e => e.Id).ToList();
				var childType = TypeHelpers.GetPropertyType(entity, item.Name); // возвращает тип элемента коллекции
				while (true) // редактирование и добавление элементов коллекции
				{
					var idName = string.Format(CultureInfo.InvariantCulture, "{0}[{1}].Id", propertyName, index);
					var idValue = valueProvider.GetValue(idName);
					if (idValue == null)
						break;
					var id = (int)idValue.ConvertTo(typeof(int));
					listToRemove.Remove(id);
					var child = list.FirstOrDefault(e => e.Id == id) ?? CreateCollectionEntity(childType, entities);

					if (item.Sortable && child is ISortableEntity)
					{
						var sortName = string.Format(CultureInfo.InvariantCulture, "{0}[{1}].Sort", propertyName, index);
						var sortValue = valueProvider.GetValue(sortName);
						if (sortValue != null)
						{
							var sort = (int) sortValue.ConvertTo(typeof (int));
							((ISortableEntity) child).Sort = sort;
						}
					}

					BindCollectionItemSimpleProperties(item, child, childType, propertyName, index, valueProvider);

					var aliasEntity = child as IHaveAliasEntity;
					if (aliasEntity != null)
					{
						var alias = aliasEntity.Alias;
						var name = aliasEntity.Name;

						if (string.IsNullOrEmpty(alias))
						{
							alias = name.Transliterate();
							aliasEntity.Alias = alias;
						}

					}

					BindCollectionItemListProperties(item, child, propertyName, index, valueProvider);
					index++;
				}

				foreach (var id in listToRemove) // удаление элементов
				{
					var child = list.First(e => e.Id == id);
					RemoveCollectionEntity(entity, entities, child);
				}
			}
		}

		[HttpPost]
		public virtual ActionResult RemoveFileCollectionItem(int imgSrc, int id, string propName, int parentId)
		{
			var fileService = DependencyResolver.Current.GetService<IFileService>();
			var entity = Repository.GetByPrimaryKey(id);
			var path = propName.Split('.');
			var parent = GetParent(entity, path, 0, parentId); // возвращает элемент коллекции innerCollection с id=parentId
			var fileprop = path[path.Length - 1]; // свойство в котором хранится файл
			var settings = Settings.GetFormSettingsItem(propName);

			var value = TypeHelpers.GetPropertyValue(parent, fileprop);
			if (value is IEnumerable)
			{
				var list = ((IListSource)value).GetList();
				var v = list.Cast<IFileEntity>().FirstOrDefault(f => f.Id == imgSrc);
				if (v != null)
				{
					fileService.DeleteFile(v, (UploadFileSettings) settings);
					Repository.DataContext.Set(v.GetType()).Remove(v);
				}
			}
			else if (value is IFileEntity)
			{
				var v = (IFileEntity) value;
				fileService.DeleteFile(v, (UploadFileSettings)settings);
				Repository.DataContext.Set(v.GetType()).Remove(v);
				TypeHelpers.SetPropertyValue(parent, fileprop, null);
			}
			Repository.Save();
			return Json(new
			{
				success = true
			});
		}

		[HttpPost]
		public virtual ActionResult UploadFileCollectionItem(FineUpload upload, int id, string propName, int parentId)
		{
			var fileService = DependencyResolver.Current.GetService<IFileService>();
			// id - айдишник корневого элемента, который мы сейчас редактируем
			// parentId - айдишник элемента, которому принадлежит файл (элемента в коллекции)
			// propName - полный путь к элементу и файлу (root.items.innerCollection.File, например)

			// каким-то образом мне надо добраться до элемента

			//var root = Repository.GetByPrimaryKey(id);
			//propName
			if (upload.InputStream != null && upload.InputStream.Length != 0)
			{
				try
				{
					var entity = Repository.GetByPrimaryKey(id);
					var path = propName.Split('.');
					var parent = GetParent(entity, path, 0, parentId); // возвращает элемент коллекции innerCollection с id=parentId
					var fileprop = path[path.Length - 1]; // свойство в котором хранится файл

					var value = TypeHelpers.GetPropertyValue(parent, fileprop);


					var result = new List<string>();

					var defaultPath = WebConfig.BasePathForImages;
					var filepath = Server.MapPath(defaultPath);
					var filename = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(upload.Filename));
					var fullname = Path.Combine(filepath, filename);

					if (value is IEnumerable)
					{
						var propType = TypeHelpers.GetPropertyType(parent, fileprop);
						var file = (IFileEntity)Activator.CreateInstance(propType);
						Repository.DataContext.Set(file.GetType()).Add(file);
						var list = ((IListSource)value).GetList();
						list.Add(file);
						var settings = Settings.GetFormSettingsItem(propName);
						fileService.SaveFile(upload.InputStream, (UploadFileSettings)settings, file, upload.Filename);

						var names = list.Cast<IFileEntity>().Select(fileService.GetUrl);
						result = names.ToList();

					}
					else
					{
						var file = (IFileEntity)value;
						var settings = Settings.GetFormSettingsItem(propName);
						if (file == null)
						{
							var propType = TypeHelpers.GetPropertyType(parent, fileprop);
							file = (IFileEntity)Activator.CreateInstance(propType);
							Repository.DataContext.Set(file.GetType()).Add(file);
							TypeHelpers.SetPropertyValue(parent, fileprop, file);
						}

						fileService.DeleteFile(file, null); // todo: будет работать только для локальных файлов
						fileService.SaveFile(upload.InputStream, (UploadFileSettings)settings, file, upload.Filename);
						result.Add(fileService.GetUrl(file));
					}
					Repository.Save();
					return new FineUploaderResult(true, new
					{
						Urls = result
					});
				}
				catch (Exception exception)
				{
					return new FineUploaderResult(false, null, exception.Message);
				}
			}
			return new FineUploaderResult(false, null, "файл не был передан");
		}

		private IEntity GetParent(object entity, string[] path, int index, int parentId)
		{
			var propertyValue = TypeHelpers.GetPropertyValue(entity, path[index]);
			var result = ((IList)propertyValue);
			if (path.Length == index + 2)
			{
				var parent = Enumerable.Cast<IEntity>(result).FirstOrDefault(e => e.Id == parentId);
				return parent;
			}
			return (from object element in result 
					select GetParent(element, path, index + 1, parentId)).FirstOrDefault(parent => parent != null);
		}

		protected virtual void BindCollectionItemListProperties(CollectionSettings settings, IEntity child, string prefix, int index, IValueProvider valueProvider)
		{
			var subPrefix = string.Format("{0}[{1}].", prefix, index);
			UpdateCollectionPropertiesRecursive(subPrefix, child, valueProvider, settings.Fields.Where(s => s is CollectionSettings).Cast<CollectionSettings>());
		}

		protected virtual void BindCollectionItemSimpleProperties(CollectionSettings settings, IEntity child, Type elementType, string prefix, int index, IValueProvider valueProvider)
		{
			foreach (var field in settings.Fields.Where(s => !(s is CollectionSettings)))
			{
				if (field is SelectSettings)
				{
					var select = (SelectSettings) field;
					if (!select.Editable && !select.Multiple) // самый обычный селект
					{
						var fullname = string.Format(CultureInfo.InvariantCulture, "{0}[{1}].{2}", prefix, index, field.Name);
						var value = valueProvider.GetValue(fullname);
						if (value == null)
							continue;
						SetProperty(value, child, field);
					}
					else if (!select.Editable && select.Multiple)
					{
						// обычная коллекция
					}
					else if (select.Editable && !select.Multiple)
					{
						// поиск элемента в бд, либо создание элемента, если такового не существует
						var linkType = TypeHelpers.GetPropertyType(child, select.Reference);
						//var list = TypeHelpers.GetEntitySet(Repository.DataContext, linkType).Cast<IEntity>(); // коллекция дочерних элементов из бд
						var fullPrefix = string.Format(CultureInfo.InvariantCulture, "{0}[{1}].{2}", prefix, index, select.Reference);
						var optionValue = valueProvider.GetValue(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", fullPrefix, select.OptionValue));
						var optionTitle = valueProvider.GetValue(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", fullPrefix, select.OptionTitle));
						if (optionValue == null || optionValue.RawValue == null || (string)optionValue.ConvertTo(typeof(string)) == "" || 0 == (int)optionValue.ConvertTo(typeof(int)))
						{
							var title = optionTitle.ConvertTo(typeof (string));
							// поиск среди только что добавленных, либо создание нового элемента
							var manager = ((IObjectContextAdapter) Repository.DataContext).ObjectContext.ObjectStateManager;
							var entities = manager.GetObjectStateEntries(EntityState.Added).Where(entry => entry.Entity != null).Select(entry => entry.Entity);
							var entity = entities.FirstOrDefault(entry =>
								{
									var a = entry.GetType() == linkType;
									if (!a)
										return false;
									var prop = TypeHelpers.GetPropertyValue(entry, select.OptionTitle);
									var b = prop != null && prop.Equals(title);
									return b;
								});
							if (entity == null)
							{
								entity = CreateReferencePropertyElement(linkType);

								TypeHelpers.SetPropertyValue(entity, select.OptionTitle, title);

								if (entity is IHaveAliasEntity)
								{
									UpdateAlias((IHaveAliasEntity)entity);
								}

								//Repository.DataContext.AddObject(TypeHelpers.GetEntitySetName(entity, Repository.DataContext), entity);
							}
							SetProperty(entity, child, select.Reference);
						}
						else
						{
							SetProperty(optionValue, child, select);
						}
					}
					else if (select.Editable && select.Multiple)
					{
						// парсинг результата и обработка его в контексте бд
					}
					// getorcreatesubchild (id, name)
					// setreferenceproperty (id or subchild)
				}
				else
				{
					var fullname = string.Format(CultureInfo.InvariantCulture, "{0}[{1}].{2}", prefix, index, field.Name);
					var value = valueProvider.GetValue(fullname);
					if (value == null)
						continue;
					SetProperty(value, child, field);
				}
			}
		}

		protected virtual object CreateReferencePropertyElement(Type type)
		{
			var result = Activator.CreateInstance(type);
			Repository.DataContext.Set(result.GetType()).Add(result);
			return result;
		}

		protected virtual void SetProperty(ValueProviderResult value, IEntity entity, FieldSettings field)
		{
			var propType = TypeHelpers.GetPropertyType(entity, field.Name);
			var newValue = value.ConvertTo(propType);
			SetProperty(newValue, entity, field.Name);
		}

		protected virtual void SetProperty(object value, IEntity entity, string field)
		{
			TypeHelpers.SetPropertyValue(entity, field, value);
		}

		protected virtual void RemoveCollectionEntity(IEntity entity, IList collectionProperty, IEntity child)
		{
			collectionProperty.Remove(child);
			Repository.DataContext.Set(child.GetType()).Remove(child);
		}

		protected virtual IEntity CreateCollectionEntity(Type type, IList collectionProperty)
		{
			var result = (IEntity)Activator.CreateInstance(type);
			collectionProperty.Add(result);
			return result;
		}

		private void UpdateEditableSelect(TEntity entity, FormCollection collection)
		{
			// TODO: не проверено
			foreach (var item in Settings.FormSettings.Fields.Where(settings => settings is SelectSettings).Cast<SelectSettings>().Where(s => !s.Multiple && s.Editable))
			{
				var type = TypeHelpers.GetPropertyType(entity, item.Name);
				var value = collection[item.Name + "." + item.OptionValue];
				IEntity s;
				if (string.IsNullOrEmpty(value))
				{
					s = (IEntity)Activator.CreateInstance(type);
					TypeHelpers.SetPropertyValue(s, item.OptionTitle, collection[item.Name + "." + item.OptionTitle]);
					if (s is IHaveAliasEntity)
					{
						UpdateAlias((IHaveAliasEntity)s);
					}

				}
				else
				{
					var dbList = DataModelContext.Set(type);
					var id = int.Parse(value);
					s = (IEntity)dbList.Find(id);
				}
				if (s != null)
				{
					TypeHelpers.SetPropertyValue(entity, item.Name, s);
				}
			}
		}

		private void UpdateEditableMultipleSelect(TEntity entity, FormCollection collection)
		{
			foreach (var item in Settings.FormSettings.Fields.Where(settings => settings is SelectSettings).Cast<SelectSettings>().Where(s => s.Multiple && s.Editable))
			{
				var type = TypeHelpers.GetPropertyType(entity, item.Name);
				var value = (IListSource) TypeHelpers.GetPropertyValue(entity, item.Name);
				var list = value.GetList();
				var values = collection[item.Name + "." + item.OptionTitle];
				// ищем по имени
				var names = values.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
				list.Clear();
				var dbList = DataModelContext.Set(type).Cast<IEntity>().ToList();
				foreach (var name in names.Where(s => !string.IsNullOrWhiteSpace(s)))
				{
					var n = name.Trim();
					var e = dbList.FirstOrDefault(el => n.ToLower().Equals(TypeHelpers.GetPropertyValue(el, item.OptionTitle).ToString(), StringComparison.InvariantCultureIgnoreCase));
					if (e == null)
					{
						e = (IEntity) Activator.CreateInstance(type);
						TypeHelpers.SetPropertyValue(e, item.OptionTitle, n);
						if (e is IHaveAliasEntity)
						{
							UpdateAlias((IHaveAliasEntity) e);
						}
					}
					list.Add(e);
				}
			}
		}

		private void UpdateMultipleSelects(TEntity entity, FormCollection collection)
		{
			foreach (var item in Settings.FormSettings.Fields.Where(settings => settings is SelectSettings).Cast<SelectSettings>().Where(s => s.Multiple && !s.Editable))
			{
				var type = TypeHelpers.GetPropertyType(entity, item.Name);
				var value = (IList) TypeHelpers.GetPropertyValue(entity, item.Name);
				var list = value;//.GetList();
				var values = collection[item.Name];
				var ids = values == null ? new int[0] : values.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
				list.Clear();
				foreach (var id in ids)
				{
					var v = (IEntity) Repository.DataContext.Set(type).Find(id);
					if (v == null)
					{
						v = (IEntity) Activator.CreateInstance(type);
						v.Id = id;
						Repository.DataContext.Set(type).Attach(v);
					}
					list.Add(v);
					//var entitySetName = TypeHelpers.GetEntitySetName(type, Repository.DataContext);
					//var v = (IEntity) Repository.DataContext.GetObjectByKey(new EntityKey(entitySetName, new[] {new EntityKeyMember("Id", id)}));
					//if (v == null)
					//{
					//	v = (IEntity) Activator.CreateInstance(type);
					//	v.Id = id;
					//	Repository.DataContext.AttachTo(TypeHelpers.GetEntitySetName(v, Repository.DataContext), v);
					//}
					//list.Add(v);
				}
				ModelState.Remove(item.Name);
			}
		}
    }
}
