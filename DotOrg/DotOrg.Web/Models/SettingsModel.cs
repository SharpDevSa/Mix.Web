using DotOrg.Db.Models;

namespace DotOrg.Web.Models
{
	public class SettingsModel
	{
		public SiteSetting Settings { get; set; }

		public string TabName { get; set; }
		public string GroupName { get; set; }
		public string ValueName { get; set; }

		public static SettingsModel Create(SiteSetting source)
		{
			var parts = source.Name.Split('.');
			var tabname = parts.Length > 1 ? parts[0] : null;
			var groupName = (parts.Length < 3) ? null : parts[1];
			var value = (parts.Length < 3) ? parts[1] : parts[2];
			return new SettingsModel
			{
				Settings = source,
				TabName = tabname,
				GroupName = groupName,
				ValueName = value
			};
		}
	}
}