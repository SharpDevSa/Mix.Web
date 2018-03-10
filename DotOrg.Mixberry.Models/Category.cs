using System.Collections.Generic;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Models.Entities;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Models
{
	public class Category : IHaveBlocksEntity<CategoryBlock>, ISortableEntity, IVisibleEntity, IHaveAliasEntity, IMetadataEntity
	{
		public Category()
		{
			Blocks = new List<CategoryBlock>();
			Products = new List<Product>();
			Images = new List<WebFile>();
			Groups = new List<ProductGroup>();
		}

		public int Id { get; set; }
		public string Alias { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Sort { get; set; }
		public bool Visibility { get; set; }

		public virtual ICollection<CategoryBlock> Blocks { get; set; }
		public virtual ICollection<ProductGroup> Groups { get; set; }
		public virtual ICollection<Product> Products { get; set; }
		public virtual ICollection<WebFile> Images { get; set; }

		public string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaTitle { get; set; }
		public string MetaData { get; set; }

		public bool ShowInMenu { get; set; }

		public string Lang { get; set; }
		public virtual Language Language { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}