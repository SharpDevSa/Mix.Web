using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using DotOrg.Db.Entities;
using DotOrg.Web.Mvc;

namespace DotOrg.Web.Extensions
{
    public static class UrlHelperExtensions
    {
        #region Image resizer

        public static string Image(this UrlHelper helper, IFileEntity file, params string[] parameters)
        {
            var relativePath = file == null ? null : file.Url;
            return helper.Image(relativePath, parameters);
        }

        public static string Image(this UrlHelper helper, string relativePath, params string[] parameters)
        {
            relativePath = relativePath ?? WebConfig.NoImage;
            if (parameters.Length == 0)
            {
                return helper.Content(relativePath);
            }
            var result = String.Format("{0}?{1}", helper.Content(relativePath), String.Join("&", parameters));
            //#if DEBUG
            //			return "http://b500.test.mastertest.ru" + result;
            //#else
            return result;
            //#endif
        }

        #endregion
    }
}
