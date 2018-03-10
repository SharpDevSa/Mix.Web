using System.Data.Entity;
using System.Linq;
using DotOrg.Db.Models;
using DotOrg.Libs.Services;

namespace DotOrg.Db
{
	public class DefaultSettingsProvider : ISettingsProvider
	{
		private readonly IDbFactory _factory;
		
		public DefaultSettingsProvider(IDbFactory factory)
		{
			_factory = factory;
		}

		public string GetValue(string name)
		{
			using (var db = _factory.Create())
			{
				var s = db.Set<SiteSetting>().FirstOrDefault(setting => setting.Name == name);
				if (s == null)
				{
					db.Set<SiteSetting>().Add(new SiteSetting
					{
						Name = name,
						Title = name,
						Value = null
					});
					db.SaveChanges();
				}
				return s != null ? s.Value : null;
			}
		}

		public void SetValue(string name, string value)
		{
			using (var db = _factory.Create())
			{
				var s = db.Set<SiteSetting>().FirstOrDefault(setting => setting.Name == name);
				if (s == null)
				{
					s = new SiteSetting { Name = name };
					db.Set<SiteSetting>().Add(s);
				}
				s.Value = value;
				db.SaveChanges();
			}
		}
	}
}