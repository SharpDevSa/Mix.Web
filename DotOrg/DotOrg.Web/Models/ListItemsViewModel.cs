using System.Collections.Generic;

namespace DotOrg.Web.Models
{
	public class ListItemsViewModel<T>
	{
		public IEnumerable<T> Items { get; set; }
		public PagerViewModel Pager { get; set; }
	}
}
