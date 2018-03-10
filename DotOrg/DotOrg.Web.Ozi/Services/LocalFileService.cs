using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotOrg.Core.Helpers;
using DotOrg.Db.Entities;
using DotOrg.Web.Ozi.PagesSettings.Forms;

namespace DotOrg.Web.Ozi.Services
{
	public class LocalFileService : IFileService
	{
		public void SaveFile(Stream stream, UploadFileSettings context, IFileEntity file, string sourceName)
		{
			var size = stream.Length;
			var name = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(sourceName));
			var relativeName = Path.Combine(context.PathToSave, name);
			var fullname = HttpContext.Current.Server.MapPath(relativeName);
			file.Size = size;
			file.Date = DateTime.Now;
			file.SourceName = sourceName;
			file.Url = relativeName;
			using (var fileStream = new FileStream(fullname, FileMode.CreateNew))
			{
				stream.CopyTo(fileStream);
			}
		}

	public void DeleteFile(IFileEntity file, UploadFileSettings context)
		{
			try
			{
				//var fullname = HttpContext.Current.Server.MapPath(file.Url);
				//if (File.Exists(fullname))
				//{
				//	File.Delete(fullname);
				//}
			}
			catch
			{
			}
		}

		public string GetThumbUrl(IFileEntity file, params object[] parameters)
		{
			var imgParams = string.Join("&", parameters);
			var url = "~/content/nophoto.jpg";
			if (file != null)
			{
				url = file.Url;
			}
			return UrlHelper.GenerateContentUrl(string.Join("?", url, imgParams), new HttpContextWrapper(HttpContext.Current));
			//var context = new HttpContextWrapper(HttpContext.Current);
			//if (width == 0 && height == 0)
			//{
			//	return UrlHelper.GenerateContentUrl(file.Url, context);
			//}

			//var thumb = file.Thumbs.FirstOrDefault(t => t.Width == width && t.Height == height && t.FillType == fill);
			//if (thumb != null)
			//{
			//	return UrlHelper.GenerateContentUrl(thumb.Url, context);
			//}

			//// create thumb
			//var localPath = context.Server.MapPath(file.Url);
			//if (!File.Exists(localPath))
			//{
			//	return null;
			//}

			//throw new NotImplementedException();
		}

		public string GetUrl(IFileEntity file)
		{
			if (file.Url.IsNullOrEmpty())
				return string.Empty;
			if (!file.Url.StartsWith("~/"))
				return file.Url;
			var context = new HttpContextWrapper(HttpContext.Current);
			return UrlHelper.GenerateContentUrl(file.Url, context);
		}

		//private static string GetImageUrl(string name, int width, int height, bool proportional)
		//{
		//	return HttpContext.Current.Server.MapPath(name);
		//}

		//private static void DeleteFile(IFileEntity file, HttpContextBase context)
		//{
		//	if (file == null)
		//		return;
		//	try
		//	{
		//		var fullname = context.Server.MapPath(file.Name);
		//		var path = Path.GetDirectoryName(fullname);
		//		var filename = Path.GetFileNameWithoutExtension(fullname);
		//		var ext = Path.GetExtension(fullname);
		//		var mask = string.Format("{0}*{1}", filename, ext);
		//		var files = Directory.GetFiles(path, mask);
		//		foreach (var f in files)
		//		{
		//			try
		//			{
		//				File.Delete(f);
		//			}
		//			catch
		//			{
		//			}
		//		}
		//	}
		//	catch
		//	{
		//	}
		//}

		//private static void ResaveImage(FineUpload upload, string filename, UploadFileSettings settings)
		//{
		//	// в некоторые типы картинок можно запихать код для исполнения, чтобы пресечь это, необходимо пересохранить кратинку
		//	using (var img = Image.FromStream(upload.InputStream))
		//	{

		//		// подготовка картинки к сохраниению, рисуем ватермарки и тыды
		//		if (settings.IsImage)
		//		{
		//			if (settings.Watermarks.Any(watermarkSettings => watermarkSettings.FillType != WatermarkFillType.None))
		//			{
		//				using (var g = Graphics.FromImage(img))
		//				{
		//					foreach (var wm in settings.Watermarks)
		//					{
		//						using (var wmImage = Image.FromFile(HttpContext.Current.Server.MapPath(wm.RelativePath)))
		//						{
		//							var k = 1.0;
		//							if ((wm.ReduceWidth.HasValue && img.Width <= wm.ReduceWidth.Value) || (wm.ReduceHeight.HasValue && img.Height <= wm.ReduceHeight.Value))
		//							{
		//								k = wm.ReduceFactor;
		//							}
		//							var width = wmImage.Width / k;
		//							var height = wmImage.Height / k;
		//							if (wm.FillType == WatermarkFillType.Mosaic)
		//							{
		//								var cY = 0;
		//								while (cY < img.Height)
		//								{
		//									var cX = 0;
		//									while (cX < img.Width)
		//									{
		//										g.DrawImage(wmImage, new Rectangle(cX, cY, (int)width, (int)height));
		//										cX += wmImage.Width;
		//									}
		//									cY += wmImage.Height;
		//								}
		//							}
		//							else
		//							{
		//								// координаты на исходном изображении куда выводить ватермарк
		//								var left = 0.0;
		//								var top = 0.0;
		//								switch (wm.FillType)
		//								{
		//									case WatermarkFillType.TopLeft:
		//										left = wm.Margins.Left / k;
		//										top = wm.Margins.Top / k;
		//										break;
		//									case WatermarkFillType.TopRight:
		//										left = img.Width - wm.Margins.Right / k - width;
		//										top = wm.Margins.Top / k;
		//										break;
		//									case WatermarkFillType.BottomLeft:
		//										left = wm.Margins.Left / k;
		//										top = img.Height - wm.Margins.Bottom / k - height;
		//										break;
		//									case WatermarkFillType.BottomRight:
		//										left = img.Width - wm.Margins.Right / k - width;
		//										top = img.Height - wm.Margins.Bottom / k - height;
		//										break;
		//									case WatermarkFillType.Center:
		//										left = (img.Width - width)/2;
		//										top = (img.Height - height)/2;
		//										break;
		//									case WatermarkFillType.Stretch:
		//										left = wm.Margins.Left / k;
		//										top = wm.Margins.Top / k;
		//										width = img.Width - wm.Margins.Right / k;
		//										height = img.Height - wm.Margins.Bottom / k;
		//										break;
		//									case WatermarkFillType.Custom:
		//										left = wm.Left + wm.Margins.Left;
		//										top = wm.Top + wm.Margins.Top;
		//										width = wm.Width == 0 ? wmImage.Width : wm.Width;
		//										height = wm.Height == 0 ? wmImage.Height : wm.Height;
		//										break;
		//									default:
		//										throw new ArgumentOutOfRangeException();
		//								}
		//								g.DrawImage(wmImage, new Rectangle((int)left, (int)top, (int)width, (int)height));
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}

		//		var ext = Path.GetExtension(filename);
		//		if (".jpg".Equals(ext, StringComparison.InvariantCultureIgnoreCase) || ".jpeg".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
		//		{
		//			SaveJpg(img, filename);
		//		}
		//		else if (".gif".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
		//		{
		//			SaveGif(img, filename);
		//		}
		//		else if (".png".Equals(ext, StringComparison.InvariantCultureIgnoreCase))
		//		{
		//			SavePng(img, filename);
		//		}
		//	}
		//}

		//private static void SavePng(Image img, string filename)
		//{
		//	img.Save(filename, ImageFormat.Png);
		//}

		//private static void SaveGif(Image img, string filename)
		//{
		//	img.Save(filename, ImageFormat.Gif);
		//}

		//private static void SaveJpg(Image img, string filename)
		//{
		//	var jgpEncoder = GetEncoder(ImageFormat.Jpeg);
		//	var myEncoder = Encoder.Quality;
		//	var myEncoderParameters = new EncoderParameters(1);
		//	var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
		//	myEncoderParameters.Param[0] = myEncoderParameter;
		//	img.Save(filename, jgpEncoder, myEncoderParameters);
		//}

		//private static ImageCodecInfo GetEncoder(ImageFormat format)
		//{
		//	var codecs = ImageCodecInfo.GetImageDecoders();
		//	return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
		//}

		////public void SaveFiles(ObjectContext dbContext, ControllerContext controllerContext, IEntity entity, UploadFileSettings imageSettings)
		////{
		////	HttpRequestBase request = controllerContext.HttpContext.Request;
		////	Type fileType = TypeHelpers.GetPropertyType(entity, imageSettings.Name);
		////	string entitysetname = DbHelper.GetEntitySetName(dbContext, fileType);
		////	if (typeof (IEnumerable).IsAssignableFrom(fileType))
		////	{
		////		fileType = fileType.GetGenericArguments()[0];
		////	}
		////	var fileData = new FileEntityFormData
		////		{
		////			EntitySetName = entitysetname,
		////			Context = controllerContext,
		////			DbContext = dbContext,
		////			Entity = entity,
		////			Settings = imageSettings,
		////			FileType = fileType
		////		};
		////	foreach (string formKey in request.Form.AllKeys.Where(formKey => formKey.StartsWith(fileData.Settings.Name + "hidden")))
		////	{
		////		SetFormData(controllerContext.HttpContext.Request, formKey, fileData);
		////		object entityFileValue = TypeHelpers.GetPropertyValue(entity, imageSettings.Name);
		////		if (entityFileValue is IEnumerable)
		////		{
		////			ProcessItemsCollection(fileData, ((IListSource) entityFileValue).GetList());
		////		}
		////		else
		////		{
		////			ProcessItem(fileData, (IFileEntity) entityFileValue);
		////		}
		////	}
		////}

		////private static void ProcessItemsCollection(FileEntityFormData data, IList fileEntityCollection)
		////{
		////	IFileEntity fileEntity = null;
		////	if (!data.IsNew)
		////	{
		////		Debug.Assert(data.Id.HasValue);
		////		fileEntity = fileEntityCollection.Cast<IFileEntity>().FirstOrDefault(entity => entity.Id == data.Id.Value);
		////	}
		////	IFileEntity file = ProcessItemInternal(data, fileEntity);
		////	if (file == null)
		////	{
		////		if (fileEntity != null)
		////			fileEntityCollection.Remove(fileEntity);
		////	}
		////	else
		////	{
		////		fileEntityCollection.Add(file);
		////	}
		////}

		////private void ProcessItem(FileEntityFormData data, IFileEntity fileEntity)
		////{
		////	fileEntity = ProcessItemInternal(data, fileEntity);
		////	TypeHelpers.SetPropertyValue(data.Entity, data.Settings.Name, fileEntity);
		////}

		////private static IFileEntity ProcessItemInternal(FileEntityFormData data, IFileEntity fileEntity)
		////{
		////	if ((data.Deleted && (fileEntity == null || data.IsNew)) || (data.IsNew && !data.HasFile))
		////		return null; // либо нечего удалять, либо нечего сохранять
		////	if (data.Deleted)
		////	{
		////		DeleteEntityAndFile(data, fileEntity); // файл не новый и значение у него уже есть
		////		return null;
		////	}
		////	if (fileEntity == null)
		////	{
		////		fileEntity = (IFileEntity) Activator.CreateInstance(data.FileType);
		////		data.DbContext.AddObject(data.EntitySetName, fileEntity);
		////	}
		////	PopulateProperties(data, fileEntity);
		////	return fileEntity;
		////}

		////private static void PopulateProperties(FileEntityFormData data, IFileEntity fileEntity)
		////{
		////	if (data.HasFile)
		////	{
		////		DeletePhysicalFile(data.Context.HttpContext, fileEntity);
		////		fileEntity.SourceName = data.File.FileName;
		////		//fileEntity.RelativeFilename = GenerateFileName(data.File.FileName, data.Settings);
		////		//fileEntity.Size = data.File.ContentLength;
		////		//fileEntity.Changed = DateTime.Now;
		////		//string path = fileEntity.GenerateRelativeUrl(data.Context.HttpContext);
		////		//data.File.SaveAs(data.Context.HttpContext.Server.MapPath(path));
		////	}
		////}

		////private static string GenerateFileName(string filename, UploadFileSettings settings)
		////{
		////	var info = new FileInfo(filename);
		////	string newName = string.Format("{0}{1}", Guid.NewGuid(), info.Extension);
		////	return Path.Combine(settings.PathToSave, newName);
		////}

		////private static void DeleteEntityAndFile(FileEntityFormData data, IFileEntity fileEntity)
		////{
		////	//if (!string.IsNullOrEmpty(data.Settings.Reference))
		////	//{
		////	//	var parents = ((IEnumerable)TypeHelpers.GetPropertyValue(fileEntity, data.Settings.Reference)).Cast<IEntity>();
		////	//	if (parents.Count() > 1)
		////	//	{
		////	//		return;
		////	//	}
		////	//}
		////	DeletePhysicalFile(data.Context.HttpContext, fileEntity);
		////	data.DbContext.DeleteObject(fileEntity);
		////}

		////private static void DeletePhysicalFile(HttpContextBase context, IFileEntity fileEntity)
		////{
		////	//if (string.IsNullOrEmpty(fileEntity.RelativeFilename))
		////	//	return;
		////	//string relativeurl = fileEntity.GenerateRelativeUrl(context);
		////	//string fullname = context.Server.MapPath(relativeurl);
		////	//try
		////	//{
		////	//	if (File.Exists(fullname))
		////	//	{
		////	//		File.Delete(fullname);
		////	//	}
		////	//}
		////	//catch (Exception exception)
		////	//{
		////	//	Logger.Instance.LogException(exception, "Не удалось удалить физический файл, при удалении логического из базы данных");
		////	//}
		////}

		////private static void SetFormData(HttpRequestBase request, string baseKey, FileEntityFormData data)
		////{
		////	data.Key = baseKey.Replace(data.Settings.Name + "hidden", string.Empty);
		////	if (!data.Key.StartsWith("_"))
		////	{
		////		data.Id = int.Parse(data.Key);
		////	}
		////	else
		////	{
		////		data.Id = null;
		////	}
		////	data.Deleted = CommonHelpers.ParseCheckbox(request.Form[data.Settings.Name + data.Key + "_delete"]);
		////	data.File = request.Files[data.Settings.Name + data.Key];
		////}

		////private class FileEntityFormData
		////{
		////	public int? Id { get; set; }
		////	public string Key { get; set; }

		////	public bool IsNew
		////	{
		////		get { return !Id.HasValue || Id.Value == 0; }
		////	}

		////	public bool HasFile
		////	{
		////		get { return File != null && File.ContentLength > 0; }
		////	}

		////	public HttpPostedFileBase File { get; set; }
		////	public bool Deleted { get; set; }

		////	public ObjectContext DbContext { get; set; }
		////	public ControllerContext Context { get; set; }
		////	public IEntity Entity { get; set; }
		////	public UploadFileSettings Settings { get; set; }

		////	public Type FileType { get; set; }

		////	public string EntitySetName { get; set; }
		////}
		//public string SaveFile(Stream stream, SaveFileContext context)
		//{
			
		//}

		//public void DeleteFile(string filename)
		//{
			
		//}
	}
}