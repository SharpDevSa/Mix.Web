using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Core.Helpers
{
	public static class DateTimeHelper
	{
		public static int GetDaysInMonth(int year, int month)
		{
			return DateTime.DaysInMonth(year, month);
		}

		public static DateTime? TryGetDate(int? day, int? month, int? year)
		{
			if (day.HasValue && month.HasValue && year.HasValue)
			{
				return new DateTime(year.Value, month.Value, Math.Min(day.Value, GetDaysInMonth(year.Value, month.Value)));
			}
			return null;
		}

		public static int? DiffInMonths(this DateTime? begin, DateTime? end)
		{
			if (!begin.HasValue || !end.HasValue) return null;
			var diff = Math.Abs((end.Value.Month - begin.Value.Month) + 12 * (end.Value.Year - begin.Value.Year));
			return diff;
		}

		public static string HumanizeDuration(this int duration)
		{
			var years = duration / 12;
			var months = duration % 12;
			return new[]
			{
				years > 0 ? years.PluralizeWord("год", "года", "лет") : null,
				months > 0 ? months.PluralizeWord("месяц", "месяца", "месяцев") : null
			}.JoinWith(", ");


			//if (durationInMonths == 0)
			//	return "Без опыта работы";
			//var years = durationInMonths / 12;
			//var months = durationInMonths % 12;
			//var result = "Опыт работы";
			//if (years > 0)
			//{
			//	result += " " + years.PluralizeWord("год", "года", "лет");
			//}
			//if (months > 0)
			//{
			//	result += " " + months.PluralizeWord("месяц", "месяца", "месяцев");
			//}
			//return result;

		}

		public static string Age(this DateTime? birthDate)
		{
			if (!birthDate.HasValue) return string.Empty;
			var months = birthDate.DiffInMonths(DateTime.Now);
			var years = (months.Value/12) * 12;
			return years.HumanizeDuration();
		}
	}
}
