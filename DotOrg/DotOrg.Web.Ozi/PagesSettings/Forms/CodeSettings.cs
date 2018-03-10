using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Web.Ozi.PagesSettings.Forms
{
	public class CodeSettings : FieldSettings
	{
		public string Type { get; set; }
		public override string Control { get { return "ozi_Code"; } }
	}
}
