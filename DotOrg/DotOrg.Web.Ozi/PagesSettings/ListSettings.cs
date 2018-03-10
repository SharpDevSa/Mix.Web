﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using DotOrg.Web.Ozi.PagesSettings.Lists;

namespace DotOrg.Web.Ozi.PagesSettings
{
	public class ListSettings
	{
		public List<ColSettings> Cols
		{
			get;
			set;
		}

		public RouteValueDictionary AddRouteParameters()
		{
			var values = new RouteValueDictionary();
			foreach (var column in Cols)
			{
				column.AddRouteParameter(values);
			}
			return values;
		}

		public bool Levels { get; set; }
		public string OrderAsc { get; set; }
		public string SelectRowProperty { get; set; }
		public int? PageSize { get; set; }
		public string OrderDesc { get; set; }
		public List<ListActionSettings> ListActions { get; set; }
		public List<GlobalActionSettings> GlobalActions { get; set; }
	}
}
