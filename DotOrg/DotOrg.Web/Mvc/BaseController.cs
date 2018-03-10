using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DotOrg.Web.Mvc
{
	public abstract class BaseController : Controller
	{
		public IWebContext WebContext
		{
			get { return DependencyResolver.Current.GetService<IWebContext>(); }
		}
	}
}
