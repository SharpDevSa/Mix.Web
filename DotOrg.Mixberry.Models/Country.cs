using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models
{
	public class Country : ISortableEntity
	{
		public int Id { get; set; }
		public int Sort { get; set; }

		public string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}