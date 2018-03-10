using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotOrg.Web.Ozi.ViewModels.Accounts
{
	public class ChangePasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Current password")]
		public string OldPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("New password")]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm new password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}