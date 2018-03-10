namespace DotOrg.Web.Models
{
	public class ItemViewModel<T>
	{
		public T Item { get; set; }
		public T Next { get; set; }
		public T Prev { get; set; }
		public int ItemPage { get; set; }
	}
}