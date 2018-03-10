using System;
using System.Web;

namespace DotOrg.Db.Entities
{
	public interface IFileEntity : IVisibleEntity, ISortableEntity
	{
		string Url { get; set; }
		string SourceName { get; set; }
		string Alt { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		DateTime Date { get; set; }
        long? Size { get; set; }
		string Class { get; set; }
	}
}
