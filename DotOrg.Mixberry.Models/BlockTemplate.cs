using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models
{
	public class BlockTemplate : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Template { get; set; }
		public bool Debug { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
