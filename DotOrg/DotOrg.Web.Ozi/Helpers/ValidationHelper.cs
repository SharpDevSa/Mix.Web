using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DotOrg.Web.Ozi.Helpers
{
    internal static class ValidationHelper
    {
        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas)
        {
            if (param == null)
            {
                return !checkForNull;
            }
            param = param.Trim();
            return (!checkIfEmpty || param.Length >= 1) && (!checkForCommas || !param.Contains(","));
        }
        internal static Exception CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    return new ArgumentNullException(paramName);
                }
                return null;
            }
            else
            {
                param = param.Trim();
                if (checkIfEmpty && param.Length < 1)
                {
                    return new ArgumentException(string.Format("Параметр не может быть пустым", new object[]
					{
						paramName
					}), paramName);
                }
                if (checkForCommas && param.Contains(","))
                {
                    return new ArgumentException(string.Format("Параметр не может содержать символ: ',' ", new object[]
					{
						paramName
					}), paramName);
                }
                return null;
            }
        }
        internal static Exception CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, string paramName)
        {
            if (param == null)
            {
                return new ArgumentNullException(paramName);
            }
            if (param.Length < 1)
            {
                return new ArgumentException(string.Format("Массив параметров не может быть пустым", new object[]
				{
					paramName
				}), paramName);
            }
            Hashtable hashtable = new Hashtable(param.Length);
            for (int i = param.Length - 1; i >= 0; i--)
            {
                ValidationHelper.CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
                if (hashtable.Contains(param[i]))
                {
                    return new ArgumentException(string.Format("Повторяющийся эелемент", new object[]
					{
						paramName
					}), paramName);
                }
                hashtable.Add(param[i], param[i]);
            }
            return null;
        }
    }
}
