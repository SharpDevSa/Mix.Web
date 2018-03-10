using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotOrg.Web.Mvc
{
	public static class UriHelper
	{
		private const string UriSeparator = "/";

		public static string Combine(string left, string right, bool relative = false)
		{
			if (left == null)
				left = string.Empty;
			if (right == null)
				right = string.Empty;
			var result = left.EndsWith(UriSeparator) ? string.Format("{0}{1}", left, right.StartsWith(UriSeparator) ? right.Substring(1) : right) : string.Format("{0}{1}{2}", left, right.StartsWith(UriSeparator) ? string.Empty : UriSeparator, right);
			result = result.StartsWith(UriSeparator) || relative ? result : string.Format("{0}{1}", UriSeparator, result);
			if (result.EndsWith(UriSeparator) && result.Length > 1)
			{
				result = result.Substring(0, result.Length - 1);
			}
			return result;
		}

		public static string GetPath(string filename)
		{
			var index = filename.LastIndexOf(UriSeparator, StringComparison.InvariantCultureIgnoreCase);
			return index == -1 ? UriSeparator : filename.Substring(0, index + 1);
		}

		public static string GetName(string filename)
		{
			var index = filename.LastIndexOf(UriSeparator, StringComparison.InvariantCultureIgnoreCase);
			return index == -1 ? filename : filename.Substring(index + 1);
		}

		public static string FullUrl(this UrlHelper helper, string relativeUrl)
		{
			var path = VirtualPathUtility.ToAbsolute(relativeUrl);
			if (helper.RequestContext.HttpContext != null && helper.RequestContext.HttpContext.Request != null && helper.RequestContext.HttpContext.Request.Url != null)
			{
				var url = new Uri(helper.RequestContext.HttpContext.Request.Url, path).AbsoluteUri;
				return url;
			}
			throw new Exception("Unsupported");
		}

		public static RouteData GetRoute(Uri uri)
		{
			return GetRoute(uri == null ? "/" : uri.PathAndQuery);
		}

		public static RouteData GetRoute(string url)
		{
			var httpContext = new DummyHttpContext(url);
			var routeData = RouteTable.Routes.GetRouteData(httpContext);
			return routeData;
		}

		public static string SetUrlParameter(this string url, string paramName, string value)
		{
			return new Uri(url).SetParameter(paramName, value).ToString();
		}

		public static Uri SetParameter(this Uri url, string paramName, string value)
		{
			if (url == null)
				return new Uri(string.Empty);
			var queryParts = HttpUtility.ParseQueryString(url.Query);
			if (string.IsNullOrEmpty(value))
			{
				queryParts.Remove(paramName);
			}
			else
			{
				queryParts[paramName] = value;
			}
			return new Uri(url.AbsoluteUriExcludingQuery() + '?' + queryParts);
		}

		public static Uri SetParameterIfDoesntExists(this Uri url, string paramName, string value)
		{
			if (url == null)
				return new Uri(string.Empty);
			var queryParts = HttpUtility.ParseQueryString(url.Query);
			if (string.IsNullOrEmpty(value))
			{
				queryParts.Remove(paramName);
			}
			else if (queryParts[paramName] == null)
			{
				queryParts[paramName] = value;
			}
			return new Uri(url.AbsoluteUriExcludingQuery() + '?' + queryParts);
		}

		public static string AbsoluteUriExcludingQuery(this Uri url)
		{
			return url.AbsoluteUri.Split('?').FirstOrDefault() ?? String.Empty;
		}

		private class DummyHttpRequest : HttpRequestBase
		{

			private readonly string _url;

			public DummyHttpRequest(string url)
			{
				_url = url.StartsWith("~") ? url : "~" + url;
			}

			public override string AppRelativeCurrentExecutionFilePath
			{
				get
				{
					return _url;
				}
			}

			public override Uri UrlReferrer
			{
				get
				{
					return null;
				}
			}

			public override string PathInfo
			{
				get
				{
					return string.Empty;
				}
			}
		}

		private class DummyHttpContext : HttpContextBase
		{

			private readonly string _url;

			public DummyHttpContext(string url)
			{
				_url = url;
			}

			public override HttpSessionStateBase Session
			{
				get
				{
					return null;
				}
			}

			public override HttpRequestBase Request
			{
				get
				{
					return new DummyHttpRequest(_url);
				}
			}

		}

	}
}
