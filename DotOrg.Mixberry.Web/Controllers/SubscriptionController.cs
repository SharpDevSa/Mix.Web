using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotOrg.Db;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Mvc;
using DotOrg.Mixberry.Web.ViewModels;
using DotOrg.Web.Models;
using DotOrg.Web.Services;

namespace DotOrg.Mixberry.Web.Controllers
{
	public partial class SubscriptionController : BaseController
	{
		private readonly IDbFactory _factory;

		public SubscriptionController(IDbFactory factory)
		{
			_factory = factory;
		}

		[ChildActionOnly]
		public virtual ActionResult SubscriptionForm()
		{
			var model = new SubscriptionViewModel();
			return PartialView(model);
		}

		[HttpPost]
		public virtual ActionResult Subscribe(SubscriptionViewModel model)
		{
			if (ModelState.IsValid)
			{
				var item = GetEntities<Subscriber>().FirstOrDefault(x => x.Email == model.Email);
				if (item != null)
				{
					if (!item.IsApprowed)
					{
						/* send email? */
					}
				}
				else
				{
					item = new Subscriber
					{
						Date = DateTime.Now,
						Email = model.Email,
						Url = HttpContext.Request.UrlReferrer.ToString(),
						IsApprowed = false,
						Key = Guid.NewGuid().ToString().Replace("-", "")
					};
					using (var db = _factory.Create())
					{
						db.Set<Subscriber>().Add(item);
						db.SaveChanges();
					}
					EmailService.SendMail(new SendMailViewModel
					{
						From = AppConfig.GetValue("Emails.Subscribers.From"),
						SenderName = AppConfig.GetValue("Emails.Subscribers.SenderName"),
						Subject = AppConfig.GetValue("Emails.Subscribers.Subject"),
						TemplateName = MVC.Mails.Views.SubscriptionConfirm,
						To = model.Email,
						ViewModel = item
					}, ControllerContext);
				}
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		public virtual ActionResult Confirm(string key)
		{
			using (var db = _factory.Create())
			{
				var item = db.Set<Subscriber>().FirstOrDefault(x => x.Key == key);
				if (item != null)
				{
					item.IsApprowed = true;
					db.SaveChanges();
				}
			}
			return View();

		}

		public virtual ActionResult Unsubscribe(string key)
		{
			using (var db = _factory.Create())
			{
				var item = db.Set<Subscriber>().FirstOrDefault(x => x.Key == key);
				if (item != null)
				{
					db.Set<Subscriber>().Remove(item);
					db.SaveChanges();
				}
			}
			return View();
		}
	}
}