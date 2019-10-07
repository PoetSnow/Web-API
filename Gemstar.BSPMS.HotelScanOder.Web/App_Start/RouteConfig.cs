using System.Web.Mvc;
using System.Web.Routing;

namespace Gemstar.BSPMS.HotelScanOrder.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Gemstar.BSPMS.HotelScanOrder.Web.Controllers" }
            );

            //语言路由
            routes.MapRoute(
                name: "DefaultLang",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "Gemstar.BSPMS.HotelScanOrder.Web.Controllers" },
                 constraints: new { lang = "zh|en|ja" }
            );



        }
    }
}
