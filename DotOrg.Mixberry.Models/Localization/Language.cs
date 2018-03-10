namespace DotOrg.Mixberry.Models.Localization
{
	public class Language
	{
		public string Lang { get; set; }
		public string Name { get; set; }
		public string StaticLocalizationFile { get; set; }
		public bool Deleted { get; set; }
		public bool Primary { get; set; }

		public override string ToString()
		{
			return string.Format("{0} ({1})", Lang, Name);
		}
	}
}