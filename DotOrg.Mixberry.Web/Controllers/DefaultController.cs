using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Mvc;
using DotOrg.Mixberry.Web.Core;

namespace DotOrg.Mixberry.Web.Controllers
{
	
	public partial class DefaultController : BaseController
    {
		public virtual ActionResult Page()
		{
			var model = GetById<Location>(WebContext.Location.Id);
			SetLocalizationRedirects();
			if (model == null)
				return HttpNotFound();
			DataModelContext.Entry(model).Collection(x => x.Blocks).Load();
            //var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
            RenderHomePageDb dbf = new RenderHomePageDb();
            WebContext.Location = dbf.RenderModelHome(WebContext.Location, WebContext.Location.Id);
            return View(WebContext.Location);
		}

		public virtual ActionResult Reset()
		{
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			lang.ResetStaticCache();
			return HttpNotFound();
		}
    }

}