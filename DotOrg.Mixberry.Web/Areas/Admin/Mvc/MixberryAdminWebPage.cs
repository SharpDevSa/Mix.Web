using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Mixberry.Web.Areas.Admin.Mvc
{
	public abstract class MixberryAdminViewPage<TModel> : AdminViewPage<TModel>
	{
		public ILocalizationContext Localization { get { return DependencyResolver.Current.GetService<ILocalizationContext>(); } }
	}

	public abstract class MixberryAdminViewPage : MixberryAdminViewPage<object>
	{
	}
}