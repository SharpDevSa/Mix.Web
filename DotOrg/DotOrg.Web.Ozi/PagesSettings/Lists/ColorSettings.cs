using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Web.Ozi.PagesSettings.Lists
{
	public class ColorSettings : ColSettings
	{
		public override string Control { get { return ControlsNames.Color; } }
	}
}
