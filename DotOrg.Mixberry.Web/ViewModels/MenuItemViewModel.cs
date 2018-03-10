using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotOrg.Mixberry.Web.ViewModels
{
	public class MenuItemViewModel
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public string Header { get; set; }
		public string Alias { get; set; }
		public bool Active { get; set; }
		public List<MenuItemViewModel> Children { get; set; }
	}
}