using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Core.Helpers;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Web.Ozi.Infrastructure;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
    public partial class PartnersController : BaseAdminController<Partner>
    {
        private string _lang;
        private const string NameKey = "partner.name.";// + alias
        private const string UrlKey = "partner.url.";// + alias
        private IEnumerable<LocalizationText> _list;

        protected override void Initialize(RequestContext requestContext)
        {
            _lang = DependencyResolver.Current.GetService<ILocalizationContext>().Current.Lang;
            base.Initialize(requestContext);
        }


        protected override dynamic GetIndexViewModel(IEnumerable<Partner> objectsList, FilterParameters filterParameters)
        {
            var localizedList = objectsList.ToList();

            localizedList.ForEach(LocalizeItem);

            return base.GetIndexViewModel(localizedList, filterParameters);
        }

        protected override void PrepareEntity(Partner entity)
        {
            LocalizeItem(entity);
            var countries = DataModelContext.Set<Country>().ToList();
            countries.ForEach(x =>
            {
                var text = GetByKey("country.name." + x.Name);
                if (text != null)
                {
                    x.Name = text.Value;
                }
            });
            WebContext.ViewBag.Countries = countries;
        }

        protected override void OnEntityEditing(Partner entity, FormCollection collection)
        {
            var name = collection["Name"];
            var url = collection["Url"];
            var alias = collection["Alias"];

            collection.Remove("Name");
            collection.Remove("Url");
            if (string.IsNullOrEmpty(alias))
            {
                if (string.IsNullOrEmpty(name))
                {
                    ModelState.AddModelError("Name", "Введите название партнёра");
                    return;
                }
                alias = name.Transliterate();
            }
            entity.Alias = alias;
            if (string.IsNullOrEmpty(entity.Name))
            {
                entity.Name = NameKey + alias;
            }
            var nameText = GetByKey(entity.Name, true);
            nameText.Value = name;

            if (entity.Url.IsNullOrEmpty())
            {
                entity.Url = UrlKey + alias;
            }
            var urlText = GetByKey(entity.Url, true);
            urlText.Value = url;

            DataModelContext.SaveChanges();
        }

        private void LocalizeItem(Partner entity)
        {
            var name = GetByKey(entity.Name);
            if (name != null)
            {
                entity.Name = name.Value;
            }
            var url = GetByKey(entity.Url);
            if (url != null)
            {
                entity.Url = url.Value;
            }
        }

        private IEnumerable<LocalizationText> GetLocalizations()
        {
            return _list ?? (_list = DataModelContext.Set<LocalizationText>().Where(x => x.Lang == _lang).ToList());
        }

        private LocalizationText GetByKey(string fullKey, bool create = false)
        {
            var localizations = GetLocalizations();
            var text = localizations.FirstOrDefault(t => t.Key == fullKey);
            if (text == null && create)
            {
                text = new LocalizationText
                {
                    Lang = _lang,
                    Key = fullKey
                };
                DataModelContext.Set<LocalizationText>().Add(text);
            }
            return text;
        }
    }
}