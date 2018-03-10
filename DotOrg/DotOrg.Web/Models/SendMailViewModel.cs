using System.Collections.Generic;
using System.Net.Mail;

namespace DotOrg.Web.Models
{
	public class SendMailViewModel
	{
		public SendMailViewModel()
		{
			Attachments = new List<string>();
		}

		public string To { get; set; }
		public string Bcc { get; set; }
		public string From { get; set; }
		public string SenderName { get; set; }
		public string Subject { get; set; }

		public MailAddress FromAddress
		{
			get { return new MailAddress(From, SenderName); }
		}

		public List<string> Attachments { get; set; }

		public string Body { get; set; }

		public dynamic ViewModel { get; set; }
		public string TemplateName { get; set; }
	}
}
