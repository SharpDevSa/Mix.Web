using System.Collections.Generic;

namespace DotOrg.Mixberry.Models.Localization
{
	public class CategoryLocalizations : LocalizationObject
	{
		public CategoryLocalizations()
		{
			Group = new List<Category>();
		}

		public virtual ICollection<Category> Group { get; set; }
	}
}