using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Web.Ozi.PagesSettings.Forms
{
	public class StringSettings : FieldSettings
	{
		public int Maxlength { get; set; }
		public override string Control { get { return "ozi_String"; } }
	}
}
