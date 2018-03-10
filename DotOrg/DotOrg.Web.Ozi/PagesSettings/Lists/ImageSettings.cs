using DotOrg.Web.Ozi.Mvc;

namespace DotOrg.Web.Ozi.PagesSettings.Lists
{
	public class ImageSettings : ColSettings
	{
		public string ImageWidth { get; set; }
		public override string Control { get { return ControlsNames.Image; } }
	}
}
