using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using DotOrg.Db.Entities;

namespace DotOrg.Web.Services
{
	public class NavigationService
	{
		protected readonly DbContext Db;

		public NavigationService(DbContext db)
		{
			Db = db;
		}

		private static object _syncObject = new object();

		private static readonly Dictionary<int, string> Cache = new Dictionary<int, string>();

		public string GetUrl<T>(ILocationEntity<T> location) where T : class, ILocationEntity<T>
		{
			if (Cache.ContainsKey(location.Id))
				return Cache[location.Id];
			lock (_syncObject)
			{
				var url = new StringBuilder();
				var item = location;
				do
				{
					url.Insert(0, BaseLocationUrl(item));
					item = GetParent(location);
				} while (item != null);
				var result = url.ToString();
				if (!WebConfig.RemoveTrailingSlash)
				{
					result = result + WebConfig.PathSeparator;
				}
				else
				{
					result = string.IsNullOrEmpty(result) ? WebConfig.PathSeparator.ToString() : result;
				}
				Cache.Add(location.Id, result);
				return result;
			}
		}

		private ILocationEntity<T> GetParent<T>(ILocationEntity<T> location) where T : class, ILocationEntity<T>
		{
			if (location.ParentId.HasValue)
			{
				return (ILocationEntity<T>) Db.Set(location.GetType()).Find(location.ParentId);
			}
			return null;
		}

		private string BaseLocationUrl<T>(ILocationEntity<T> location) where T : class, ILocationEntity<T>
		{
			return location.Alias == WebConfig.HomepageAlias ? string.Empty : WebConfig.PathSeparator + location.Alias;
		}
	}
}
