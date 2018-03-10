using System.Collections.Generic;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Web.ViewModels
{
	public class LanguagesViewModel
	{
		public Language Current { get; set; }
		public IEnumerable<Language> All { get; set; }
	}
}