using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotOrg.Web.Ozi.ViewModels.Accounts
{
	public class LogOnModel
	{
		[Required(ErrorMessage = "Укажите имя пользователя")]
		[DisplayName("Имя пользователя")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Укажите пароль")]
		[DataType(DataType.Password)]
		[DisplayName("Пароль")]
		public string Password { get; set; }

		[DisplayName("Запомнить?")]
		public bool RememberMe { get; set; }
	}
}