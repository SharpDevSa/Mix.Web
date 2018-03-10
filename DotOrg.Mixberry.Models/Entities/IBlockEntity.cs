using System.Collections.Generic;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models.Entities
{
	public interface IBlockEntity: ISortableEntity, IHaveAliasEntity
	{
		string Content { get; set; }
		ICollection<WebFile> Images { get; set; } 
	}
}