using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotOrg.Web.Ozi.PagesSettings.Forms;

namespace DotOrg.Web.Ozi.PagesSettings
{
	public class FormSettings
	{
		public List<TabsSettings> Tabs { get; set; }
		public bool? Localizable { get; set; }
		public string Roles { get; set; }

		public List<FieldSettings> Fields
		{
			get { return Tabs.OrderBy(t => t.Sort).SelectMany(t => t.Fields).ToList(); }
		}
	}
}
