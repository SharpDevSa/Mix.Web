using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotOrg.Web.Ozi.PagesSettings.Forms;

namespace DotOrg.Web.Ozi.PagesSettings
{
	public class TabsSettings
	{
		public List<FieldSettings> Fields { get; set; }
		public string Name { get; set; }
		public string Roles { get; set; }
		public int Sort { get; set; }
	}
}
