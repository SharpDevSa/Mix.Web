using System;
using System.Globalization;
using System.Xml.Linq;

namespace DotOrg.Core.Helpers
{
	public static class XmlHelpers
	{
		public static int GetInt(this XElement element, string attributeName, int defaultValue)
		{
			return GetInt(element, attributeName) ?? defaultValue;
		}

		public static int? GetInt(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			if (attribute == null)
				return null;
			int result;
			return int.TryParse(attribute.Value, out result) ? result : (int?)null;
		}


		public static XName ToXName(this string localName)
		{
			return XName.Get(localName, "urn:settings");
		}

		public static double GetDouble(this XElement element, string attributeName, double defaultValue)
		{
			return GetDouble(element, attributeName) ?? defaultValue;
		}

		public static double? GetDouble(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			if (attribute == null)
				return null;
			double result;
			return double.TryParse(attribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? result : (double?)null;
		}

		public static string GetString(this XElement element, string attributeName, string defaultValue = null)
		{
			var attribute = element.Attribute(attributeName);
			return attribute != null ? attribute.Value : defaultValue;
		}

		public static bool? GetNullableBoolean(this XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);
			return attribute == null ? null : (bool?)Boolean.Parse(attribute.Value);
		}

		public static bool GetBoolean(this XElement element, string attributeName, bool defaultValue = false)
		{
			var attribute = element.Attribute(attributeName);
			return attribute == null
				? defaultValue
				: Boolean.Parse(attribute.Value);
		}

		public static T GetValue<T>(this XElement field, string name, Func<string, T> converter, T defaultValue)
		{
			var attr = field.Attribute(name);
			return attr != null ? converter(attr.Value) : defaultValue;
		}
	}
}
