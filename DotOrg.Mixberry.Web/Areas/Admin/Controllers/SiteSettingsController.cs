using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Db.Models;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Web.Core;
using DotOrg.Web.Ozi.Controllers;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	[Authorize]
	public partial class SiteSettingsController : OziController
	{
		private readonly DbContext _db;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var localization = DependencyResolver.Current.GetService<ILocalizationContext>();
			localization.ChangeUrl = lang =>
			{
				var url = Request.RawUrl;
				var result = LocalizationContext.ReplaceLangInUrl(url, lang);
				return result;
			};

		}

		public SiteSettingsController(DbContext db)
		{
			_db = db;
		}

		public virtual ActionResult Emails()
		{
			var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
			var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
			Debug.Assert(settings != null);
			return View(settings.Smtp);
		}

		[HttpPost]
		public virtual ActionResult Emails(SmtpSection model)
		{
			var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
			var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
			if (settings != null)
			{
				if (model.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					settings.Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					settings.Smtp.Network.ClientDomain = model.Network.ClientDomain;
					settings.Smtp.Network.DefaultCredentials = model.Network.DefaultCredentials;
					settings.Smtp.Network.EnableSsl = model.Network.EnableSsl;
					settings.Smtp.Network.Host = model.Network.Host;
					settings.Smtp.Network.Password = model.Network.Password;
					settings.Smtp.Network.Port = model.Network.Port;
					settings.Smtp.Network.UserName = model.Network.UserName;
				}
				else if (model.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
				{
					settings.Smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					settings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation = model.SpecifiedPickupDirectory.PickupDirectoryLocation;
				}
			}
			config.Save();
			return View(model);
		}

		public virtual ActionResult Logs()
		{
			var list = _db.Set<EmailLog>().OrderByDescending(log => log.Date).ToList();
			return View(list);
		}

		public virtual ActionResult General()
		{
			var model = _db.Set<SiteSetting>().ToList();
			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		public virtual ActionResult General(FormCollection values)
		{
			var settings = _db.Set<SiteSetting>().ToList();
			foreach (string key in values.Keys)
			{
				var value = values[key];
				var s = settings.FirstOrDefault(setting => setting.Name == key);
				if (s == null)
					continue;
				s.Value = value;
			}
			_db.SaveChanges();
			return RedirectToAction(MVC.Admin.SiteSettings.General());
		}
	}
}
