using System.Web;
using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
