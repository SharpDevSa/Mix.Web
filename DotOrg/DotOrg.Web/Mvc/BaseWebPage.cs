using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

namespace DotOrg.Web.Mvc
{
	public abstract class BaseWebPage<TModel> : WebViewPage<TModel>
	{

		private static readonly object O = new object();

		public HelperResult RenderSection(string sectionName, Func<object, HelperResult> defaultContent)
		{
			return IsSectionDefined(sectionName) ? RenderSection(sectionName) : defaultContent(O);
		}

		public HelperResult RedefineSection(string sectionName)
		{
			return RedefineSection(sectionName, null);
		}

		public HelperResult RedefineSection(string sectionName, Func<object, HelperResult> defaultContent)
		{
			if (IsSectionDefined(sectionName))
			{
				DefineSection(sectionName, () => Write(RenderSection(sectionName)));
			}
			else if (defaultContent != null)
			{
				DefineSection(sectionName, () => Write(defaultContent(O)));
			}
			return new HelperResult(_ => { });
		}

		protected string If(bool condition, string trueCase, string falseCase = null)
		{
			if (condition)
				return trueCase;
			return falseCase;
		}

		protected string If(bool condition, Func<string> trueCase, Func<string> falseCase = null)
		{
			if (condition)
				return trueCase();
			if (falseCase != null)
				return falseCase();
			return string.Empty;
		}
	}
	
	public abstract class BaseWebPage : BaseWebPage<object>
	{
	}
}
