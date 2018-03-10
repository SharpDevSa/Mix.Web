using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Core.Helpers
{
	public static class EnumHelpers
	{
		public static bool HasFlags<TEnum>(this TEnum item, params TEnum[] flags) where TEnum : struct, IConvertible
		{
			if (!typeof(TEnum).IsEnum)
				throw new ArgumentException("TEnum must be an enumerated type");
			return flags
				.Select(flag => Convert.ToInt32(flag))
				.All(t => (Convert.ToInt32(item) & t) == t);
		}
	}
}
