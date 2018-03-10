using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DotOrg.Web.Models
{
	public class PagerViewModel
	{
		public PagerViewModel()
		{
			PageSize = 1;
		}

		const int Range = 2;

		public int PageSize { get; set; }
		public int Page { get; set; }
		public int Total { get; set; }

		public int PagesCount
		{
			get
			{
				return Total / PageSize;
			}
		}


		public int End
		{
			get
			{
				var page = Page;
				var end = Math.Min(Math.Max(page + Range, 1 + 2 * Range), PagesCount);
				return end;
			}
		}

		public int Start
		{
			get
			{
				var page = Page;
				var start = Math.Max(1, Math.Min(page - Range, PagesCount - 2 * Range));
				return start;
			}
		}

		public bool CanPrev
		{
			get { return Page != 1; }
		}

		public bool CanNext
		{
			get { return Page != End + 1; }
		}

		public IEnumerable<int> GetPages()
		{
			for (var i = Start; i <= End + 1; i++)
				yield return i;
		}

		public Func<int, string> GenerateUrl { get; set; }
	}
}
