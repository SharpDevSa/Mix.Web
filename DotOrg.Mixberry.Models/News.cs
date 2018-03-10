using System;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Models
{
	public class News : IArticleEntity
	{
		public int Id { get; set; }
		public int Sort { get; set; }
		public string Name { get; set; }
		public string Content { get; set; }
		public DateTime Date { get; set; }
		public string Alias { get; set; }
		public bool Visibility { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaTitle { get; set; }
		public string MetaData { get; set; }
		public int? ImageId { get; set; }
		public virtual WebFile Image { get; set; }
		public int? BigImageId { get; set; }
		public virtual WebFile BigImage { get; set; }

		public string Lang { get; set; }
		public virtual Language Language { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
