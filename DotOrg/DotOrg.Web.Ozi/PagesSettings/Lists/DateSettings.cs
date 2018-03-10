using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Web.Ozi.PagesSettings.Lists
{
	public class DateSettings : ColSettings
	{
		public string Format { get; set; }
		public override string Control { get { return ControlsNames.Date; } }
	}
}
