using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotOrg.Db.Entities;
using DotOrg.Web.Mvc;
using DotOrg.Web.Ozi.PagesSettings;
using DotOrg.Web.Ozi.PagesSettings.Forms;
using DotOrg.Web.Ozi.PagesSettings.Lists;
using DotOrg.Web.Ozi.ViewModels;

namespace DotOrg.Web.Ozi.Mvc
{
	public class AdminWebContext
	{
		public AdminWebContext()
		{
			IncludeCodeScripts = new List<string>();
		}
		private Settings _settings;
		private string _returnUrl;

		public string ReturnUrl
		{
			get { return _returnUrl ?? HttpContext.Current.Request["returnUrl"] ?? HttpContext.Current.Request["ozi_backlink"]; }
			set { _returnUrl = value; }
		}

		public bool IsCreate { get; set; }

		public string EditViewName { get; set; }

		public string ControllerName { get; set; }

		public Settings GetSettings(Type controllerType = null)
		{
			if (controllerType == null)
				return _settings;
			return _settings ?? (_settings = new Settings(GetSettingsName(controllerType)));
		}

		private string GetSettingsName(Type controllerType)
		{
			Debug.Assert(controllerType.BaseType != null);
			return controllerType.BaseType.GetGenericArguments()[0].Name;
		}

		private ViewDataDictionary _viewData;
		private DynamicViewDataDictionary _dynamicViewData;

		public dynamic ViewBag
		{
			get { return _dynamicViewData ?? (_dynamicViewData = new DynamicViewDataDictionary(() => ViewData)); }
		}

		protected virtual void SetViewData(ViewDataDictionary viewData)
		{
			_viewData = viewData;
		}

		public ViewDataDictionary ViewData
		{
			get
			{
				if (_viewData == null)
				{
					SetViewData(new ViewDataDictionary());
				}
				return _viewData;
			}
			set { SetViewData(value); }
		}
        public int? PrevId { get; set; }
        public int? NextId { get; set; }

		public FieldSettings FieldSettings { get; set; }
		public ColSettings ColSettings { get; set; }

		public TabsSettings CurrentTab { get; set; }

		public string HtmlPageTitle { get; set; }

		public IEntity Model { get; set; }

		public Type ModelType { get; set; }

		private IEnumerable<string> _roles;
		public bool IsUserInAnyRole(string allowedRoles)
		{
			if (string.IsNullOrEmpty(allowedRoles))
				return true;
			var roles = _roles ?? (_roles = allowedRoles.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));
			var uRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
			return roles.Intersect(uRoles).Any();
		}

		public List<string> IncludeCodeScripts { get; set; }
		public bool IncludeColorScript { get; set; }
		public bool IncludeGoogleMapScript { get; set; }
	}
}