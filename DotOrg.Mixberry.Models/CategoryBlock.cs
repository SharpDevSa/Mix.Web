using System.Collections.Generic;
using DotOrg.Mixberry.Models.Entities;

namespace DotOrg.Mixberry.Models
{
	public class CategoryBlock: IBlockEntity
	{
		public CategoryBlock()
		{
			Images = new List<WebFile>();
		}
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public string Alias { get; set; }
		public string Name { get; set; }
		public int Sort { get; set; }
		public string Content { get; set; }
		public virtual Category Category { get; set; }
		public virtual ICollection<WebFile> Images { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}