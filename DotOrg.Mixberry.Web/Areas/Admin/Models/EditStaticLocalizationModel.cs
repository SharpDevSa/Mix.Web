using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Mixberry.Web.Areas.Admin.Models
{
	public class EditStaticLocalizationModel
	{
		public List<EditStaticLocalizationItem> Items { get; set; }
	}

	public class EditStaticLocalizationItem
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string Label { get; set; }
		public string DefaultValue { get; set; }
	}
}
