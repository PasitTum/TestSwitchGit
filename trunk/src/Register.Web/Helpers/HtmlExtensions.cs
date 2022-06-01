using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Caching;

namespace Register.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static IHtmlString GetAppVersion(this HtmlHelper helper)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return MvcHtmlString.Create(version);
        }

        public static string GetAppTitle(this HtmlHelper helper)
        {
            var assembly = Assembly.GetExecutingAssembly();
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            AssemblyTitleAttribute attribute = null;
            if (attributes.Length > 0)
            {
                attribute = attributes[0] as AssemblyTitleAttribute;
                return attribute.Title;
            }
            return "";
        }

        public static MvcHtmlString JsWithVersion(this HtmlHelper helper, string filename)
        {
            string version = GetVersion(helper, filename);
            var context = helper.ViewContext.RequestContext.HttpContext;
            var scriptFile = UrlHelper.GenerateContentUrl(filename, context) + version;
            return MvcHtmlString.Create("<script type='text/javascript' src='" + scriptFile + "'></script>");
            //return MvcHtmlString.Create("<script type='text/javascript' src='" + filename + version + "'></script>");
        }

        private static string GetVersion(this HtmlHelper helper, string filename)
        {
            var context = helper.ViewContext.RequestContext.HttpContext;

            if (context.Cache[filename] == null)
            {
                var physicalPath = context.Server.MapPath(filename);
                var version = $"?v={new System.IO.FileInfo(physicalPath).LastWriteTime.ToString("MMddHHmmss")}";
                context.Cache.Add(filename, version, null,
                  DateTime.Now.AddMinutes(5), TimeSpan.Zero,
                  CacheItemPriority.Normal, null);
                return version;
            }
            else
            {
                return context.Cache[filename] as string;
            }
        }
    }
}