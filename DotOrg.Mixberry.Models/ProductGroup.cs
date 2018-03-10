using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models
{
	public class ProductGroup : ISortableEntity, IVisibleEntity
	{
		public ProductGroup()
		{
			Products = new List<Product>();
		}

		public int Id { get; set; }

		public int Sort { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }

		public bool Visibility { get; set; }

		public int CategoryId { get; set; }
		public virtual Category Category { get; set; }
		public virtual ICollection<Product> Products { get; set; }
	}
}
