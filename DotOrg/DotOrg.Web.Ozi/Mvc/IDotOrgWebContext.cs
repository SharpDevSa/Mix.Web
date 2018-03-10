using System.Web.Mvc;
using DotOrg.Db.Entities;

namespace DotOrg.Web.Ozi.Mvc
{
	public interface IDotOrgWebContext
	{
		IMetadataEntity Metadata { get; set; }
		dynamic ViewBag { get; }
		ViewDataDictionary ViewData { get; set; }
	}
}