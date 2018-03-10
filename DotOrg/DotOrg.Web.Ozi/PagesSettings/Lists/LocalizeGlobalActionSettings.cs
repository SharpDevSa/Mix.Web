using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Web.Ozi.PagesSettings.Lists
{
	public class LocalizeGlobalActionSettings : GlobalActionSettings
	{
		public override string Control { get { return ControlsNames.Localize; } }
	}
}
