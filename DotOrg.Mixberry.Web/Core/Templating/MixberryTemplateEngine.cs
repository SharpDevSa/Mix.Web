using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using RazorEngine.Compilation.ReferenceResolver;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace DotOrg.Mixberry.Web.Core.Templating
{
	public class MixberryTemplateEngine : IDisposable
	{
		private IRazorEngineService _service;
		private DefaultCachingProvider _cache;

		private IRazorEngineService GetEngine()
		{
			if (_service == null)
			{
				_cache = new DefaultCachingProvider();
				var config = new TemplateServiceConfiguration
				{
					BaseTemplateType = typeof (MixberryTemplate<>),
					Namespaces = new SortedSet<string>(new[]
					{
						"System",
						"System.Linq",
						"DotOrg.Mixberry.Web.Core.Templating",
						"DotOrg.Mixberry.Models",
						"DotOrg.Core.Helpers"
					}),
					AllowMissingPropertiesOnDynamic = true,
					CachingProvider = _cache,
					ReferenceResolver = new UseCurrentAssembliesReferenceResolver()
					//ReferenceResolver = new RazorEngineReferenceResolver()
				};
				_service = RazorEngineService.Create(config);
			}
			return _service;
		}

		public string RenderTemplate(string key, string template, TemplateModel model)
		{
			string result;
			try
			{
				result = RunCompile(key, template, model);
			}
			catch (Exception e)
			{
				result = string.Format("<article class=\"razor-error\"><h1>{0}</h1><pre>{1}</pre></article>", key, e.Message);
			}
			return result;
		}

		private static readonly object SyncObject = new object();

		private string RunCompile(string key, string template, TemplateModel model = null)
		{
			var engine = GetEngine();
			var modelType = typeof(TemplateModel);

			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			var resultKey = lang.Current.Lang + key;

			lock (SyncObject)
			{
				var cached = engine.IsTemplateCached(resultKey, modelType);
				var result = cached
					? engine.Run(resultKey, modelType, model)
					: engine.RunCompile(template, resultKey, modelType, model);
				return result;
			}
		}

		public void RefreshCache(int id, string alias)
		{
		}

		#region disposable logic

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (_service != null)
					{
						_service.Dispose();
					}
					if (_cache != null)
					{
						_cache.Dispose();
					}
				}
				_disposed = true;
			}
		}

		~MixberryTemplateEngine()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}