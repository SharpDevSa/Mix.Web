using System.Collections.Generic;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models.Entities
{
	public interface IHaveBlocksEntity<TBlock>: IEntity where TBlock: IBlockEntity
	{
		ICollection<TBlock> Blocks { get; set; } 
		 
	}
}