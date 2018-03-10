using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotOrg.Mixberry.Models;

namespace DotOrg.Mixberry.Web.ViewModels
{
	public class ProductViewModel
	{
		public Product Product { get; set; }
		public Category Category { get; set; }
	}
}