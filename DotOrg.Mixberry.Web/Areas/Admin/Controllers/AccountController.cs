using System.Web.Mvc;
using System.Web.Security;
using DotOrg.Core.Helpers;
using DotOrg.Web.Ozi.Controllers;
using DotOrg.Web.Ozi.ViewModels.Accounts;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	[HandleError]
	[Authorize]
	public partial class AccountController : OziController
	{
		public virtual ActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public virtual ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = Membership.GetUser();
			if (user == null)
				return RedirectToAction(MVC.Admin.Account.Login());
			var result = user.ChangePassword(model.OldPassword, model.NewPassword);
			if (result)
			{
				return RedirectToAction(MVC.Admin.Locations.Index());
			}
			return View(model);
		}

		[AllowAnonymous]
		public virtual ActionResult Login(string returnUrl)
		{
			return View(new LogOnModel());
		}

		[HttpPost]
		[AllowAnonymous]
		public virtual ActionResult Login(LogOnModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = Membership.ValidateUser(model.UserName, model.Password);
			if (result)
			{
				FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
				if (!returnUrl.IsNullOrEmpty())
				{
					return Redirect(returnUrl);
				}
				return RedirectToAction(MVC.Admin.Locations.Index());
			}
			return View();
		}

		public virtual ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction(MVC.Admin.Account.Login());
		}
	}
}
