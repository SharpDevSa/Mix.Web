using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models
{
	public class ProductModel : ISortableEntity, IVisibleEntity
	{
		public int Id { get; set; }

		public int Sort { get; set; }

		public bool Visibility { get; set; }
		public string Color { get; set; }

		public int? ImageId { get; set; }
		public virtual WebFile Image { get; set; }

		public int ProductId { get; set; }
		public virtual Product Product { get; set; }
	}
}
