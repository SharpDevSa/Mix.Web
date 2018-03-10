using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models
{
	public class Partner : ISortableEntity, IVisibleEntity, IHaveAliasEntity
	{
		public Partner()
		{
			Countries = new List<Country>();
		}
		public int Id { get; set; }
		public int Sort { get; set; }
		public bool Visibility { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Alias { get; set; }

		public int? LogoId { get; set; }
		public virtual WebFile Logo { get; set; }

		public virtual ICollection<Country> Countries { get; set; }

		public bool Distributor { get; set; }
		public bool Retailer { get; set; }

		public override string ToString()
		{
			return Alias;
		}
	}
}
