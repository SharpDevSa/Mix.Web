using System.Data.Entity;
using System.Web.Mvc;
using DotOrg.Web.Ozi.Mvc;
using DotOrg.Web.Mvc;

namespace DotOrg.Web.Ozi.Controllers
{
    public abstract class OziController : Controller
    {
		protected bool IsPjax
		{
			get { return Request.Headers["X-PJAX"] != null; }
		}

		public virtual DbContext DataModelContext { get { return null; } }

		[NonAction]
		protected virtual ActionResult ToIndex()
		{
			return RedirectToAction(Constants.IndexView);
		}


		[NonAction]
		protected ActionResult Back()
		{
			if (Request.UrlReferrer == null)
				return ToIndex();
			return Redirect(Request.UrlReferrer.AbsoluteUri);
		}
    }
}
