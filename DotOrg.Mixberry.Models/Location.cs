using System.Collections.Generic;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Models.Entities;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Models
{
	public class Location : ILocationEntity<Location>, IHaveBlocksEntity<LocationBlock>
    {
	    public Location()
	    {
		    Blocks = new List<LocationBlock>();
			Images = new List<WebFile>();
	    }
		public int Id { get; set; }
	    public int Sort { get; set; }
	    public string Name { get; set; }
	    public string Header { get; set; }
		public string Content { get; set; }
		public string Alias { get; set; }
		public bool Visibility { get; set; }
		public bool ShowInMenu { get; set; }
	    public string MetaDescription { get; set; }
	    public string MetaKeywords { get; set; }
	    public string MetaTitle { get; set; }
	    public string MetaData { get; set; }
	    public int Level { get; set; }
	    public int? ParentId { get; set; }
	    public bool HasChilds { get; set; }
	    public ICollection<Location> Children { get; set; }
	    public Location Parent { get; set; }
	    public virtual ICollection<LocationBlock> Blocks { get; set; }
		public virtual ICollection<WebFile> Images { get; set; }

		public string Lang { get; set; }
		public virtual Language Language { get; set; }

	    public override string ToString()
	    {
		    return Name;
	    }
    }
}
