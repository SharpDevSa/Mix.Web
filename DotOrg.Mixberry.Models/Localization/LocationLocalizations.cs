using System.Collections.Generic;

namespace DotOrg.Mixberry.Models.Localization
{
	public class LocationLocalizations : LocalizationObject
	{
		public LocationLocalizations()
		{
			Group = new List<Location>();
		}
		public virtual ICollection<Location> Group { get; set; }
	}
}