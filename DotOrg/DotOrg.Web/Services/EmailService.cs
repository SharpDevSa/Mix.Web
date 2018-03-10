using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using DotOrg.Web.Models;

namespace DotOrg.Web.Services
{
	public class EmailService
	{
		private static readonly object LockObject = new object();

		public static bool SendMail(SendMailViewModel model, ControllerContext controllerContext)
		{
			var logger = DependencyResolver.Current.GetService<IEmailLogService>();
			try
			{
				var message = new MailMessage();

				var toes = model.To.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
				foreach (var to in toes)
				{
					message.To.Add(to);
				}
				message.From = model.FromAddress;
				message.BodyEncoding = System.Text.Encoding.UTF8;
				if (!string.IsNullOrEmpty(model.Bcc))
				{
					toes = model.Bcc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var to in toes)
					{
						message.Bcc.Add(to);
					}
				}
				message.Subject = model.Subject;

				if (model.Attachments != null && model.Attachments.Any())
				{
					foreach (var attachment in model.Attachments)
					{
						if (File.Exists(attachment))
						{
							message.Attachments.Add(new Attachment(attachment));
						}
					}
				}

				if (controllerContext != null)
				{
					message.IsBodyHtml = true;
					var viewData = new ViewDataDictionary(model.ViewModel);
					message.Body = ViewHelpers.RenderPartialViewToString(model.TemplateName, viewData, controllerContext);
				}
				else
				{
					message.IsBodyHtml = false;
					message.Body = model.Body;
				}
				lock (LockObject)
				{
					var client = new SmtpClient();
					if (client.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
					{
						client.EnableSsl = false;
					}
					System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
					client.Send(message);
				}
				logger.Write(model.From, model.To, model.Subject, message.Body, null);
				return true;
			}
			catch (Exception exception)
			{
				logger.Write(model.From, model.To, model.Subject, Newtonsoft.Json.JsonConvert.SerializeObject(model), exception);
				//using (var writer = new StreamWriter(HttpContext.Current.Server.MapPath(string.Format("~/Logs/Mail/{0:yyyyMMdd}.txt", DateTime.Now)), true))
				//{
				//	writer.WriteLine("{0:D} -- {1} -- {2}", DateTime.Now, exception.Message, exception.GetType());
				//}
				return false;
			}
		}
	}

	public interface IEmailLogService
	{
		void Write(string from, string to, string subject, string content, Exception exception);
	}
}
