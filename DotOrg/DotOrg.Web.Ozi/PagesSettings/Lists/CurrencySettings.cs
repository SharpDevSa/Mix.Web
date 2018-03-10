using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Web.Ozi.PagesSettings.Lists
{
	public class CurrencySettings : ColSettings
	{
		//TODO не реализовано
		public string Format { get; set; }
		public override string Control { get { return ControlsNames.Currency; } }
	}
}
