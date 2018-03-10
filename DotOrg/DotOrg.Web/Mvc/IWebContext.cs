using DotOrg.Db.Entities;

namespace DotOrg.Web.Mvc
{
	public interface IWebContext
	{
        //Location Location { get; set; }
        IMetadataEntity Metadata { get; set; }
        string StartScriptParameter { get; set; }
    }
}
