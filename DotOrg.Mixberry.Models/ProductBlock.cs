using System.Collections.Generic;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Models.Entities;

namespace DotOrg.Mixberry.Models
{
	public class ProductBlock : ISortableEntity, IHaveAliasEntity, IVisibleEntity
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public virtual Product Product { get; set; }
		public int Sort { get; set; }
		public bool Visibility { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public string Content { get; set; }
	}

	//public class ProductBlock: IBlockEntity
	//{
	//	public ProductBlock()
	//	{
	//		Images = new List<WebFile>();
	//	}
	//	public int Id { get; set; }
	//	public int ProductId { get; set; }
	//	public string Alias { get; set; }
	//	public string Name { get; set; }
	//	public int Sort { get; set; }
	//	public string Content { get; set; }
	//	public virtual Product Product { get; set; }
	//	public virtual ICollection<WebFile> Images { get; set; }

	//	public override string ToString()
	//	{
	//		return Name;
	//	}
	//}
}