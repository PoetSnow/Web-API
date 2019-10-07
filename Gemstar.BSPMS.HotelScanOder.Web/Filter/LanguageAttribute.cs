using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Filter
{
    public class LanguageAttribute : ActionFilterAttribute
    {
        public const string LanguageCookieName = "BSSPALanguage";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["lang"] != null &&
                     !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
            {
                ///从路由数据(url)里设置语言
                var lang = filterContext.RouteData.Values["lang"].ToString();
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                ///从cookie里读取语言设置
                var cookie = filterContext.HttpContext.Request.Cookies[LanguageCookieName];
                var langHeader = string.Empty;
                if (cookie != null)
                {
                    ///根据cookie设置语言
                    langHeader = cookie.Value;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                else
                {
                    ///如果读取cookie失败则设置默认语言
                    langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                ///把语言值设置到路由值里
                filterContext.RouteData.Values["lang"] = langHeader;
            }

            /// 把设置保存进cookie
            HttpCookie _cookie = new HttpCookie(LanguageCookieName, Thread.CurrentThread.CurrentUICulture.Name);
            _cookie.Expires = DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.SetCookie(_cookie);

            base.OnActionExecuting(filterContext);

        }
    }


}
 
