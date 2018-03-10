using DataAnnotationsExtensions.ClientValidation;
using DotOrg.Mixberry.Web.Startup;

[assembly: WebActivator.PreApplicationStartMethod(typeof(RegisterClientValidationExtensions), "Start")]

namespace DotOrg.Mixberry.Web.Startup
{
	public static class RegisterClientValidationExtensions
	{
		public static void Start()
		{
			DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
		}
	}
}