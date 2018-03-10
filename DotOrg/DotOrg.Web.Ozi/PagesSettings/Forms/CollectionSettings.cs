using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Web.Ozi.PagesSettings.Forms
{
	public class CollectionSettings : FieldSettings
	{
		public override string Control { get { return "ozi_Collection"; } }
		public bool Sortable { get; set; }
		public bool AllowModify { get; set; }
		public List<FieldSettings> Fields { get; set; }
	}
}
