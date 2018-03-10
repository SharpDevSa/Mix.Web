using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Web.Ozi.PagesSettings.Forms
{
	public class WysiwygSettings : FieldSettings
	{
		public int Rows { get; set; }
		public override string Control { get { return "ozi_Wysiwyg"; } }
	}
}
