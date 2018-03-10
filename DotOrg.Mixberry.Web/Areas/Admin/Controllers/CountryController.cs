using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DotOrg.Core.Helpers;
using DotOrg.Db.Entities;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Web.Ozi.Infrastructure;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
    [Authorize]
    public partial class CountryController : BaseAdminController<Country>
    {
        protected override void Initialize(RequestContext requestContext)
        {
            _lang = DependencyResolver.Current.GetService<ILocalizationContext>().Current.Lang;
            base.Initialize(requestContext);
        }

        protected override dynamic GetIndexViewModel(IEnumerable<Country> objectsList, FilterParameters filterParameters)
        {
            var localizedList = objectsList.ToList();
            localizedList.ToList().ForEach(x =>
            {
                var text = GetByKey(x.Name);
                if (text != null && text.Value != null)
                    x.Name = text.Value;
            });

            return base.GetIndexViewModel(localizedList, filterParameters);
        }

        protected override void OnEntityEditing(Country entity, FormCollection collection)
        {
            var name = collection["Name"];
            collection.Remove("Name");

            if (name.IsNullOrEmpty())
            {
                ModelState.AddModelError("Name", "Обязательное поле");
                return;
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                var alias = name.Transliterate().Replace(" ", "");
                entity.Name = alias;
            }

            var text = GetByKey(entity.Name, true);
            text.Value = name;
            DataModelContext.SaveChanges();
        }

        protected override void PrepareEntity(Country entity)
        {
            var text = GetByKey(entity.Name);
            if (text != null)
            {
                entity.Name = text.Value;
            }
        }

        private LocalizationText GetByKey(string shortKey, bool create = false)
        {
            var localizations = GetLocalizations();
            var countryKey = Key + shortKey;
            var text = localizations.FirstOrDefault(t => t.Key == countryKey);
            if (text == null && create)
            {
                text = new LocalizationText
                {
                    Lang = _lang,
                    Key = countryKey
                };
                DataModelContext.Set<LocalizationText>().Add(text);
            }
            return text;
        }

        private string _lang;
        private const string Key = "country.name.";
        private IEnumerable<LocalizationText> _list;
        private IEnumerable<LocalizationText> GetLocalizations()
        {
            if (_list == null)
            {
                _list = DataModelContext.Set<LocalizationText>().Where(x => x.Lang == _lang && x.Key.StartsWith(Key)).ToList();
            }
            return _list;
        }
    }
}