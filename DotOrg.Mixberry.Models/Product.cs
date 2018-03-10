using System.Collections.Generic;
using System.Linq;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Models.Entities;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Models
{
	public class Product: ISortableEntity, IVisibleEntity, IHaveAliasEntity, IMetadataEntity
	{
		public Product()
		{
			//Blocks = new List<ProductBlock>();
			Models = new List<ProductModel>();
			Categories = new List<Category>();
			Groups = new List<ProductGroup>();

			Banners = new List<WebFile>();
			DetailsImages = new List<WebFile>();
			Related = new List<Product>();
			Blocks = new List<ProductBlock>();
		}

		public int Id { get; set; }
		public string Alias { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Specification { get; set; }
		public int Sort { get; set; }
		public bool Visibility { get; set; }


		#region details page

		public bool ShowDetails { get; set; }

		public virtual ICollection<WebFile> Banners { get; set; }
		public virtual ICollection<WebFile> DetailsImages { get; set; }
		public virtual ICollection<Product> Related { get; set; }
		public virtual ICollection<ProductBlock> Blocks { get; set; }
		public string DetailsTitle { get; set; }
		public string DetailsSubTitle { get; set; }
		public string DetailsDescription { get; set; }

		#endregion

		public int? ImageId { get; set; }
		public int? PromoImageId { get; set; }
		public virtual WebFile Image { get; set; }
		public virtual WebFile PromoImage { get; set; }

		public virtual ICollection<Category> Categories { get; set; }
		public virtual ICollection<ProductModel> Models { get; set; }
		public virtual ICollection<ProductGroup> Groups { get; set; }
		public string MetaDescription { get; set; }

		public string MetaKeywords { get; set; }

		public string MetaTitle { get; set; }

		public string MetaData { get; set; }

		public bool ShowInMenu { get; set; }

		public override string ToString()
		{
			return Name;
		}

		// template properties

		public WebFile ModelImage
		{
			get { return Models.Any() ? Models.First().Image : null; }
		}

		public WebFile SpecImage
		{
			get { return Image; }
		}

		//public string Lang { get; set; }
		//public virtual Language Language { get; set; }
	}
}