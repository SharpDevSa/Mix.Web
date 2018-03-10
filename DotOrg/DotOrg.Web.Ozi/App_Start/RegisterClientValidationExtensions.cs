using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(DotOrg.Web.Ozi.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace DotOrg.Web.Ozi.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}