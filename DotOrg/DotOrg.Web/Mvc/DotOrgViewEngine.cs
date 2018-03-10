using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using DotOrg.Core.Helpers;

namespace DotOrg.Web.Mvc
{
	public class DotOrgViewEngine : RazorViewEngine
	{
		public DotOrgViewEngine()
			: this(null)
		{

		}

		public DotOrgViewEngine(IViewPageActivator viewPageActivator)
			: base(viewPageActivator)
		{
			var baseViewLocationsFormat = ViewLocationFormats;

			var newLocationFormats = new[]
				{
					"~/Views/Shared/{{TYPE}}/{{ALIAS}}.cshtml"
				};

			var viewLocations = new List<string>();
			viewLocations.AddRange(newLocationFormats);
			viewLocations.AddRange(baseViewLocationsFormat);
			ViewLocationFormats = viewLocations.ToArray();

			var basePartialFormats = PartialViewLocationFormats;
			viewLocations = new List<string>();
			viewLocations.AddRange(newLocationFormats);
			viewLocations.AddRange(basePartialFormats);
			PartialViewLocationFormats = viewLocations.ToArray();
			ViewLocationCache = new FakeViewCache(ViewLocationCache);
		}

		protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
		{
			return base.CreateView(controllerContext, ChangePath(viewPath, controllerContext.Controller.ViewData.Model), masterPath);
		}

		protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
		{
			return base.CreatePartialView(controllerContext, ChangePath(partialPath, controllerContext.Controller.ViewData.Model));
		}

		protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
		{
			return base.FileExists(controllerContext, ChangePath(virtualPath, controllerContext.Controller.ViewData.Model));
		}

		private string ChangePath(string virtualPath, object model)
		{
			if (model != null)
			{
				string typename;
				string alias;

				var attr = model.GetType().GetCustomAttribute(typeof(CustomViewAttribute), true);
				if (attr != null)
				{
					var viewAttribute = ((CustomViewAttribute)attr);
					typename = viewAttribute.TypeName;
					alias = model.GetPropertyValue<string>(viewAttribute.AliasName);
				}
				else
				{
					alias = model.GetPropertyValue<string>("alias");
					typename = model.GetTypeName();
				}
				if (!string.IsNullOrEmpty(alias) && !string.IsNullOrEmpty(typename))
				{
					return virtualPath.Replace("{TYPE}", typename).Replace("{ALIAS}", alias);
				}
			}
			return virtualPath;
		}
	}
}
