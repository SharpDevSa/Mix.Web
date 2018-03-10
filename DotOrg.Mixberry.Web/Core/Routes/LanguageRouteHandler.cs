using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotOrg.Mixberry.Web.Core.Routes
{
	public class LanguageRouteHandler : MvcRouteHandler
	{
		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			// todo: надо добавить язык в урле, если его нет

			/*
			if (requestContext.RouteData.Values["lang"] == null)
			{
				requestContext.RouteData.Values["lang"] = "ru";
			}
			if ("ru".Equals(requestContext.RouteData.Values["lang"]))
			{
				requestContext.RouteData.Values["lang"] = null;
			}
			*/
			return base.GetHttpHandler(requestContext);
		}
	}
}