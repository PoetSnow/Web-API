using Gemstar.BSPMS.HotelScanOrder.Web.Filter;
using System.Web;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Web.Filter;

namespace Gemstar.BSPMS.HotelScanOrder.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LanguageAttribute());
            filters.Add(new LogExceptionAttribute());
        }
    }
}
