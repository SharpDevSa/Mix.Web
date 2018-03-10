using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Core.Mvc;
using DotOrg.Web.Models;
using EntityFramework.Filters;

namespace DotOrg.Mixberry.Web.Controllers
{
	public partial class NewsController : BaseController
	{
		private readonly ILocalizationContext _localization;
		private const int PageSize = 20;

		public NewsController(ILocalizationContext localization)
		{
			_localization = localization;
		}

		public virtual ActionResult Index(int page)
		{
			var pageValue = page - 1;
			var vm = new ListItemsViewModel<News>
			{
				Items = GetNewsPage(pageValue).ToList(),
				Pager = new PagerViewModel
				{
					PageSize = PageSize,
					Total = GetEntities<News>().Count(x => x.Visibility),
					Page = pageValue,
					GenerateUrl = p => Url.Action(MVC.News.Index(p))
				}
			};

			SetLocalizationRedirects();

			if (!vm.Items.Any())
			{
				return HttpNotFound();
			}

            return View(vm);
        }

		public virtual ActionResult Details(int id)
		{
			var page = 0;
			var list = GetNewsPage(page);
			while (!list.Any(x => x.Id == id))
			{
				page++;
				list = GetNewsPage(page);
				if (!list.Any())
				{
					page--;
					break;
				}
			}

			var model = new ItemViewModel<News>
			{
				Item = GetEntities<News>().Include(x => x.Image).Include(x => x.BigImage).FirstOrDefault(x => x.Visibility && x.Id == id && x.Lang == _localization.Current.Lang),
				ItemPage = page + 1,
				Next = GetNews().ToList().SkipWhile(x => x.Id != id).Skip(1).FirstOrDefault(),
				Prev = GetNews().ToList().TakeWhile(x => x.Id != id).LastOrDefault()
			};

			SetLocalizationRedirects();

			if (model.Item == null)
			{
				return HttpNotFound();
			}

			//WebContext.PushBreadcrumb(model);
			WebContext.Metadata = model.Item;

		    return View(model);
	    }

		private IQueryable<News> GetNews()
		{
			return GetEntities<News>()
				.Include(x => x.Image)
				.Where(x => x.Visibility && x.Lang == _localization.Current.Lang)
				.OrderByDescending(x => x.Date);
		}

		private IQueryable<News> GetNewsPage(int page)
		{
			return GetNews()
				.Skip(page*PageSize)
				.Take(PageSize);
		}

		[ChildActionOnly]
		public virtual ActionResult LatestNews()
		{
			var model = GetNews()
				.Take(6)
				.ToList();
			return PartialView(model);
		}

		[ChildActionOnly]
		public virtual ActionResult NewsArchive()
		{
			var model = GetNews()
				.Take(3)
				.ToList();
			return PartialView(model);
		}

		protected override void EnableFilters()
		{
			DataModelContext.EnableFilter(FilterNames.News).SetParameter("lang", _localization.Current.Lang);
		}
	}
}