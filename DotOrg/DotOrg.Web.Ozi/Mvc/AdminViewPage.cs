using System.Web.Mvc;
using DotOrg.Web.Ozi.Mvc;
using DotOrg.Web.Mvc;

namespace DotOrg.Web.Ozi.Mvc
{
	public abstract class AdminViewPage<TModel> : BaseWebPage<TModel>
	{
		public AdminWebContext WebContext { get { return DependencyResolver.Current.GetService<AdminWebContext>(); } }

		public class Actions
		{
			public static string SendEMail { get { return "SendEMail"; } }
			public static string Index { get { return "Index"; } }
			public static string Edit { get { return "Edit"; } }
			public static string UploadFile { get { return "UploadFile"; } }
			public static string UploadFileCollectionItem { get { return "UploadFileCollectionItem"; } }
			public static string RemoveFileCollectionItem { get { return "RemoveFileCollectionItem"; } }
			public static string SaveFileData { get { return "SaveFileData"; } }
			public static string DeleteFile { get { return "DeleteFile"; } }
			public static string SaveOrder { get { return "SaveOrder"; } }
		}
	}

	public abstract class AdminViewPage : AdminViewPage<object>
	{
	}
}