using System.Collections.Generic;

namespace DotOrg.Mixberry.Models.Localization
{
	public class NewsLocalizations : LocalizationObject
	{
		public NewsLocalizations()
		{
			Group = new List<News>();
		}

		public virtual ICollection<News> Group { get; set; }
	}
}