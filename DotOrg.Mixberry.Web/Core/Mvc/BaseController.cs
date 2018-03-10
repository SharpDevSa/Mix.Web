using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using DotOrg.Core.Helpers;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Web.Mvc;
using DotOrg.Web.Ozi.Controllers;

namespace DotOrg.Mixberry.Web.Core.Mvc
{
	public abstract class BaseController : OziController
	{
		public IWebContext WebContext { get { return DependencyResolver.Current.GetService<IWebContext>(); } }

		public override DbContext DataModelContext
		{
			get { return DependencyResolver.Current.GetService<DbContext>(); }
		}

		protected virtual DbSet<T> GetObjectSet<T>() where T : class
		{
			EnableFilters();
			var list = DataModelContext.Set<T>();
			return list;
		}

		protected virtual void EnableFilters()
		{ }
		protected virtual void DiableFilters()
		{ }

		protected virtual IQueryable<T> GetEntities<T>() where T : class
		{
			var list = GetObjectSet<T>();
			return list;
		}

		protected T GetById<T>(int id) where T : class, IEntity
		{
			var result = GetEntities<T>().FirstOrDefault(arg => arg.Id == id);
			return result;
		}

		protected T GetByAlias<T>(string alias) where T : class, IHaveAliasEntity
		{
			return GetEntities<T>().FirstOrDefault(arg => arg.Alias == alias);
		}

		protected void SetLocalizationRedirects()
		{
			var localization = DependencyResolver.Current.GetService<ILocalizationContext>();
			var db = DataModelContext;

			var location = WebContext.Location.Alias;
			localization.ChangeUrl = lang =>
			{
				var alias = db.Set<Location>().Where(x => x.Lang == lang).Select(x => x.Alias).FirstOrDefault(x => x == location);
				if (alias == "home") alias = null;
				return "/" + new[] {lang, alias}.JoinWith("/");
			};
		}


	}
}
