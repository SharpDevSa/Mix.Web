using System.Collections.Generic;
using DotOrg.Mixberry.Models.Entities;

namespace DotOrg.Mixberry.Models
{
	public class LocationBlock: IBlockEntity
	{
		public LocationBlock()
		{
			Images = new List<WebFile>();
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public string Content { get; set; }
		public int Sort { get; set; }
		public int LocationId { get; set; }
		public virtual Location Location { get; set; }
		public virtual ICollection<WebFile> Images { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}