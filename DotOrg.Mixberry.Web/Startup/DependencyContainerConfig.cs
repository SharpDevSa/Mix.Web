using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using DotOrg.Db;
using DotOrg.Libs.Services;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Web.Core;
using DotOrg.Mixberry.Web.Core.Routes;
using DotOrg.Mixberry.Web.Core.Templating;
using DotOrg.Web.Ozi.Mvc;
using DotOrg.Web.Ozi.Services;
using DotOrg.Web.Services;

namespace DotOrg.Mixberry.Web.Startup
{
	public static class DependencyContainerConfig
	{
		public static void RegisterDependencies()
		{
			var builder = new ContainerBuilder();
			var assembly = Assembly.GetExecutingAssembly();
			builder.RegisterControllers(assembly);
			//builder.RegisterModelBinders(assembly);
			//builder.RegisterModelBinderProvider();
			builder.RegisterModule<AutofacWebTypesModule>();

			BuildSolutionDependencies(builder);

			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}

		private static void BuildSolutionDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<MixberryDbContext>().As<DbContext>().InstancePerRequest();
			builder.RegisterType<WebContext>().As<IWebContext>().InstancePerRequest();
			builder.RegisterType<AdminWebContext>().AsSelf().InstancePerRequest();
			builder.RegisterType<EmailService>().AsSelf().InstancePerRequest();
			builder.RegisterType<MixberryNavigationService>().AsSelf().InstancePerRequest();
			builder.RegisterType<LocalFileService>().As<IFileService>().InstancePerRequest();
			builder.RegisterType<MixberryDbFactory>().As<IDbFactory>().InstancePerRequest();
			builder.RegisterType<DefaultSettingsProvider>().As<ISettingsProvider>().InstancePerRequest();
			builder.RegisterType<EmailLogsService>().As<IEmailLogService >().InstancePerRequest();
			builder.RegisterType<LocalFileService>().As<IFileService >().InstancePerRequest();
			builder.RegisterType<MixberryTemplateEngine>().AsSelf().SingleInstance();
			builder.RegisterType<LocalizationContext>().As<ILocalizationContext>().InstancePerRequest();
			builder.RegisterType<LangDbContext>().AsSelf().InstancePerRequest();
		}
	}
}