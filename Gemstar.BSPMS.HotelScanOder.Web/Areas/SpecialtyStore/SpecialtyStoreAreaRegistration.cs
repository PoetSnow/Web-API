using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore
{
    public class SpecialtyStoreAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SpecialtyStore";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SpecialtyStore_default",
                "SpecialtyStore/{controller}/{action}/{id}",              
                new { action = "Index", id = UrlParameter.Optional, },
                new[] { "Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Controllers" }
            );

            //语言路由
            context.MapRoute(
                name: "SpecialtyStoreLang",
                url: "{lang}/SpecialtyStore/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Controllers" },
                constraints: new { lang = "zh|en|ja" }
            );

        }
    }
}