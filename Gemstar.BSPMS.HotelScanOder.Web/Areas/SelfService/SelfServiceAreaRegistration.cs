using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Areas.SelfService
{
    public class SelfServiceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SelfService";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SelfService_default",
                "SelfService/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Gemstar.BSPMS.HotelScanOrder.Web.Areas.SelfService.Controllers" }
            );

            //语言路由
            context.MapRoute(
                name: "SelfServiceLang",
                url: "{lang}/SelfServiceLang/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Gemstar.BSPMS.HotelScanOrder.Web.Areas.SelfService.Controllers" },
                 constraints: new { lang = "zh|en|ja" }
            );

        }
    }
}