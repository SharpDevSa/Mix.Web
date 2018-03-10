using System.Collections.Generic;

namespace DotOrg.Mixberry.Models.Localization
{
	public abstract class LocalizationObject
	{
		public int Id { get; set; }

		public string Lang { get; set; }
		public Language Language { get; set; }
	}
}
