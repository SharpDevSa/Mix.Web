using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotOrg.Web.Ozi.ViewModels.Accounts
{
	public class RegisterModel
	{
		[Required]
		[DisplayName("User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email address")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}