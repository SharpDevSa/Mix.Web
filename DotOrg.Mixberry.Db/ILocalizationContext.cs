using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Db
{
	public interface ILocalizationContext
	{
		Language Current { get; }
		//Location ChangeLocation(Location from, string langTo);
		bool ValidateLanguage(string lang, bool isAdmin = false);
		Func<string, string> ChangeUrl { get; set; }
		IEnumerable<Language> All();
		Language GetDefaultLang();
		string SetUrlLang(string url);
		string GetStaticText(string key);
		void ResetStaticCache();
	}
}