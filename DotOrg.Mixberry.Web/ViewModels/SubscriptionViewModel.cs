using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace DotOrg.Mixberry.Web.ViewModels
{
	public class SubscriptionViewModel
	{
		[Required]
		[Email]
		public string Email { get; set; }
	}
}