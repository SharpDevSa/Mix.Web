using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DotOrg.Web.Ozi.Mvc
{
	public class HandleHttp401ErrorAttribute : HandleErrorAttribute
	{
		public HandleHttp401ErrorAttribute()
		{
		}

		public override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.Exception is HttpException && ((HttpException) filterContext.Exception).ErrorCode == 401)
			{
				View = "Permissions";
				filterContext.ExceptionHandled = true;
			}
			base.OnException(filterContext);
		}
	}
}
