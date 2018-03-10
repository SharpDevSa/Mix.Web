using DotOrg.Web.Ozi.Infrastructure;
using System.Collections.Generic;
using DotOrg.Web.Ozi.PagesSettings;

namespace DotOrg.Web.Ozi.ViewModels
{
    public class ListViewModel
    {
        public Settings Settings { get;set; }
        public IEnumerable<object> ListData { get; set; }
    	public FilterParameters FilterParameters { get; set; }
    }
}