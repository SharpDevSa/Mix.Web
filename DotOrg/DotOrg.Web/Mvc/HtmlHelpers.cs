using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DotOrg.Web.Extensions;

namespace DotOrg.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static string PluralizeWord(this HtmlHelper htmlHelper, int count, string single, string several, string many)
		{
			var dec = (count % 100) / 10; // кол-во десятков
			var low = count % 10; // кол-во единиц
			if (low == 1 && dec != 1)
			{
				return single;
			}
			if (low > 1 && low < 5 && dec != 1)
			{
				return several;
			}
			return many;
		}

	    public static MvcForm UploadForm(this HtmlHelper helper, ActionResult result)
		{
			return helper.BeginForm(result, FormMethod.Post, new { enctype = "multipart/form-data" });
		}

		public static MvcForm UploadForm(this HtmlHelper helper, ActionResult result, object htmlAttributes)
		{
			var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			if (!dictionary.ContainsKey("enctype"))
				dictionary.Add("enctype", "multipart/form-data");
			return helper.BeginForm(result, FormMethod.Post, dictionary);
		}

		#region image resizer

		public static IHtmlString Image(this HtmlHelper helper, string relativePath, params string[] parameters)
		{
			return helper.Image(relativePath, null, parameters);
		}

		public static IHtmlString Image(this HtmlHelper helper, string relativePath, object attrs = null, params string[] parameters)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var url = urlHelper.Image(relativePath, parameters);
			return GenerateImageElement(urlHelper, url, attrs, 0, 0);
		}

		private static IHtmlString GenerateImageElement(UrlHelper urlHelper, string imgUrl, object attrs, int w, int h)
		{
			var img = new TagBuilder("img");
			if (attrs != null)
			{
				img.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attrs));
			}
			if (string.IsNullOrEmpty(imgUrl))
			{
				var url = urlHelper.Content(WebConfig.NoImage);
				img.MergeAttribute("src", url);
				if (w != 0)
					img.MergeAttribute("width", w.ToString(CultureInfo.InvariantCulture));
				if (h != 0)
					img.MergeAttribute("height", h.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				img.MergeAttribute("src", imgUrl);
			}
			return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
		}

        #endregion

        #region scripts

        private class ScriptBlock : IDisposable
		{
			public static List<string> PageScripts(string blockKey)
			{
				if (HttpContext.Current.Items[blockKey] == null)
					HttpContext.Current.Items[blockKey] = new List<string>();
				return (List<string>)HttpContext.Current.Items[blockKey];
			}

			private readonly string _blockKey;
			readonly WebViewPage _webPageBase;

			public ScriptBlock(string blockKey, WebViewPage webPageBase)
			{
				_blockKey = blockKey;
				_webPageBase = webPageBase;
				_webPageBase.OutputStack.Push(new StringWriter());
			}

			public void Dispose()
			{
				PageScripts(_blockKey).Add((_webPageBase.OutputStack.Pop()).ToString());
			}
		}

		public static IDisposable BeginScripts(this HtmlHelper helper)
		{
			return new ScriptBlock("scripts", (WebViewPage)helper.ViewDataContainer);
		}

		public static MvcHtmlString PageScripts(this HtmlHelper helper)
		{
			return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PageScripts("scripts").Select(s => s.ToString(CultureInfo.InvariantCulture))));
		}

		public static IDisposable BeginHead(this HtmlHelper helper)
		{
			return new ScriptBlock("head", (WebViewPage)helper.ViewDataContainer);
		}

		public static MvcHtmlString PageHead(this HtmlHelper helper)
		{
			return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PageScripts("head").Select(s => s.ToString(CultureInfo.InvariantCulture))));
		}

		#endregion
    }
}
